using System.Collections.Generic;

namespace SIT323
{
    /// <summary>
    ///     Ilogger Interface
    ///     Enforcing implemented class to hold list of message for error logging purpose
    /// </summary>
    public interface ILogger
    {
        List<LogMessage> LogList { get; set; }
        string LogListInString();
    }

    /// <summary>
    ///     Enum of level, only Error is used so far
    /// </summary>
    public enum Level
    {
        Error,
        Debug
    }

    /// <summary>
    ///     Log message class
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        ///     Location of the log, eg. in Crozzle, WordList, Validator and so on
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        ///     Level of message, eg Error, Debug etc
        /// </summary>
        public Level Level { get; set; }

        /// <summary>
        ///     The message itself in string
        /// </summary>
        public string TextMessage { get; set; }

        /// <summary>
        ///     Overrides to string for displaying log
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}   {1}    {2}", Level, Location, TextMessage);
        }
    }
}