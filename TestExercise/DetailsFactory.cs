using System;
using System.Collections.Generic;
using System.Linq;

namespace TestExercise
{
    public interface IDetailsFactory 
    {
        string GetDetails(Employee employee);
    }

    public class DetailsFactory : IDetailsFactory
    {
        private readonly List<Func<Employee, string>> rules;

        protected DetailsFactory(IEnumerable<Func<Employee, string>> rules)
        {
            this.rules = rules.ToList();
        }

        public string GetDetails(Employee item)
        {
            return 
                rules.Select(rule => rule.Invoke(item))
                .Where(d => d != null)
                .FirstOrDefault();
        }

        public static DetailsFactory Create(IEnumerable<Func<Employee, string>> rules)
        {
            return new DetailsFactory(rules);
        }

        public static Func<Employee, string> CreateEmployeePinDivisibilityRule(int[] pinDividers, string detail)
        {
            return
                employee =>
                    pinDividers.All(div => (employee.Pin % div == 0))
                        ? detail
                        : null;
        }
    }
}
