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
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private CancellationToken token;

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
            int maxRequestLimit = httpClientService.GetMaxRequestLimit();
            int interval = httpClientService.GetMinInterval();

            if (semaphore != null)
            {
                logger.Debug("Destroy semaphore");
                semaphore.Dispose();
                timer.Dispose();
            }
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            semaphoreNumber = maxRequestLimit;
            logger.Debug($"Create semaphore with max = {semaphoreNumber}");
            semaphore = new SemaphoreSlim(0, semaphoreNumber);
            InitializeTimer(interval);
        }

        private void CancelTasks()
        {
            logger.Debug("Cancel tasks");
            timer?.Stop();
            tokenSource.Cancel();
        }

        private void InitializeTimer(int interval)
        {
            logger.Debug($"Initialize timer (interval = {interval}s)");
            timer = new System.Timers.Timer();
            timer.Elapsed += OnTimer;
            timer.AutoReset = true;
            timer.Interval = interval * 1000;
            timer.Enabled = true;
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
                int i = semaphoreNumber - semaphore.CurrentCount;
                if (i > 0)
                {
                    logger.Debug($"Release {i} semaphores");
                    semaphore.Release(i);
                }
            }
        }

        private void OnProcessGetRequestStarted(object sender, HttpRequestEventArgs args)
        {
            logger.Debug($"Wait for semaphore - start ({semaphore?.CurrentCount})");
            semaphore?.WaitAsync(token);
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
                tokenSource.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
