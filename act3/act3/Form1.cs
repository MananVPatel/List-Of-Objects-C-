using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace act3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        //List Declaration which olds Employee Class Objects
        List<Employee> employeeList = new List<Employee>();

        //Global Variables
        int employeeCount = 0;

       
        private void Form1_Load(object sender, EventArgs e)
        {
            bool OK;
            OK = readFile(); 

            //if OK = true
            if (OK)
            {
                injectEmployeeData(employeeCount); // insert info into the textboxes.
            }
            else
            {
                disableButtons(); //otherwise call disableBUtton function
            }
        }

        //enters information into textboxes
        void injectEmployeeData(int employeeCount)
        {

            txtIDnum.Text = employeeList[employeeCount].numID;
            txtName.Text = employeeList[employeeCount].name;
            txtDep.Text = employeeList[employeeCount].department;
            txtPos.Text = employeeList[employeeCount].position;
        }

        //reads the info from the file
        bool readFile()
        {
            bool OK = false;
            StreamReader fileIn; 
            fileIn = File.OpenText("Employees.txt");
            
            string lineIn; //holds each line from the Employees.txt

            //if file exists execute the file
            if (File.Exists("Employees.txt"))
            {
                OK = true;
                //splitter array 
                String[] splitter = new String[4];
                while (!fileIn.EndOfStream)
                {
                    Employee emp = new Employee();
                    lineIn = fileIn.ReadLine();
                    splitter = lineIn.Split('#'); // split the line at the # character
                    
                    //pass each element to the list 
                    emp.name = splitter[0] ;
                    emp.numID = splitter[1]  ;
                    emp.department = splitter[2] ;
                    emp.position = splitter[3]  ;
                    employeeList.Add(emp);
                }
            }
            else
            {
                OK = false;
                MessageBox.Show("Not able to open file!"); 
            }
            fileIn.Close(); // close file
            return OK;
        }

        //checks which textboxes are filled and returns the associated number 
        int fieldsFilled()
        {
            int ff = 0;
            if (txtIDnum.Text != string.Empty)
                ff += 8;
            if (txtName.Text != string.Empty)
                ff += 4;
            if (txtDep.Text != string.Empty)
                ff += 2;
            if (txtPos.Text != string.Empty)
                ff += 1;
            return ff;
        }

        //disables all buttons besides exit and add
        private void disableButtons()
        {
            foreach(Button b in this.Controls)
            {
                if(b.Name != "bExit" && b.Name != "bAdd")
                {
                    Enabled = false;
                }
            }
        }

        //enables the buttons listed int his function
        void enable (bool action)
        {
            bEdit.Enabled = action;
            bDelete.Enabled = action;
            bNext.Enabled = action;
            bBack.Enabled = action;
        }
        
        //checks if the form of the ID is correct
        private bool isValidID(string testID)
        {
            bool OK = false;
            string pattern = @"^I-[0-9]{4}$";
            Regex id = new Regex(pattern);
            if (id.IsMatch(testID))
                OK = true;
            return OK;
        }

        //Exit Button: Saves the modified info to Employees.txt and closes the file
        private void bExit_Click(object sender, EventArgs e)
        {
            
            StreamWriter write = new StreamWriter("Employees.txt"); // open Employees.txt to write info out to it

            //Iterate through the Employee List, concatenate the fields with # between them 
            //and overwrite the employeeList.txt file
            foreach (Employee emp in employeeList)
            {
                write.WriteLine(emp.name+'#'+emp.numID+'#'+emp.department+'#'+emp.position);    
            }
            write.Close(); // closes the files
            this.Dispose(); //close the form
        }

        //Leave event for the ID number text box
        private void txtIDnum_Leave(object sender, EventArgs e)
        {
            //check if ID already exits and if the ID is in the correct form, 
            //if neither execute the if statement
            if (!isValidID(txtIDnum.Text))
            {
                txtIDnum.BackColor = Color.OrangeRed;
                txtIDnum.Focus();
                txtIDnum.Text = "is Invalid!";
                txtName.Text = String.Empty;
                txtDep.Text = String.Empty;
                txtPos.Text = String.Empty;
            }
        }

        //ADD Button
        private void bAdd_Click(object sender, EventArgs e)
        {
            Employee addEmp = new Employee();
            //check if the ID exists, if so return index where found
            int index = employeeList.FindIndex(x => x.numID.Equals(txtIDnum.Text));
            int filled;
            filled = fieldsFilled();

            //if no userID found
            if (index == -1)
            {
                if (filled >= 12)
                {
                    //if only name and id found in textboxes, create a new employee using this info
                    if (fieldsFilled() == 12)
                    {
                        employeeList.Add(new Employee(txtName.Text, txtIDnum.Text));
                    }

                     //otherwise if all fields filled, create a new employee using all info
                    else if (fieldsFilled() == 15)
                    {
                        employeeList.Add(new Employee(txtName.Text, txtIDnum.Text, txtDep.Text, txtPos.Text));
                    }

                }
            }
            //otherwise if ID exists, search info at that index and modify.
            else
            {
                searchAndReplace();
            }
        }
        
        private void bNext_Click(object sender, EventArgs e)
        {

            //print the current employee information into the text box
            injectEmployeeData(employeeCount);

            //increment to the next employee
            employeeCount++;
           
            //if you reach the end of the list, set the employeeCount to 0 so it wraps around to first employee  
            if (employeeCount == employeeList.Count)
            {
                employeeCount = 0;
            }
            
        }

        private void bBack_Click(object sender, EventArgs e)
        {
            //if you are at the first index, set the employeeCount to the last index of the list  
            if (employeeCount == 0)
            {
                employeeCount = employeeList.Count;
            }

            //decrements to the previous employee
            employeeCount--;

            //print the current employee information into the text box
            injectEmployeeData(employeeCount);
        }

      
        //Find Button
        private void bFind_Click(object sender, EventArgs e)
        {
            //check if the ID exists, if so return index where found
            int index = employeeList.FindIndex(x => x.numID.Equals(txtIDnum.Text)); 
            
            //if ID exists, show the info associated with it.
            if(index != -1)
            {
                injectEmployeeData(index);
            }
            //otherwise, show and Error Message
            else
            {
                txtDep.Text = String.Empty;
                txtPos.Text = String.Empty;
                txtName.Text = "Not Found! To add, Fill in fields and click add!";
            }
            
        }

        //Delete Button
        private void bDelete_Click(object sender, EventArgs e)
        {
            //check if the ID exists, if so return index where found
            int index = employeeList.FindIndex(x => x.numID.Equals(txtIDnum.Text));
            
            //if all the information is availabe or part of the information is available, 
            //delete all info associated with this index. 
            if (fieldsFilled() >= 12)
                employeeList.RemoveAt(index);
        
        }

        //Edit Button
        private void bEdit_Click(object sender, EventArgs e)
        {
            enable(true); // call enable to enable certain buttons
            searchAndReplace(); // call search and replace and make modification to the current employeeList list/file
        }

        //Compares given ID and if it exists, modifies persons info and if not adds a new person to the list
        void searchAndReplace()
        {
            //check if the ID exists, if so return index where found
            int index = employeeList.FindIndex(x => x.numID.Equals(txtIDnum.Text));

            //if exists, execute the if statement
            if(index != -1)
            {
                //remove the info associated with this index
                employeeList.RemoveAt(index);

                //if department and position exist, then insert just name and ID.
                if(fieldsFilled() == 12)
                {
                    employeeList.Insert(index, new Employee(txtName.Text, txtIDnum.Text)); 
                }
               
                //otherwise add a new employee
                else
                {
                    employeeList.Add(new Employee(txtName.Text, txtIDnum.Text, txtDep.Text, txtPos.Text)); 
                }
                
            }
            //otherwise add a whole new employee
            else
            {
                //if department and position are empty, add only name and IDNum to the list
                if (fieldsFilled() == 12)
                {
                    employeeList.Add(new Employee(txtName.Text, txtIDnum.Text));
                }

                 //otherwise add a new employee
                else
                {
                    employeeList.Add(new Employee(txtName.Text, txtIDnum.Text, txtDep.Text, txtPos.Text));
                }
            }
        }



    }
}
