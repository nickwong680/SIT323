﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIT323.Models
{

    public abstract class Validator: ILogger
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

    public class StringValidtor : Validator
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
    public class IntValidator : Validator
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

    public class WordListValidator<T> : Validator
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


    public class CrozzleValidator : Validator
    {
        private Crozzle crozzle;
        public CrozzleValidator(Crozzle c)
        {
            LogList = new List<LogMessage>();
            crozzle = c;
        }

        public CrozzleValidator AreCellsSizeCorrect(int row, int columns)
        {
            if (crozzle.Width != row)
            {
                LogList.Add(new LogMessage()
                {
                    Level = Level.Error,
                    Location = "Row",
                    TextMessage = string.Format("file contains ({0}) rows. instead of {1} rows", crozzle.Width, row)
                });
            }
            for (int i = 0; i < crozzle.Width; i++)
            {
                if (crozzle[i].Length != columns)
                {
                    LogList.Add(new LogMessage()
                    {
                        Level = Level.Error,
                        Location = "Columns",
                        TextMessage = string.Format("row {0} contains ({1}) columns. instead of {2} columns", i, crozzle[i].Length, columns)
                    });
                }
            }
 
            return this;
        }
        public CrozzleValidator AreCellsValidAlphabet()
        {
            for (int i = 0; i < crozzle.Width; i++)
            {
                for (int j = 0; j < crozzle[i].Length; j++)
                {
                    string Message = null;
                    switch (crozzle[i, j])
                    {
                        case '\t':
                            Message = string.Format("cell [{0},{1}] contains ({2}), it should only contain exactly 1 alphabetic character", i, "tab", crozzle[i, j]);
                            break;
                        default:
                            if (!char.IsLetter(crozzle[i, j]) && !char.IsWhiteSpace(crozzle[i, j]))
                            {
                                Message = string.Format( "cell [{0},{1}] contains ({2}), it should only contain exactly 1 alphabetic character",  i, j, crozzle[i, j]);
                            }
                            break;
                    }
                    if (Message != null)
                    {
                        LogList.Add(new LogMessage()
                        {
                            Level = Level.Error,
                            Location = "Cell",
                            TextMessage = Message
                        });
                    }
                }
            }

//            for (int i = 0; i < crozzle.Width; i++)
//            {
//                if (crozzle[i].Length != columns)
//                {
//                    LogList.Add(new LogMessage()
//                    {
//                        Level = Level.Error,
//                        Location = "Columns",
//                        TextMessage = string.Format("row {0} contains ({1}) columns. instead of {2} columns", i, crozzle[i].Length, columns)
//                    });
//                }
//            }
            return this;
        }

    }
}
