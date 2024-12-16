using labb3.Data;

namespace labb3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Labb3");
            getStudents();

        }
        static void getStudents()
        {
            using (var context = new SchoolDBContext())
            {
                var employees = context.Students.Select(e => e);
                Console.Clear();
                Console.WriteLine("Students:");
                foreach (var employee in employees)
                {
                    Console.WriteLine($"\n{employee.FirstName} {employee.LastName}");
                }
            }
        }
    }
}