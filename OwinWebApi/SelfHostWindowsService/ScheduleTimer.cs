using System;
using System.Timers;

namespace SelfHostWindowsService
{
    public class ScheduleTimer
    {
        private Timer _timer;

        private readonly Func<bool> _func;

        private DateTime _nowTime;

        private DateTime _scheduledTime;

        public ScheduleTimer(int hour, int minute, Func<bool> func)
        {
            _func = func;
            _nowTime = DateTime.Now;
            _scheduledTime = new DateTime(_nowTime.Year, _nowTime.Month, _nowTime.Day, hour, minute, 0, 0);
            ScheduleTask();
        }

        public void ScheduleTask()
        {
            _nowTime = DateTime.Now;
            if (_nowTime > _scheduledTime)
            {
                _scheduledTime = _scheduledTime.AddDays(1);
            }

            Console.WriteLine($"Task next run is at {_scheduledTime}");

            double tickTime = (double)(_scheduledTime - _nowTime).TotalMilliseconds;
            _timer = new Timer(tickTime);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        public void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine($"Task run is at {DateTime.Now}");
            _timer.Stop();
            bool result = _func();
            Console.WriteLine($"Task run finished. Succeeded:{result} \n");
            ScheduleTask();
        }

        public void Stop() => _timer.Stop();

        public void Start() => _timer.Start();

        public void StartAt(int hour, int minute)
        {
            _nowTime = DateTime.Now;
            _scheduledTime = new DateTime(_nowTime.Year, _nowTime.Month, _nowTime.Day, hour, minute, 0, 0);
            ScheduleTask();
        }
    }
}
