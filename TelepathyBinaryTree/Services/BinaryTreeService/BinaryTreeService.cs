using System.Globalization;
using System.Text.RegularExpressions;
using TelepathyBinaryTree.ViewModel;

namespace TelepathyBinaryTree.Services.BinaryTreeService
{
    public class BinaryTreeService : IBinaryTreeService
    {

        public BinaryTreeService()
        {
        }

        public int CalculateBinaryTree(List<string> postFix)
        {
            var tree = BuildTree(postFix);
            var total = EvaluateBinaryTree(tree);
            return total;
        }

        public List<string> ConvertToPostfix(string infix)
        {
            //Assuming that the infix has a proper negative symbol to represent negative

            string postfix = "";
            List<string> postFixList = new List<string>();
            bool isNegative = false;

            infix = Regex.Replace(infix, @"\s+", "");
            Stack<string> operatorStack = new Stack<string>();

            //Make sure to convert all negative number with a proper negative symbol
            //for (int x = 0; x < infix.Length; x++)
            //{
            //    string text = infix[x].ToString();

            //    if (text != Operators.Minus)
            //    {
            //        continue;
            //    }
            //    if (x == 0 || infix[x - 1] == '(' || isOperation(infix[x - 1].ToString()))
            //    {
            //        infix[x] = '~';
            //        infix[x] = 
            //    }
            //}

            for (int i = 0; i < infix.Length; i++)
            {
                string character = infix[i].ToString();
                if (IsOperation(character))
                {
                    //if(character == Operators.Negative)
                    //{
                    //    isNegative = true;
                    //    continue;
                    //}
                    // Handle Multiple digit
                    if (character == Operators.OpenBracket)
                    {
                        operatorStack.Push(character);
                        continue;
                    }

                    int priority = GetPriority(character);
                    if (priority > 0)
                    {
                        while (operatorStack.Count > 0 && operatorStack.Peek() != Operators.OpenBracket && GetPriority(operatorStack.Peek()) >= priority)
                        {
                            //postfix += operatorStack.Pop() + " ";
                            postFixList.Add(operatorStack.Pop());
                        }
                        operatorStack.Push(character);
                    }
                }
                else
                {
                    if (character == Operators.CloseBracket)
                    {
                        while (operatorStack.Count > 0 && operatorStack.Peek() != Operators.OpenBracket)
                        {
                            //postfix += operatorStack.Pop() + " ";
                            postFixList.Add(operatorStack.Pop());
                        }
                        if (operatorStack.Count > 0 && operatorStack.Peek() == Operators.OpenBracket)
                        {
                            operatorStack.Pop();
                        }
                    }
                    else
                    {
                        //if(isNegative)
                        //{
                        //    postfix = $"{Operators.Negative} + {character} ";
                        //    isNegative = false;
                        //}
                        //else
                        //{
                        //postfix += character + " ";
                        if (i != 0 && IsDigit(infix[i - 1]))
                        {
                            postFixList[postFixList.Count - 1] = postFixList[postFixList.Count - 1] + character;
                        }
                        else
                        {
                            postFixList.Add(character);
                        }
                        //}
                    }
                }
            }

            while (operatorStack.Count > 0)
            {
                //if (operatorStack.Count != 1)
                //{
                //    postfix += operatorStack.Pop() + " ";
                //}
                //else
                //    postfix += operatorStack.Pop();
                postFixList.Add(operatorStack.Pop());
            }
            return postFixList;
            //return postFixList;
        }

        private Node BuildTree(List<string> postFix)
        {
            Stack<Node> stackNode = new Stack<Node>();

            for (int i = 0; i < postFix.Count; i++)
            {
                var dataValue = postFix[i].ToString();

                Node node = new Node(dataValue);
                if (!IsOperation(dataValue))
                {
                    stackNode.Push(node);
                }
                else
                {
                    node.rightHand = stackNode.Pop();
                    node.leftHand = stackNode.Pop();

                    stackNode.Push(node);
                }
            }

            return stackNode.Pop();
        }
        private int EvaluateBinaryTree(Node node)
        {
            if (node == null)
            {
                return 0;
            }
            int leftEval;
            int rightEval;

            if (IsOperation(node.data))
            {
                leftEval = EvaluateBinaryTree(node.leftHand);
                rightEval = EvaluateBinaryTree(node.rightHand);

                if (node.data.Equals(Operators.Plus))
                    return leftEval + rightEval;

                if (node.data.Equals(Operators.MathSymbolMinus))
                    return leftEval - rightEval;

                if (node.data.Equals(Operators.Multiply) || node.data.Equals(Operators.MathSymbolMultiply))
                    return leftEval * rightEval;

                return leftEval / rightEval;
            }
            else
            {
                return Convert.ToInt32(node.data);
            }
        }

        private bool IsOperation(string c)
        {
            return c == Operators.Plus
                || c == Operators.Divide
                || c == Operators.Multiply
                || c == Operators.OpenBracket
                || c == Operators.MathSymbolMinus
                || c == Operators.MathSymbolMultiply
                || c == Operators.MathSymbolDivide;
        }

        private int GetPriority(string c)
        {
            if (c == Operators.Plus || c == Operators.MathSymbolMinus)
            {
                return 1;
            }
            else if (c == Operators.Divide || c == Operators.Multiply || c == Operators.MathSymbolMultiply || c == Operators.MathSymbolDivide)
            {
                return 2;
            }
            else
                return 0;
        }

        private bool IsDigit(char c)
        {
            return (Char.IsDigit(c) || c.ToString() == Operators.Negative);
        }
    }
}
