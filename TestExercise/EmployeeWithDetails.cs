using System.Collections.Generic;
using System.Linq;

namespace TestExercise
{
    public class EmployeeWithDetails 
    {
        public Employee Employee { get; set; }
        public string Details { get; set; }

        public static IEnumerable<EmployeeWithDetails> EnumerateEmployeesWithNonEmptyDetails(IEnumerable<Employee> employees, IDetailsFactory detailsFactory)
        {
            return EnumerateEmployeesWithNonEmptyDetails(employees, new[] { detailsFactory });
        }

        public static IEnumerable<EmployeeWithDetails> EnumerateEmployeesWithNonEmptyDetails(IEnumerable<Employee> employees, params IDetailsFactory[] detailsFactores)
        {
            foreach (var e in employees)
            {
                foreach(var r in detailsFactores)
                {
                    var details = r.GetDetails(e);
                    if (!string.IsNullOrEmpty(details))
                    {
                        yield return new EmployeeWithDetails
                        {
                            Employee = e,
                            Details = details
                        };
                    }
                }
            }
        }
    }

}
