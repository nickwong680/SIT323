using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT323.Models
{

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
                    TextMessage = string.Format("word value ({0}) is not an alphabetic", s)
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
        protected int value = -1;

        public IntValidator(string s, string location)
        {
            LogList = new List<LogMessage>();
            this.location = location;

            if (!IsEmpty(s)) if(IsInt(s)) int.TryParse(s, out value);
        }

        private bool IsInt(string s)
        {
            if (s.Any(x => !char.IsDigit(x)))
            {
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = location,
                    TextMessage = string.Format("value ({0}) is not an integer", s)
                });
                return false;
            }
            return true;
        }

        public IntValidator IsInRange(int min, int max)
        {
            if (value == -1) return this;
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

    public class WordListValidator<T> : Validator, ILogger
    {
        private List<T> list;
        public WordListValidator(List<T> l, string location)
        {
            list = l;
            LogList = new List<LogMessage>();
            this.location = location;

            IsEmpty(string.Join(",", list.ToArray()));
        }

        public WordListValidator<T> IsInRange(int min, int max)
        {
            if (list.Count == 0) return this;
            if (list.Count < min || list.Count > max)
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = location,
                    TextMessage = string.Format("{0} contains ({1}) words. Expecting {2} to {3} words", location, list.Count, min, max)
                });
            return this;
        }
    }
}
