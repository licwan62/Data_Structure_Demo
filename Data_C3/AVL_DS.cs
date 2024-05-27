using System.Text;
using System.Xml.Linq;

namespace Data_C3;

internal class AVL_DS
{
    public Node? Root { get; set; }
    public int Count { get; set; }
    public string Name { get; set; }
    public AVL_DS()
    {
        Root = null;
        Count = 0;
        Name = "";
    }
    public AVL_DS(string name)
    {
        Name = name;
        Root = null;
        Count = 0;
    }
    #region Insert
    /// <summary>
    /// Traverse tree, return at null child
    /// then add node at leaf's child
    /// </summary>
    /// <param name="current">current root</param>
    /// <param name="node">node to insert</param>
    /// <returns></returns>
    private Node? InsertNode(Node current, Node node)
    {
        if (node.Key < current.Key)
        {
            if (current.Left == null)
            {
                current.Left = node;
                Count++;
            }
            else
            {
                InsertNode(current.Left, node);
            }
        }
        else if (node.Key > current.Key)
        {
            if (current.Right == null)
            {
                current.Right = node;
                Count++;
            }
            else
            {
                InsertNode(current.Right, node);
            }
        }
        return current;
    }
    public string Add(string data)
    {
        Node node = new Node(data, data.Length);
        if (node.Data.StartsWith("#"))
        {
            return $"Invalid node {node} Begins with \"#\", Not Added!";
        }
        else if (Root == null)
        {
            Root = node;
            Count++;
            return $"New node {node} Added";
        }
        else if (SearchNode(Root, node) != null)
        {
            return $"Duplicated node {node}, Not Added!";
        }
        else
        {
            Root = InsertNode(Root, node);
            return $"New node {node} Added";
        }
    }
    private Node? SearchNode(Node? current, Node node)
    {
        if (current != null)
        {
            if (current.Key == node.Key)
            {
                return current;
            }
            else if (current.Key < node.Key)
            {
                return SearchNode(current.Left, node);
            }
            else
            {
                return SearchNode(current.Right, node);
            }
        }
        return null;
    }
    public string Find(string data)
    {
        Node? node = new Node(data, data.Length);
        node = SearchNode(Root, node);
        if (node == null)
        {
            return $"Non-Exist node {node}";
        }
        else
        {
            int depth = GetHeight(node);
            return $"Node {node} is Found, Depth: {depth}";
        }
    }
    private Node BalanceTree(Node tree)
    {
        int factor = BalanceFactor(tree);
        if (factor > 1)
        {// left heavy
            if (BalanceFactor(tree.Left) > 0)
            {
                tree = RotateLL(tree);
            }
            else
            {
                tree = RotateLR(tree);
            }
        }
        else if (factor < -1)
        {// right heavy
            if (BalanceFactor(tree.Right) > 0)
            {
                tree = RotateRL(tree);
            }
            else
            {
                tree = RotateRR(tree);
            }
        }
        return tree;
    }
    private Node RotateLL(Node parent)
    {
        Node pivot = parent.Left;
        parent.Left = pivot.Right;
        pivot.Right = parent;
        return pivot;
    }
    private Node RotateRR(Node parent)
    {
        Node pivot = parent.Right;
        parent.Right = pivot.Left;
        pivot.Left = parent;
        return pivot;
    }
    private Node RotateLR(Node parent)
    {
        Node pivot = parent.Left;
        pivot = RotateRR(pivot);
        return RotateLL(parent);
    }
    private Node RotateRL(Node parent)
    {
        Node pivot = parent.Right;
        pivot = RotateLL(pivot);
        return RotateRR(parent);
    }
    private int BalanceFactor(Node current)
    {
        return GetHeight(current.Left) - GetHeight(current.Right);
    }
    private int GetHeight(Node current)
    {
        if (current != null)
        {
            int left = GetHeight(current.Left);
            int right = GetHeight(current.Right);
            int max = left > right ? left : right;
            return max + 1;
        }
        return 0;
    }
    #endregion Insert
    #region Traverse and Print
    private string InOrderTraverse(Node current)
    {
        StringBuilder sb = new StringBuilder();
        if (current != null)
        {
            sb.Append(InOrderTraverse(current.Left));
            sb.AppendLine(current.ToPrint());
            sb.Append(InOrderTraverse(current.Right));
        }
        return sb.ToString();
    }
    public string InOrderPrint()
    {
        StringBuilder sb = new StringBuilder();
        if (Root == null)
        {
            sb.AppendLine("Empty Tree");
        }
        else
        {
            sb.AppendLine($"*** Print AVL Tree: {Name} ***");
            sb.AppendLine(InOrderTraverse(Root));
            sb.AppendLine($"*** Printed AVL Tree: {Name}, Words: {Count}");
        }
        return sb.ToString();
    }
    private string PreOrderTraverse(Node current)
    {
        StringBuilder sb = new StringBuilder();
        if (current != null)
        {
            sb.AppendLine(current.ToPrint() + "\t Depth: " + GetHeight(current));
            sb.Append(PreOrderTraverse(current.Left));
            sb.Append(PreOrderTraverse(current.Right));
        }
        return sb.ToString();
    }
    public string PreOrderPrint()
    {
        StringBuilder sb = new StringBuilder();
        if (Root == null)
        {
            sb.AppendLine("Empty Tree");
        }
        else
        {
            sb.AppendLine($"*** Print AVL Tree: {Name} ***");
            sb.AppendLine(PreOrderTraverse(Root));
            sb.AppendLine($"*** Printed AVl Tree: {Name}, Words: {Count}");
        }
        return sb.ToString();
    }
    private string PostOrderTraverse(Node current)
    {
        StringBuilder sb = new StringBuilder();
        if (current != null)
        {
            sb.Append(PostOrderTraverse(current.Left));
            sb.Append(PostOrderTraverse(current.Right));
            sb.AppendLine(current.ToPrint());
        }
        return sb.ToString();
    }
    public string PostOrderPrint()
    {
        StringBuilder sb = new StringBuilder();
        if (Root == null)
        {
            sb.AppendLine("Empty Tree");
        }
        else
        {
            sb.AppendLine($"*** Print AVL Tree: {Name} ***");
            sb.AppendLine(PostOrderTraverse(Root));
            sb.AppendLine($"*** Printed AVL Tree: {Name}, Words: {Count}");
        }
        return sb.ToString();
    }
    #endregion Traverse and Print
}
