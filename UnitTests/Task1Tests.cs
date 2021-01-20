using Moq;

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TestExercise;
using TestExercise.Generator;

namespace UnitTests
{
    public class Task1Tests
    {
        [Test]
        public void EnumerateEmployeesWithNonEmptyDetails_Algorithm()
        {
            int count_total = 10;

            // Arrange
            var employees = Enumerable.Range(1, count_total).Select(i => new Employee { Pin = i }).ToList();
            var rules = new List<Func<Employee, string>>
            {
                DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 2 }, "divided_by_2")
            };
            var detailsFactory = DetailsFactory.Create(rules);

            // Act
            var employeesWithDetails = EmployeeWithDetails.EnumerateEmployeesWithNonEmptyDetails(employees, detailsFactory).ToList();

            // Assert
            Assert.AreEqual(count_total, employees.Count);
            Assert.AreEqual(count_total / 2, employeesWithDetails.Count);
        }

        [TestCase(3 * 4, "divided_by_3")]
        [TestCase(21 * 4, "divided_by_3")] // see rule order!
        [TestCase(7 * 4, "divided_by_7")]
        public void DetailsFactory_RuleOrderMatter(int pin, string expectedDetails)
        {
            // Arrange
            var rules = new List<Func<Employee, string>>
            {
                DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 3 },    "divided_by_3"),
                DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 3, 7 }, "divided_by_3_and_7"),
                DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 7 },    "divided_by_7")
            };
            var detailsFactory = DetailsFactory.Create(rules);
            var employee = new Employee { Pin = pin };

            // Act
            var details = detailsFactory.GetDetails(employee);

            // Assert
            Assert.AreEqual(expectedDetails, details);
        }

        [TestCase(3 * 4, "divided_by_3")]
        [TestCase(7 * 4, "divided_by_7")]
        [TestCase(21 * 4, "divided_by_3_and_7")]
        [TestCase(1, null)]
        public void DetailsFactory_ContractWithNull(int pin, string expectedDetails)
        {
            // Arrange
            var rules = new List<Func<Employee, string>>
            {
                DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 3, 7 }, "divided_by_3_and_7"),
                DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 3 },    "divided_by_3"),
                DetailsFactory.CreateEmployeePinDivisibilityRule(new[] { 7 },    "divided_by_7")
            };
            var detailsFactory = DetailsFactory.Create(rules);
            var employee = new Employee { Pin = pin };

            // Act
            var details = detailsFactory.GetDetails(employee);

            // Assert
            Assert.AreEqual(expectedDetails, details);
        }
    }
}