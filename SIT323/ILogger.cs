using System;
using System.Collections.Generic;
using System.Linq;

namespace SIT323
{
    public interface ILogger
    {
        List<LogMessage> LogList { get; set; }

        string LogListInString();
    }

    public enum Level
    {
        Error,
        Debug,
    }

    public class LogMessage
    {
        public string Location { get; set; }
        public Level Level { get; set; }
        public string TextMessage { get; set; }

        public override string ToString()
        {
            return string.Format("{0}   {1}    {2}", Level, Location, TextMessage);
        }
    }
}