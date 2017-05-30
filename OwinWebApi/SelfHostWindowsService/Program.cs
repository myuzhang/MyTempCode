using System;

namespace SelfHostWindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            int hour, minute;
            Console.WriteLine("Please specify the time (as format of HH:MM) you want to run the task");
            Console.Write("For example, you can put 20:30, which means 8:30PM:");
            var input = Console.ReadLine();
            TryToGetHourMinute(input, out hour, out minute);

            var business = true;//your business
            var timer = new ScheduleTimer(hour, minute, () => business);
            Console.WriteLine($"The task runs at {hour}:{minute} everyday");
            do
            {
                Console.WriteLine("\nAt anytime you can type in 'Stop' to stop the task or new time to adjust task run time from the console.\n");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                if (input.Equals("stop", StringComparison.CurrentCultureIgnoreCase))
                {
                    timer.Stop();
                    break;
                }
                TryToGetHourMinute(input, out hour, out minute);
                timer.StartAt(hour, minute);
                Console.WriteLine($"The task run at {hour}:{minute} everyday");
            } while (true);
        }

        private static void TryToGetHourMinute(string timeSet, out int hour, out int minute)
        {
            while (!ParseTimeToHourMinute(timeSet, out hour, out minute))
            {
                Console.Write("Please try to set the right time as format of HH:MM where HH is in [0-23] and MM is in [0-59]:");
                timeSet = Console.ReadLine();
            }
        }

        private static bool ParseTimeToHourMinute(string timeSet, out int hour, out int minute)
        {
            try
            {
                var hourMinutes = timeSet.Split(':');
                if (!hourMinutes.Length.Equals(2))
                    throw new ArgumentException("The time format is not correct.");

                hour = int.Parse(hourMinutes[0]);
                if (hour < 0 || hour > 23)
                    throw new ArgumentException($"The hour is {hour} which is out of range");

                minute = int.Parse(hourMinutes[1]);
                if (minute < 0 || minute > 59)
                    throw new ArgumentException($"The minute is {minute} which is out of range");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                hour = minute = 0;
                return false;
            }
            return true;
        }
    }
}
