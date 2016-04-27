namespace MyTinyCollege.Migrations.CollegeMigrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyTinyCollege.DAL.SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\CollegeMigrations";
        }

        protected override void Seed(MyTinyCollege.DAL.SchoolContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //1. Add some students
            var students = new List<Student>
            {
                new Student {FirstName="Carson",
                             LastName ="Alexander",
                             EnrollmentDate=DateTime.Parse("2015-02-01"),
                             Email = "calexander@tinycollege.com"
                },
                new Student {FirstName="Alonso",
                             LastName ="Arturo",
                             EnrollmentDate=DateTime.Parse("2015-02-01"),
                             Email = "aarturo@tinycollege.com"
                },
                new Student {FirstName="John",
                             LastName ="Smith",
                             EnrollmentDate=DateTime.Parse("2015-02-01"),
                             Email = "jsmith@tinycollege.com"
                },
                new Student {FirstName="Fadi",
                             LastName ="Fakhouri",
                             EnrollmentDate=DateTime.Parse("2015-02-01"),
                             Email = "ffakhouri@tinycollege.com"
                },
                new Student {FirstName="Laura",
                             LastName ="Norman",
                             EnrollmentDate=DateTime.Parse("2015-02-01"),
                             Email = "lnorma@tinycollege.com"
                }
            };

            //Loop each student and add to database (only if thay are not already present)
            //using the AddorUpdate Method
            //The first parameter of this method specifies the property to check if a row ....
            students.ForEach(s => context.Students.AddOrUpdate(p => p.Email, s));
            context.SaveChanges();


            //2. Add some instructors
            var instructors = new List<Instructor>
            {
                new Instructor {FirstName="Marc",
                                LastName="Williams",
                                HireDate=DateTime.Parse("1996-09-01"),
                                Email = "mwilliams@faculty.tinycollege.com"
                },
                new Instructor {FirstName="Frank",
                                LastName="Beckering",
                                HireDate=DateTime.Parse("1996-07-01"),
                                Email = "fbeckering@faculty.tinycollege.com"
                }
            };
            instructors.ForEach(s => context.Instructors.AddOrUpdate(p => p.Email, s));
            context.SaveChanges();

            //3. Add some departments

            var departments = new List<Department>
            {
                new Department {Name= "Engineering", Budget = 350000,
                                StartDate = DateTime.Parse("2010-09-01"),
                                InstructorID = 1
                },
                new Department {Name= "English", Budget = 150000,
                                StartDate = DateTime.Parse("2010-09-01"),
                                InstructorID = 2
                }
            };
            departments.ForEach(s => context.Departments.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            //4. Add some courses
            var courses = new List<Course>
            {
                new Course {CourseID = 1045, Title="Chemistry", Credits=3, DepartmentID = 1},
                new Course {CourseID = 4022, Title="Physics", Credits=3, DepartmentID = 1 },
                new Course {CourseID = 3141, Title="Calculus", Credits=3, DepartmentID = 1 },
                new Course {CourseID = 2021, Title="Literature", Credits=3, DepartmentID = 2 },
            };
            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

            //5. Add some enrollments
            var enrollments = new List<Enrollment>
            {
                new Enrollment {StudentID =1, CourseID = 1045, Grade = Grade.A},
                new Enrollment {StudentID =1, CourseID = 4022, Grade = Grade.B},
                new Enrollment {StudentID =2, CourseID = 3141, Grade = Grade.C},
                new Enrollment {StudentID =2, CourseID = 1045, Grade = Grade.B},
                new Enrollment {StudentID =3, CourseID = 2021, Grade = Grade.B},
                new Enrollment {StudentID =3, CourseID = 3141, Grade = Grade.C}
            };

            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDatabase = context.Enrollemnts.Where(
                    s =>
                    s.StudentID == e.StudentID &&
                    s.course.CourseID == e.CourseID
                    ).SingleOrDefault();
                //SingleOrDefault : Returns a single, specific element of a sequence,
                //                  or a default value if no such element is found.
                //Use when expecting 0 or 1 item
                // you get 0 when 0 or 1 was expected (ok)
                // you get 1 when 0 or 1 was expected (ok)
                // you get 2 or more when 0 or 1 was expected (error)

                // Single: returns a single, specific element of a sequence 
                // Use when 1 item is expected 
                // you get 0 when 1 was expected (error)
                // you get 1 when 1 was expected (ok)
                // you get 2 or more when 1 was expected (error)

            if (enrollmentInDatabase == null)
                {
                    //enrollment was not found - add it
                    context.Enrollemnts.Add(e);
                }
            } //end of foreach
            context.SaveChanges();


        }
    }
}
