using System.Collections.Generic;

namespace MVCControlsExtensions.UnitTests
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ClassName { get; set; }

        public string Standerd { get; set; }

        public StudentClass Class { get; set; }

        public IList<Result> Results { get; set; }
    }

    public class StudentClass
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Result
    {
        public string Subject { get; set; }
        public int Mark { get; set; }
    }
}