using System;
using System.Collections.Generic;
using System.Linq;
using TestExercise.Generator;

namespace TestExercise
{
    partial class Program
    {
        // Задача 1.
        //  Я, как пользователь, хочу,
        //  * чтобы рядом с пином каждого сотрудника, чей Пин делиться без остатка на 7, отображалось слово WithBonus,
        //  * если делиться на 3 - Fired,
        //  * а если делиться без остатка и на 3 и на 7, то должно выводиться только слово FiredWithBonus.
        //  * сотрудники, чей пин не делиться ни на одно из этих чисел - выводиться не должны.

        // Задача 2.
        //  Бизнесу понравилась решение вашей предыдущей задачи.И теперь они хотят использовать аналогичную логику в другом месте.
        //  Я, как пользователь, хочу, чтобы помимо 7 и 3, аналогичная логика работала для чисел 4 и 5. 
        //  * На 4 должно выводится слово WithBonus,
        //  * на 5 Fired.
        //  * на 4 и 5 должен выводится FiredWithBonus
        //
        //  Логика для чисел 7 и 3 должна сохраниться. 
        //  Для примера для 12 в консоли должно выводиться вот так:
        //  Pin 12 (Fired) Abigail Alexander
        //  Pin 12 (WithBonus) Abigail Alexander

        static void Main(string[] args)
        {
            // А если Пин делиться на и (3,7) и на (4,5) что требуется выводить? 
            // FIXME: FiredWithBonus дважды? 
            //
            // А что для 90? 
            // FIXME: Fired дважды верно? 
            // Pin 90(Fired) Theresa Sanders
            // Pin 90(Fired) Theresa Sanders
            var detailsFactory_3_7 = DetailsFactory.Create(
                new List<Func<Employee, string>>
                {
                    DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 3, 7 }, "FiredWithBonus"),
                    DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 3 },    "Fired"),
                    DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 7 },    "WithBonus"),
                });

            var detailsFactory_4_5 = DetailsFactory.Create(
                new List<Func<Employee, string>>
                {
                    DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 4, 5 }, "FiredWithBonus"),
                    DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 4 },    "WithBonus"),
                    DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 5 },    "Fired"),
                });

            var employees = EmployeeWithDetails.EnumerateEmployeesWithNonEmptyDetails(new EmployeeGenerator().GetEmployees(), detailsFactory_3_7, detailsFactory_4_5).ToList();
            employees.ForEach(item => Console.WriteLine($"Pin {item.Employee.Pin} ({item.Details}) {item.Employee.FirstName} {item.Employee.SecondName}"));
        }
    }
}
