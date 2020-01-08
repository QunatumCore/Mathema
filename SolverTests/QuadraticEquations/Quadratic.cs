using Mathema.Algorithms.Handlers;
using Mathema.Algorithms.Parsers;
using Mathema.Models.Equations;
using Mathema.Solver;
using NUnit.Framework;

namespace SolverTests.QuadraticEquations
{
    [TestFixture]
    public class Quadratic
    {
        [Test]
        public void Delta_gtZero()
        {
            //Arrange
            var text = "-2*x + 2*x^2 - 2 - 2*x - x^2 - 3";
            var expression = ExpressionBuilder.BuildFlat(RPNParser.Parse(text).Output).Execute();
            var equation = new Equation(text, expression, null);

            //Act
            var sut = Solver.Solve(equation);

            //Assert
            Assert.IsTrue(sut.Solutions.Count == 2);
            Assert.AreEqual(sut.Solutions[0].Count.ToNumber(), 5);
            Assert.AreEqual(sut.Solutions[1].Count.ToNumber(), -1);
        }

        [Test]
        public void Delta_Zero()
        {
            //Arrange
            var text = "x^2 - 2*x + 1";
            var expression = ExpressionBuilder.BuildFlat(RPNParser.Parse(text).Output).Execute();
            var equation = new Equation(text, expression, null);

            //Act
            var sut = Solver.Solve(equation);

            //Assert
            Assert.IsTrue(sut.Solutions.Count == 1);
            Assert.AreEqual(sut.Solutions[0].Count.ToNumber(), 1);
        }

        [Test]
        public void Delta_ltZero()
        {
            //Arrange
            var text = "x^2 + 6*x + 10";
            var expression = ExpressionBuilder.BuildFlat(RPNParser.Parse(text).Output).Execute();
            var equation = new Equation(text, expression, null);

            //Act
            var sut = Solver.Solve(equation);

            //Assert
            Assert.IsTrue(sut.Solutions.Count == 0);
        }

    }
}