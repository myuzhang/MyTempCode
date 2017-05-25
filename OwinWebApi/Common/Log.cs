using System;
using System.Text;

namespace Common
{
    public class Log
    {
        private static Log _instance;

        private readonly StringBuilder _logInfo;

        private readonly StringBuilder _logAction;

        private readonly StringBuilder _logError;

        private Log()
        {
            _logInfo = new StringBuilder();
            _logAction = new StringBuilder();
            _logError = new StringBuilder();
            _logInfo.AppendLine();
        }

        public static Log Instance => _instance ?? (_instance = new Log());

        public string LogInfo => _logInfo.ToString();

        public string LogAction => _logAction.ToString();

        public string LogError => _logError.ToString();

        public void WriteInfo(string log)
        {
            _logInfo.Append("<li>");
            _logInfo.Append(log);
            _logInfo.Append("</li>");
        }

        public void WriteInfo(string log, Func<bool> func)
        {
            if (func != null && func())
            {
                WriteInfo(log);
            }
        }


        public void WriteAction(string log)
        {
            _logAction.Append("<li>");
            _logAction.Append(log);
            _logAction.Append("</li>");
        }

        public void WriteAction(string log, Func<bool> func)
        {
            if (func != null && func())
            {
                WriteAction(log);
            }
        }

        public void WriteError(string log)
        {
            _logError.Append("<li>");
            _logError.Append(log);
            _logError.Append("</li>");
        }

        public void WriteError(string log, Func<bool> func)
        {
            if (func != null && func())
            {
                WriteError(log);
            }
        }

        public string GetEmailLog()
        {
            var info = _logInfo.ToString();
            var act = _logAction.ToString();
            var err = _logError.ToString();
            return $@"<h4>Hi there,</h4>
<h4>Please note that you should get this email everyday, otherwise please contact your IT department.</h4>
<h4>Below is the log for email sending to the patient</h4>
<p>-----This is the email sending information for today:-----</p>
<ol>{info}</ol>
<br />
<p>-----This is the email sending actions for today:-----</p>
<ol>{act}</ol>
<br />
<p>-----This is the email sending errors for today:-----</p>
<ul>{err}</ul>";
        }
    }
}
