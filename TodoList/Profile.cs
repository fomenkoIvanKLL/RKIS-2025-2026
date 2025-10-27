using System;

namespace TodoList
{
    public class Profile
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int BirthYear { get; private set; }

        public Profile(string firstName, string lastName, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public string GetInfo()
        {
            int age = DateTime.Now.Year - BirthYear;
            return $"{FirstName} {LastName}, возраст {age}";
        }

        public void UpdateProfile(string firstName, string lastName, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }
    }
}