using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningOperators
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Person person = new Person("Artur", "Valdna");
            //var now1 = DateTimeOffset.UtcNow.ToFileTime();

            //RemoveLastName(person);
            //var now2 = DateTimeOffset.UtcNow.ToFileTime();

            //Console.WriteLine($"{now2} {now1}");

            //person = new Person("Artur", "Valdna");
            //now1 = DateTimeOffset.UtcNow.ToFileTime();

            //RemoveLastName(ref person);
            //now2 = DateTimeOffset.UtcNow.ToFileTime();

            //Console.WriteLine($"{now2} {now1}");
            //Generator.GeneratePerson(out Person artur);
            //artur.PrintOutInfo();
            //PrintOutName(in artur);

            object number = 10;
            
            if(number is int n)
            {
                Console.WriteLine(n.ToString());
            }

        }

        private static void RemoveLastName(ref Person person) => person.LastName = "";
        private static void RemoveLastName(Person person) => person.LastName = "";
        private static void PrintOutName(in Person person) => Console.WriteLine(person.FirstName);
    }
    public class Person
    {
        public Guid Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Person(string firstName, string lastName)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
        }

        public void PrintOutInfo() => Console.WriteLine($"{Id}. {FirstName} {LastName}");
    }
    public static class Generator
    {
        public static void GeneratePerson(out Person person) => person = new Person("Artur", "Valdna");
    }
}
