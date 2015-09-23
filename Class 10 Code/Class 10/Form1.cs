using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Class_10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void shallowCopyButton_Click(object sender, EventArgs e)
        {
            Address mainStreet = new Address("8 Main St", "Highton", "VIC", 3216);
            Employee peter = new Employee("Peter", "Smith", 80000, mainStreet);
            Employee shallowPeter = peter.ShallowCopy();

            Console.WriteLine(peter.address.street);
            Console.WriteLine(shallowPeter.address.street);

            peter.address.street = "123 High Rd";

            Console.WriteLine(peter.address.street);
            Console.WriteLine(shallowPeter.address.street);
        }

        private void deepCopyButton_Click(object sender, EventArgs e)
        {
            Address mainStreet = new Address("8 Main St", "Highton", "VIC", 3216);
            Employee peter = new Employee("Peter", "Smith", 80000, mainStreet);
            Employee deepPeter = peter.DeepCopy();

            Console.WriteLine(peter.address.street);
            Console.WriteLine(deepPeter.address.street);

            peter.address.street = "123 High Rd";

            Console.WriteLine(peter.address.street);
            Console.WriteLine(deepPeter.address.street);
        }

        private void parametersButton_Click(object sender, EventArgs e)
        {
            Address mainStreet = new Address("8 Main St", "Highton", "VIC", 3216);
            Employee peter = new Employee("Peter", "Smith", 80000, mainStreet);

            doSomething(peter);
            Console.WriteLine(peter.firstname);
            Console.WriteLine(peter.address.street);
        }

        private void doSomething(Employee emp)
        {
            Console.WriteLine(emp.address.street);
            emp.firstname = "John";
            emp.address.street = "123 High Rd";
            Console.WriteLine(emp.firstname);
            Console.WriteLine(emp.address.street);
        }

        private void parametersShallowCopyButton_Click(object sender, EventArgs e)
        {
            Address mainStreet = new Address("8 Main St", "Highton", "VIC", 3216);
            Employee peter = new Employee("Peter", "Smith", 80000, mainStreet);

            doSomething(peter.ShallowCopy());
            Console.WriteLine(peter.firstname);
            Console.WriteLine(peter.address.street);
        }

        private void parametersDeepCopyButton_Click(object sender, EventArgs e)
        {
            Address mainStreet = new Address("8 Main St", "Highton", "VIC", 3216);
            Employee peter = new Employee("Peter", "Smith", 80000, mainStreet);

            doSomething(peter.DeepCopy());
            Console.WriteLine(peter.firstname);
            Console.WriteLine(peter.address.street);
        }
    }
}
