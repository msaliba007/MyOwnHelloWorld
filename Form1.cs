using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HelloLinQ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Student
        {
            public int StudentID {get; set;}
            public string StudentName { get; set; }
            public int Age { get; set; }
            public int DepartmentId { get; set; }
        }

        public class Department
        {
            public int DepartmentId { get; set; }
            public string DepartmentName { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText("Student List:" + "\r\n");
            List<Student> studList = PopulateStudentList();
            foreach (Student stud in studList)
            {
                richTextBox1.AppendText("Id: " + "\t" + stud.StudentID.ToString() +  "\t" + " Name: " +stud.StudentName + "\t" +
                   "Age: " + stud.Age.ToString() + "\t" + "DepartmentId: " +stud.DepartmentId.ToString()  +"\r\n");
            }

            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("Department List:" + "\r\n");
            List<Department> deptList = PopulateDepartment();
            foreach(Department dept in deptList)
            {
                richTextBox1.AppendText("DepartmentID: " + "\t" + dept.DepartmentId.ToString() + "\t" + "Department Name: " + dept.DepartmentName +"\r\n");
            }
            textBox1.Refresh();
        }

        private List<Student> PopulateStudentList()
        {
            List<Student> studentList = new List<Student>()
            {
                new Student(){StudentID=1, StudentName="Ron",Age=15, DepartmentId=1},
                new Student() { StudentID = 2, StudentName = "Mary",  Age = 21, DepartmentId=1 } ,
                new Student() { StudentID = 3, StudentName = "Bill",  Age = 18, DepartmentId=2 } ,
                new Student() { StudentID = 3, StudentName = "William",  Age = 18, DepartmentId=2 } ,
                new Student() { StudentID = 4, StudentName = "Ram" , Age = 20, DepartmentId=3} ,
                new Student() { StudentID = 5, StudentName = "John" , Age = 15, DepartmentId=3 }
            };
            return studentList;
        }

        private List<Department> PopulateDepartment()
        {
            List<Department> departmentList = new List<Department>()
            {
                new Department(){DepartmentId=1, DepartmentName="Math"},
                new Department(){DepartmentId=2, DepartmentName="Physics"},
                new Department(){DepartmentId=3, DepartmentName="Chemistry"}
            };


            return departmentList;
        }

        private void btnListStudents_Click(object sender, EventArgs e)
        {
            List<Student> studList = PopulateStudentList();

            // query syntax
            var teenAgeStudents = from s in studList
                                  where s.Age >= 13 && s.Age <= 19
                                  orderby s.StudentName
                                  select s;
            richTextBox1.Clear();
            richTextBox1.AppendText("Teeen age Student List using query syntax:" + "\r\n");

            foreach (Student stud in teenAgeStudents)
            {
                richTextBox1.AppendText("Id: " + "\t" + stud.StudentID.ToString() + "\t" + " Name: " + stud.StudentName + "\t" +
                   "Age: " + stud.Age.ToString() + "\t" + "DepartmentId: " + stud.DepartmentId.ToString() + "\r\n");
            }

            // method syntax
            var teenStudent = studList.Where(s => s.Age >= 13 && s.Age <= 19).OrderBy(s=> s.StudentName);
            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("Teeen age Student List using method syntax:" + "\r\n");
            foreach (Student studM in teenStudent)
            {
                richTextBox1.AppendText("Id: " + "\t" + studM.StudentID.ToString() + "\t" + " Name: " + studM.StudentName + "\t" +
                   "Age: " + studM.Age.ToString() + "\t" + "DepartmentId: " + studM.DepartmentId.ToString() + "\r\n");
            }
        }

        private void btnGroupBy_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.AppendText("Grouping by Age using query syntax: " + "\r\n");
            List<Student> studentList = PopulateStudentList();
 
            var groupResult = from stud in studentList
                              orderby stud.Age, stud.StudentName
                              group stud by stud.Age;
                              
                              //into g
                              //where g.Count()>1
                              //select g;
                              
            // List groups and each student in the group
            foreach (var group in groupResult)
            {
                richTextBox1.AppendText("Age Group: " + "\t" + group.Key.ToString() + "\r\n");
                foreach (Student s in group)
                {
                    richTextBox1.AppendText("Student Name: " + "\t" + s.StudentName + "\r\n");
                }
            }

            richTextBox1.AppendText("\r\n");
            richTextBox1.AppendText("Grouping by Age using method syntax: " + "\r\n");
            var groupResult2 = studentList.OrderBy(s => s.Age).ThenBy(s=>s.StudentName).GroupBy(s => s.Age);
            foreach (var grp in groupResult2)
            {
                richTextBox1.AppendText("Age Group: " + "\t" + grp.Key.ToString() + "\r\n");
                foreach (Student s in grp)
                {
                    richTextBox1.AppendText("Student Name: " + "\t" + s.StudentName + "\r\n");
                }
            }
        }

        private void btnInnerJoin_Click(object sender, EventArgs e)
        {
            /* Query Syntax for inner join
            var innerJoinOrderBy =
            from c in categories
            join p in products on c.ID equals p.CategoryID
            orderby c.Name, p.Name
            select new {
                Category = c.Name,
                Product = p.Name
            };
            */


            List<Student> studentList = PopulateStudentList();
            List<Department> departmentList = PopulateDepartment();

            var innerJoin = studentList.Join( // outer selector
                            departmentList, // innter selector
                            student => student.DepartmentId,
                            department => department.DepartmentId,
                            (student,department) => new 
                            {
                                StudentName = student.StudentName,
                                DepartmentName = department.DepartmentName
                            });
            richTextBox1.Clear();
            foreach (var obj in innerJoin.OrderBy(i=> i.StudentName).ThenBy(i=>i.DepartmentName))
            {
                richTextBox1.AppendText("Student Name: " + "\t" + obj.StudentName + "\t" +
                "Department: " + "\t" +  obj.DepartmentName + "\r\n");
            }
        }
    }
    
}
