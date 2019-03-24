using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace WebDriverManagement
{
    public class WebDriverPool
    {
        private readonly object _lock = new object();
        private readonly string _browserType;

        private readonly UserDriver[] _pool;

        public WebDriverPool(int poolSize, string browserType)
        {
            var size = Math.Max(poolSize, GetWorkerNumber());
            _pool = new UserDriver[size];
            _browserType = browserType;
        }

        public IWebDriver GetDriver(string username, string testName = null)
        {
            lock (_lock)
            {
                // is there an existing available instance set up for that user?
                var existingIndex = Array.FindIndex(
                    _pool,
                    x =>
                        x != null &&
                        !x.Loaned &&
                        x.User.Equals(username ?? "", StringComparison.OrdinalIgnoreCase));

                if (existingIndex >= 0)
                {
                    _pool[existingIndex].Loaned = true;
                    _pool[existingIndex].AddTestName(testName);

                    return _pool[existingIndex].Driver;
                }

                // first look for an empty slot
                var firstFreeSlot = Array.FindIndex(
                    _pool,
                    x => x == null);

                if (firstFreeSlot < 0)
                    // otherwise fall back to any available slot
                    firstFreeSlot = Array.FindIndex(
                        _pool,
                        x => !x.Loaned);

                if (firstFreeSlot < 0) // won't happen provided pool size is greater than number of concurrent tests
                    throw new Exception("Attempted to get driver from exhausted pool.");

                // if slot is in use by a different user browser quit it first
                _pool[firstFreeSlot]?.Driver.Quit();

                var webDriver = DriverHelper.GetDriver(_browserType);
                _pool[firstFreeSlot] = new UserDriver(username ?? "", webDriver);
                _pool[firstFreeSlot].AddTestName(testName);

                return webDriver;
            }
        }

        public void ReturnDriver(IWebDriver driver, bool destroy)
        {
            lock (_lock)
            {
                var matchIndex = Array.FindIndex(_pool, x => x?.Driver == driver);

                if (matchIndex < 0)
                    throw new Exception("Should NOT come here...");

                if (destroy)
                {
                    var testnameTrack = _pool[matchIndex].GetTestName();
                    Console.WriteLine($@"The web driver has been used by tests: {testnameTrack}");

                    _pool[matchIndex]?.Driver.Quit();
                    _pool[matchIndex] = null;

                    return;
                }

                _pool[matchIndex].Loaned = false;
            }
        }

        public void CloseAllDrivers()
        {
            if (_pool == null) return;
            lock (_lock)
                Array.ForEach(_pool, p => p?.Driver?.Quit());
        }

        public string GetAssociatedUser(IWebDriver driver)
        {
            lock (_lock)
            {
                var index = Array.FindIndex(_pool, x => x?.Driver == driver);
                if (index >= 0)
                    return _pool[index].User;
            }

            return null;
        }

        public override string ToString()
        {
            lock (_lock)
            {
                var used = _pool.Where(x => x != null).ToList();
                var loaned = used.Where(x => x.Loaned);
                var unloaned = used.Where(x => !x.Loaned);

                return
                    $"Allocated: {used.Count}, loaned: {loaned.Count()}, unloaned: {unloaned.Count()}";
            }
        }

        private int GetWorkerNumber()
        {
            var parrallels = GetType().Assembly.GetCustomAttributes(typeof(ParallelizeAttribute), false);
            return parrallels.Length == 0 ? 0 : ((ParallelizeAttribute)parrallels[0]).Workers;
        }
    }
}
