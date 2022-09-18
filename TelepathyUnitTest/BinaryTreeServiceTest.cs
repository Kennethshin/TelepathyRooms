using System.Text.RegularExpressions;
using TelepathyBinaryTree.Services.BinaryTreeService;

namespace TelepathyUnitTest
{
    [TestClass]
    public class BinaryTreeServiceTest
    {
        BinaryTreeService _binaryTreeService;

        [TestInitialize]
        public void TestInitialize()
        {
            _binaryTreeService = new BinaryTreeService();
        }

        [TestMethod]
        public void Get_PostFixExpression_Success()
        {
            //Arrange
            String exp = $"2*(4+3{Operators.MathSymbolMinus}(6+2))*5";

            //Act
            var expected = $"243+62+{Operators.MathSymbolMinus}*5*";
            var stringList = _binaryTreeService.ConvertToPostfix(exp);
            string result = "";

            foreach (string val in stringList)
            {
                result += val;
            }

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Calculate_BinarytreeValue_Success()
        {
            //Arrange
            String postFixExp = $"2 4 3 + 6 2 + {Operators.MathSymbolMinus} * 5 *";
            List<string> mockList = new List<string>()
            {
                "2", "4", "3", "+", "6", "2", "+", {Operators.MathSymbolMinus}, "*", "5", "*"
            };

            postFixExp = Regex.Replace(postFixExp, @"\s+", "");

            //Act
            var expected = -10;
            var result = _binaryTreeService.CalculateBinaryTree(mockList);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Calculate_BinarytreeMultipleDigitPositiveValue_Success()
        {
            //Arrange
            List<string> mockList = new List<string>()
            {
                "2", "45", "3", "+", "6", "2", "+", {Operators.MathSymbolMinus}, "*", "5", "*"
            };

            //Act
            var expected = 400;
            var result = _binaryTreeService.CalculateBinaryTree(mockList);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Calculate_NegativeDigit_Success()
        {
            //Arrange
            //$"2*(-4+3{Operators.MathSymbolMinus}(6+2))*5";
            string negVal = Operators.Negative + "4";
            List<string> mockList = new List<string>()
            {
                "2", negVal, "3", "+", "6", "2", "+", {Operators.MathSymbolMinus}, "*", "5", "*"
            };

            //Act
            var expected = -90;
            var result = _binaryTreeService.CalculateBinaryTree(mockList);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Calculate_NegativeDigitAndMultipleDigits_Success()
        {
            //Arrange
            //inFix $"((15 ÷ (7 − (1 + 1) ) ) × -3 ) − (2 + (1 + 1))"
            // expected Post 15 7 1 1 + - / -3 x 2 1 1 + + -
            string negVal = Operators.Negative + "4";
            List<string> mockList = new List<string>()
            {
                "15", "7", "1", "1", "+",
                "−", "÷", "-3", "×", "2",
                "1", "1", "+", "+", "−"
             };

            //Act
            var expected = -13;
            var result = _binaryTreeService.CalculateBinaryTree(mockList);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertPostfix_MultiDigits_Success()
        {
            //Arrange
            string exp = $"2 * (4 5 + 3 {Operators.MathSymbolMinus} (6 + 2)) * 5";
            List<string> mockList = new List<string>()
            {
                "2", "45", "3", "+", "6", "2", "+", {Operators.MathSymbolMinus}, "*", "5", "*"
            };

            //Act
            var stringList = _binaryTreeService.ConvertToPostfix(exp);

            bool equal = stringList.SequenceEqual(mockList);
            //Assert
            Assert.IsTrue(equal);
        }

        [TestMethod]
        public void ConvertPostfix_NegativeDigits_Success()
        {
            //Arrange
            string exp = $"2*({Operators.Negative}4+3{Operators.MathSymbolMinus}(6+2))*5";
            string negVal = Operators.Negative + "4";
            List<string> mockList = new List<string>()
            {
                "2", negVal, "3", "+", "6", "2", "+", {Operators.MathSymbolMinus}, "*", "5", "*"
            };

            //Act
            var stringList = _binaryTreeService.ConvertToPostfix(exp);

            bool equal = stringList.SequenceEqual(mockList);
            //Assert
            Assert.IsTrue(equal);
        }

        [TestMethod]
        public void ConvertPostfix_MultipleDigitAndNegativeDigit_Success()
        {
            //Arrange
            string exp = $"((15 ÷ (7 − (1 + 1) ) ) × -3 ) − (2 + (1 + 1))";

            // Expected 15 7 1 1 + - / -3 x 2 1 1 + + -
            List<string> mockList = new List<string>()
            {
                "15", "7", "1", "1", "+",
                "−", "÷", "-3", "×", "2",
                "1", "1", "+", "+", "−"
             };

            //Act
            var stringList = _binaryTreeService.ConvertToPostfix(exp);

            bool equal = stringList.SequenceEqual(mockList);
            //Assert
            Assert.IsTrue(equal);
        }
    }
}