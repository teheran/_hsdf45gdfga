using System.Collections.Generic;

namespace TestExercise
{
    public class EmployeeWithDetails 
    {
        public Employee Employee { get; set; }
        public string Details { get; set; }

        public static IEnumerable<EmployeeWithDetails> EnumerateEmployeesWithNonEmptyDetails(IEnumerable<Employee> employees, IDetailsFactory detailsFactory)
        {
            foreach (var e in employees)
            {
                var details = detailsFactory.GetDetails(e);
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
