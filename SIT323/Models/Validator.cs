using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT323.Models
{
    public class RegExp
    {
        
    }

    public abstract class Validator
    {
        protected string location;
        public List<LogMessage> LogList { get; set; }
        protected bool IsEmpty(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = location,
                    TextMessage = "is empty"
                });
                return true;
            }
            return false;
        }
    }

    public class StringValidtor : Validator, ILogger
    {
        private string value;
        public StringValidtor(string s, string location)
        {
            LogList = new List<LogMessage>();
            this.location = location;

            IsEmpty(s);
            IsAlphabetic(s);
            value = s;
        }

        private bool IsAlphabetic(string s)
        {
            if (s.Any(x => !char.IsLetter(x)))
            {
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = location,
                    TextMessage = string.Format("word value ({0}) is not an alphabetic", value)
                });
                return false;
            }
            return true;
        }
    }


    /// <summary>
    /// Validator is a Builder 
    /// </summary>
    public class IntValidator : Validator, ILogger
    {
        protected int value;

        public IntValidator(string s, string location)
        {
            LogList = new List<LogMessage>();
            this.location = location;

            IsEmpty(s);
            IsInt(s);

            int.TryParse(s, out value);
        }

        private bool IsInt(string s)
        {
            if (s.Any(x => !char.IsDigit(x)))
            {
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = location,
                    TextMessage = string.Format("value ({0}) is not an integer", value)
                });
                return false;
            }
            return true;
        }

        public IntValidator IsInRange(int min, int max)
        {
            if (value < min || value > max)
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = location,
                    TextMessage = string.Format("size value ({0}) is not in range", value)
                });
            return this;
        }
    }
}
