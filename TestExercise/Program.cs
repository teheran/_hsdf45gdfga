using System;
using System.Collections.Generic;
using System.Linq;
using TestExercise.Generator;

namespace TestExercise
{
    partial class Program
    {
        //  Я, как пользователь, хочу,
        //  * чтобы рядом с пином каждого сотрудника, чей Пин делиться без остатка на 7, отображалось слово WithBonus,
        //  * если делиться на 3 - Fired,
        //  * а если делиться без остатка и на 3 и на 7, то должно выводиться только слово FiredWithBonus.
        //  * сотрудники, чей пин не делиться ни на одно из этих чисел - выводиться не должны.

        static void Main(string[] args)
        {
            var rules = new List<Func<Employee, string>>
            {
                DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 3, 7 }, "FiredWithBonus"),
                DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 3 },    "Fired"),
                DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 7 },    "WithBonus")
            };
            var detailsFactory = DetailsFactory.Create(rules);

            var employees = EmployeeWithDetails.EnumerateEmployeesWithNonEmptyDetails(new EmployeeGenerator().GetEmployees(), detailsFactory).ToList();
            employees.ForEach(item => Console.WriteLine($"Pin {item.Employee.Pin} {item.Details} {item.Employee.FirstName} {item.Employee.SecondName}"));
        }
    }
}
