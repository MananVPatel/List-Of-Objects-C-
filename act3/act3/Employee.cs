using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace act3
{
    //Employee Class
    class Employee
    {
        //Private Instance Variables
        private String _name;
        private String _department;
        private String _position;
        private String _numID;

        //Constructor to intialize private variables
        public Employee(String name, String numID, String department, String position)
        {
            _name = name;
            _numID = numID;
            _department = department;
            _position = position;
        }

        //Constructor to intialize only two variables
        public Employee(String name, String numID)
        {
            _name = name;
            _numID = numID;
            _position = String.Empty;
            _department = String.Empty;
        }

        //empty constructor to intialize all variables to an empty string
        public Employee()
        { 
            _name = String.Empty;
            _numID = String.Empty;
            _position = String.Empty;
            _department = String.Empty;
        }
        
        
        /* Setters and getters for each of the instance variables defined below*/
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }
        public String department
        {
            get { return _department; }
            set { _department = value; }
        }
        public String position
        {
            get { return _position; }
            set { _position = value; }
        }
        public String numID
        {
            get { return _numID; }
            set { _numID = value; }
        }
    }
}
