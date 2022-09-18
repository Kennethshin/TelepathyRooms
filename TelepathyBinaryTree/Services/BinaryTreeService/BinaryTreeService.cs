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
            if(postFix == null || postFix.Count == 0) return 0;

            var tree = BuildTree(postFix);
            var total = EvaluateBinaryTree(tree);
            return total;
        }

        public List<string> ConvertToPostfix(string infix)
        {
            if (String.IsNullOrEmpty(infix)) return null;

            List<string> postFixList = new List<string>();

            infix = Regex.Replace(infix, @"\s+", ""); // remove empty spaces
            infix = Regex.Replace(infix, "[xX]", Operators.MathSymbolMultiply); // replace character x X to Multiplier x symbol
            Stack<string> operatorStack = new Stack<string>();

            for (int i = 0; i < infix.Length; i++)
            {
                string character = infix[i].ToString();
                if (IsOperation(character))
                {
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
                            postFixList.Add(operatorStack.Pop());
                        }
                        if (operatorStack.Count > 0 && operatorStack.Peek() == Operators.OpenBracket)
                        {
                            operatorStack.Pop();
                        }
                    }
                    else
                    {
                        if (i != 0 && IsDigit(infix[i - 1]))
                        {
                            postFixList[postFixList.Count - 1] = postFixList[postFixList.Count - 1] + character;
                        }
                        else
                        {
                            postFixList.Add(character);
                        }
                    }
                }
            }

            while (operatorStack.Count > 0)
            {
                postFixList.Add(operatorStack.Pop());
            }
            return postFixList;
        }

        public Node BuildTree(List<string> postFix)
        {
            if (postFix == null || postFix.Count == 0) return null;

            Stack<Node> stackNode = new Stack<Node>();
            try
            {
                for (int i = 0; i < postFix.Count; i++)
                {
                    var dataValue = postFix[i].ToString();

                    Node node = new Node(dataValue);
                    if (!IsOperation(dataValue))
                    {
                        //if negative is detected, then i will create right node '-' and right node Number
                        if (dataValue.Length > 1 && dataValue[0].ToString() == Operators.Negative)
                        {
                            var tempNode = new Node(dataValue[0].ToString());
                            node.data = dataValue.Remove(0, 1);
                            stackNode.Push(node);
                            stackNode.Push(tempNode); // Pushing the negative value
                        }
                        else
                        {
                            stackNode.Push(node);
                        }
                    }
                    else
                    {
                        node.rightHand = stackNode.Pop();
                        if (node.rightHand.data == Operators.Negative)
                        {
                            node.rightHand.rightHand = stackNode.Pop();
                        }

                        node.leftHand = stackNode.Pop();
                        if (node.leftHand.data == Operators.Negative)
                        {
                            node.leftHand.rightHand = stackNode.Pop();
                        }

                        stackNode.Push(node);
                    }
                }

                return stackNode.Pop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            
        }

        private int EvaluateBinaryTree(Node node)
        {
            try
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
                if(node.data == Operators.Negative) // If negative, i will return negative value. ASSUMING THAT negative only follows by number
                {
                    rightEval = EvaluateBinaryTree(node.rightHand);

                    return -rightEval;
                }
                else
                {
                    return Convert.ToInt32(node.data);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
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
