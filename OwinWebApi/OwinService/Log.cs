using System;
using System.IO;

namespace OwinService
{
    public class Log
    {
        public enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        private static Log _instance;
        
        private StreamWriter _sw;

        private Log()
        {
            LogFile = "log.txt";
            _sw = new StreamWriter(LogFile, true);
            _sw.WriteLine("The log starts here:");
            _sw.Flush();
            _sw.Close();
        }

        public string LogFile { get; set; }

        public static Log Instance => _instance ?? (_instance = new Log());

        public void WriteLine(string log, LogLevel level)
        {
            _sw = new StreamWriter(LogFile, true);
            _sw.WriteLineAsync($"[{LogTime()} {level}] ==> {log}");
            _sw.Flush();
            _sw.Close();
        }

        public void Write(string log, LogLevel level)
        {
            _sw = new StreamWriter(LogFile, true);
            _sw.WriteAsync($"[{LogTime()} {level}] ==> {log}");
            _sw.Flush();
            _sw.Close();
        }

        private string LogTime()
        {
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            string sHour = DateTime.Now.Hour.ToString();
            string sMinute = DateTime.Now.Minute.ToString();
            string sSecond = DateTime.Now.Second.ToString();
            return $"{sDay}/{sMonth}/{sYear}_{sHour}:{sMinute}:{sSecond}";
        }
    }
}
