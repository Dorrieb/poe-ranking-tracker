using PoeRankingTracker.Events;
using PoeRankingTracker.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace PoeRankingTracker.Services
{
    public interface ISemaphoreService
    {
        void CreateSemaphore(int requestLimit, int interval);
        void CancelTasks();
    }

    public class SemaphoreService : ISemaphoreService, IDisposable
    {
        private System.Timers.Timer timer;
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private SemaphoreSlim semaphore;
        private int semaphoreNumber;
        private List<RuleApi> rulesState;
        private readonly IHttpClientService httpClientService;

        public SemaphoreService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
            httpClientService.ProcessGetRequestStarted += OnProcessGetRequestStarted;
            httpClientService.ProcessGetRequestEnded += OnProcessGetRequestEnded;
            httpClientService.UpdateRulesStarted += OnUpdateRulesStarted;
            httpClientService.UpdateRulesEnded += OnUpdateRulesEnded;
        }

        public void CreateSemaphore(int requestLimit, int interval)
        {
            if (semaphore != null)
            {
                logger.Debug("Destroy semaphore");
                semaphore.Dispose();
            }
            semaphoreNumber = requestLimit;
            logger.Debug($"Create semaphore with max = {semaphoreNumber}");
            semaphore = new SemaphoreSlim(0, semaphoreNumber);
            InitializeTimer(interval);
        }

        public void CancelTasks()
        {
            logger.Debug("Cancel tasks");
            timer.Stop();
            if (semaphore != null)
            {
                semaphore.Dispose();
                semaphore = null;
            }
        }

        private void InitializeTimer(int interval)
        {
            if (timer == null)
            {
                OnTimer(null, null);
                timer = new System.Timers.Timer();
                timer.Elapsed += OnTimer;
                timer.AutoReset = true;
                timer.Interval = interval * 1000;
                timer.Enabled = true;
            }
        }

        private void OnTimer(Object source, ElapsedEventArgs e)
        {
            int timeout = httpClientService.GetTimeout();
            if (timeout == 0)
            {
                ReleaseSemaphore();
            }
            else
            {
                timer.Stop();
                Thread.Sleep(timeout * 1000);
                ReleaseSemaphore();
                timer.Start();
            }
        }
        
        private void ReleaseSemaphore()
        {
            if (semaphore != null)
            {
                int i = semaphoreNumber - semaphore.CurrentCount - rulesState[0].RequestLimit;
                if (i > 0)
                {
                    semaphore.Release(i);
                }
                else if (semaphore.CurrentCount == 0)
                {
                    rulesState[0].RequestLimit = 0;
                    OnTimer(null, null);
                }
            }
        }

        private void OnProcessGetRequestStarted(object sender, HttpRequestEventArgs args)
        {
            semaphore.Wait();
        }

        private void OnProcessGetRequestEnded(object sender, HttpRequestEventArgs args)
        {
            ReleaseSemaphore();
        }

        private void OnUpdateRulesStarted(object sender, RulesEventArgs args)
        {
            timer.Stop();
        }

        private void OnUpdateRulesEnded(object sender, RulesEventArgs args)
        {
            rulesState = args.RulesState;
            timer.Start();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                timer?.Dispose();
                semaphore?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
