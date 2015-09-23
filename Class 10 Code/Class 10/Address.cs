using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Class_10
{
    class Address
    {
        public string street;
        public string suburb;
        public string state;
        public int postcode;

        public Address(string st, string sub, string s, int pcode)
        {
            street = st;
            suburb = sub;
            state = s;
            postcode = pcode;
        }

        public Address ShallowCopy()
        {
            return (Address)this.MemberwiseClone();
        }

        public Address DeepCopy()
        {
            // only need a shallow copy in this case
            return this.ShallowCopy();
        }
    }
}
