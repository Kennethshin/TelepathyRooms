using System;
using System.Collections.Generic;
class Program
{

    // Tree Structure
    public class Node
    {
        public string data;
        public Node leftHand;
        public Node rightHand;

        public Node(string data)
        {
            this.data = data;
            leftHand = null;
            rightHand = null;
        }
    }

    public static bool isOperation(string c)
    {
        return c == Operators.Plus 
            || c == Operators.Minus 
            || c == Operators.Divide 
            || c == Operators.Multiply 
            || c == Operators.OpenBracket 
            || c == Operators.MathSymbolMinus
            || c == Operators.MathSymbolMultiply
            || c == Operators.Negative
            || c == Operators.MathSymbolDivide;
    }

    public static int GetPriority(string c)
    {
        if (c == Operators.Plus || c == Operators.Minus)
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

    public static Node BuildTree(string postFix)
    {
        Stack<Node> stackNode = new Stack<Node>();
        
        for(int i = 0; i < postFix.Length; i++)
        {
            var dataValue = postFix[i].ToString();

            Node node =  new Node(dataValue);
            if (!isOperation(dataValue))
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

    public static int evaluate(Node node)
    {
        if (node == null)
        {
            return 0;
        }
        int leftEval;
        int rightEval;

        if (isOperation(node.data))
        {
            leftEval = evaluate(node.leftHand);
            rightEval = evaluate(node.rightHand);

            if (node.data.Equals(Operators.Plus))
                return leftEval + rightEval;

            if (node.data.Equals(Operators.Minus))
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


    public static string convert(string infix)
    {

        string postfix = "";
        bool isNegative = false;
        Stack<string> s1 = new Stack<string>();
        for (int i = 0; i < infix.Length; i++)
        {
            string character = infix[i].ToString();
            if (isOperation(character))
            {
                //if(character == Operators.Negative)
                //{
                //    isNegative = true;
                //    continue;
                //}

                if (character == Operators.OpenBracket)
                {
                    s1.Push(character);
                    continue;
                }

                int priority = GetPriority(character);
                if (priority > 0)
                {
                    while(s1.Count > 0 && s1.Peek() != Operators.OpenBracket && GetPriority(s1.Peek()) >= priority)
                    {
                        postfix += s1.Pop();
                    }
                    s1.Push(character);
                }
            }
            else
            {
                if (character == Operators.CloseBracket)
                {
                    while (s1.Count > 0 && s1.Peek() != Operators.OpenBracket)
                    {
                        postfix += s1.Pop();
                    }
                    if (s1.Count > 0 && s1.Peek() == Operators.OpenBracket)
                    {
                        s1.Pop();
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
                        postfix += character;
                    //}
                }
            }
        }

        while(s1.Count > 0)
        {
            postfix += s1.Pop();
        }
        return postfix;
    }

    // Driver code
    public static void Main(String[] args)
    {
        string s = "(1+1)-4*(2+4/2)";
        string t = "((5/(7-(1+1)))*3)-(2+(1+1))";
        //string s = "((15÷(7−(1+1)))×3)−(2+(1+1))";
        string postFix = convert(s);
        Console.WriteLine(postFix);
        Node root = BuildTree(postFix);
        int result = evaluate(root);
        Console.WriteLine(result);

        Console.ReadLine();
        
    }
}