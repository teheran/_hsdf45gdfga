using Moq;

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TestExercise;
using TestExercise.Generator;

namespace UnitTests
{
    public class Tests
    {
        [Test]
        public void GetEmployee_ReturnEmployee()
        {
            // Arrange
            var name = new Mock<INameGenerator>();
            name.SetupGet(r => r.NextName).Returns("TestName");
            var secondName = new Mock<INameGenerator>();
            secondName.SetupGet(r => r.NextName).Returns("TestFamilie");

            var generator = new EmployeeGenerator(name.Object, secondName.Object);
            // Act
            var employee = generator.GetEmployee(1);
            // Assert
            Assert.AreEqual(employee.Pin, 1);
            Assert.AreEqual(employee.FirstName, "TestName");
            Assert.AreEqual(employee.SecondName, "TestFamilie");
        }

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

        [TestCase(3*4,  "divided_by_3")]
        [TestCase(21*4, "divided_by_3")] // see rule order!
        [TestCase(7*4,  "divided_by_7")]
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

        [TestCase(3*4,  "divided_by_3")]
        [TestCase(7*4,  "divided_by_7")]
        [TestCase(21*4, "divided_by_3_and_7")]
        [TestCase(1,  null)]
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