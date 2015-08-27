using System.Collections.Generic;

namespace SIT323.Models
{
    public interface ILogger
    {
        List<LogMessage> LogList { get; set; }
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
    }
}