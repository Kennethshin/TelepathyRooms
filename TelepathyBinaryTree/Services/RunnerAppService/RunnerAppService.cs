using Newtonsoft.Json;

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
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            bool IsExit = false;
            string Input;

            while (!IsExit)
            {
                Console.WriteLine("==================================");
                Console.WriteLine("Please enter your infix expression");
                Input = Console.ReadLine();

                if (String.IsNullOrEmpty(Input))
                {
                    IsExit = true;
                    break;
                }

                var postfixList = GetConvertPostfix(Input);
                DrawTree(postfixList);
                int result = _binaryTreeService.CalculateBinaryTree(postfixList);

                Console.WriteLine($"Your value {result}");
            }
        }

        private List<string> GetConvertPostfix(string input)
        {
            var postFixList = _binaryTreeService.ConvertToPostfix(input);
            string postFix = "";

            foreach (string val in postFixList)
            {
                postFix += val + " ";
            }
            Console.WriteLine($"Your Postfix expression {postFix}");

            return postFixList;
        }

        private void DrawTree(List<string> postfixList)
        {
            Console.WriteLine("***Binary Tree Structure***");
            var tree = _binaryTreeService.BuildTree(postfixList);
            var json = JsonConvert.SerializeObject(tree, Formatting.Indented);
            Console.WriteLine(json);
        }

    }
}
