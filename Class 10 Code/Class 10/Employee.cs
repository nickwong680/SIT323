using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Class_10
{
    class Employee
    {
        public string firstname;
        public string surname;
        public int salary;
        public Address address;

        public Employee(string fname, string sname, int sal, Address addr)
        {
            firstname = fname;
            surname = sname;
            salary = sal;
            address = addr;
        }

        public Employee ShallowCopy()
        {
            return (Employee)this.MemberwiseClone();
        }

        public Employee DeepCopy()
        {
            Employee emp = (Employee)this.MemberwiseClone();
            emp.address = emp.address.ShallowCopy();
            return emp;
        }
    }
}
