using Mathema.Algorithms.Handlers;
using Mathema.Algorithms.Helpers;
using Mathema.Algorithms.Parsers;
using Mathema.Interfaces;
using NUnit.Framework;


namespace FlatExpressionTests.FlatAddTests.BasicOperations
{
    [TestFixture]
    public class AddTests
    {
        [Test]
        public void Add_FlatAdd_Variable()
        {
            //Arrange
            var text = "2 + x + x^2 + x";
            var expected = RPNParser.Parse("x*x + 2*x + 2");

            //Act
            var rpn = RPNParser.Parse(text);
            var expr = ExpressionBuilder.BuildFlat(rpn.Output).Execute();
            var actual = RPNParser.Parse(expr.ToString());

            //Assert
            Assert.IsTrue(RPNComparer.Compare(expected.Output, actual.Output));
        }

        [Test]
        public void Add_FlatAdd_FlatAdd_TheSame()
        {
            //Arrange
            var test = "2 + x + 2 * (x + 2)";
            var expText = " 3 * (2+x)";
            var expected = RPNParser.Parse(expText);

            //Act
            var rpn = RPNParser.Parse(test);
            var expr = ExpressionBuilder.BuildFlat(rpn.Output).Execute();
            var actual = RPNParser.Parse(expr.ToString());

            //Assert
            Assert.IsTrue(RPNComparer.Compare(expected.Output, actual.Output), $"Expected: {expText} but was {expr.ToString()}");
        }
    }
}