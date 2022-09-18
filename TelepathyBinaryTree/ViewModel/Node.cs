namespace TelepathyBinaryTree.ViewModel
{
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
}
