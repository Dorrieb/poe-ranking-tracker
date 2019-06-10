using PoeApiClient.Events;
using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Timers;

namespace PoeApiClient.Services
{
    public interface ISemaphoreService
    {
        void CreateSemaphore();
    }

    public class SemaphoreService : ISemaphoreService, IDisposable
    {
        private System.Timers.Timer timer;
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private SemaphoreSlim semaphore;
        private int semaphoreNumber;
        private readonly IHttpClientService httpClientService;

        public SemaphoreService(IHttpClientService httpClientService)
        {
            Contract.Requires(httpClientService != null);

            this.httpClientService = httpClientService;
            httpClientService.ProcessGetRequestStarted += OnProcessGetRequestStarted;
            httpClientService.ProcessGetRequestEnded += OnProcessGetRequestEnded;
            httpClientService.UpdateRulesStarted += OnUpdateRulesStarted;
            httpClientService.UpdateRulesEnded += OnUpdateRulesEnded;
            httpClientService.CancelRequested += OnCancelRequested;
        }

        public void CreateSemaphore()
        {
            int currentRequestLimit = httpClientService.GetCurrentRequestLimit();
            int maxRequestLimit = httpClientService.GetMaxRequestLimit();
            int interval = httpClientService.GetInterval();

            if (semaphore != null)
            {
                logger.Debug("Destroy semaphore");
                semaphore.Dispose();
            }
            semaphoreNumber = maxRequestLimit;
            int initialCount = maxRequestLimit - currentRequestLimit;
            logger.Debug($"Create semaphore with initial = {initialCount} and max = {semaphoreNumber}");
            semaphore = new SemaphoreSlim(initialCount, semaphoreNumber);
            InitializeTimer(interval);
        }

        private void CancelTasks()
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
                int requestLimit = httpClientService.GetCurrentRequestLimit();
                int i = semaphoreNumber - semaphore.CurrentCount - requestLimit;
                if (i > 0)
                {
                    logger.Debug($"Release {i} semaphores");
                    semaphore.Release(i);
                }
                else if (semaphore.CurrentCount == 0)
                {
                    int timeout = httpClientService.GetTimeout();
                    if (timeout > 0)
                    {
                        Thread.Sleep(timeout * 1000);
                    }
                    logger.Debug("Release all semaphores");
                    semaphore.Release(semaphoreNumber);
                }
            }
        }

        private void OnProcessGetRequestStarted(object sender, HttpRequestEventArgs args)
        {
            logger.Debug($"Wait for semaphore - start ({semaphore?.CurrentCount})");
            semaphore?.Wait();
            logger.Debug($"Wait for semaphore - end ({semaphore?.CurrentCount})");
        }

        private void OnProcessGetRequestEnded(object sender, HttpRequestEventArgs args)
        {
        }

        private void OnUpdateRulesStarted(object sender, RulesEventArgs args)
        {
            timer?.Stop();
        }

        private void OnUpdateRulesEnded(object sender, RulesEventArgs args)
        {
            timer?.Start();
        }

        private void OnCancelRequested(object sender, EventArgs args)
        {
            CancelTasks();
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
