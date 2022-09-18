using TelepathyBinaryTree.ViewModel;

namespace TelepathyBinaryTree.Services.BinaryTreeService
{
    public class RunnnerAppService : IRunnnerAppService
    {
        IBinaryTreeService _binaryTreeService;
        public RunnnerAppService(IBinaryTreeService binaryTreeService)
        {
            _binaryTreeService = binaryTreeService;
        }

        public void Run()
        {
            //string s = "(1+1)-4*(2+4/2)";
            //string t = "((5/(7-(1+1)))*3)-(2+(1+1))";
            //string s = "((15÷(7−(1+1)))×3)−(2+(1+1))";
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine("Please enter your infix expression");
            string inputExpression = Console.ReadLine();
            var postFixList = _binaryTreeService.ConvertToPostfix(inputExpression);
            string postFix = "";

            foreach(string val in postFixList)
            {
                postFix += val + " ";
            }
            Console.WriteLine($"Your Postfix expression {postFix}");
            int result = _binaryTreeService.CalculateBinaryTree(postFixList);
            //Node root = _binaryTreeService.BuildTree(postFix);
            //int result = _binaryTreeService.EvaluateBinaryTree(root);
            Console.WriteLine($"Your value {result}");

            Console.ReadLine();
        }

    }
}
