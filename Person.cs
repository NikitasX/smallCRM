using System;

namespace Ergasia
{
    class Person
    {
        protected string astring;

        private int age_;

        public int Age
        {
            get {
                return age_;
            }
            set {
                if (value >= 1 && value < 120) {
                    age_ = value;
                    return;
                }

                throw new ArgumentOutOfRangeException(
                    "age", "Age must be in range of 1 and 120");
            }
        }
        public string LastName { get; private set; }

        public string FirstName { get; set; }

        public Person (string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName)) {
                throw new ArgumentNullException($"{nameof(lastName)}");
            }
            LastName = lastName;
        }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        public bool IsAdult()
        {
            return Age >= 18;
        }
    }

}
