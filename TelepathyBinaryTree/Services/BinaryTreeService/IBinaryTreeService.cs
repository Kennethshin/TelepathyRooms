namespace TelepathyBinaryTree.Services.BinaryTreeService
{
    public interface IBinaryTreeService
    {
        List<string> ConvertToPostfix(string infix);

        int CalculateBinaryTree(List<string> postFix);
    }
}