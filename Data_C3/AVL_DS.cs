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
        Name = "";
        Root = null;
        Count = 0;
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
    private Node InsertNode(Node current, Node node)
    {
        if (current == null)
        {
            Count++;
            return node;
        }
        else if ((current.Data.CompareTo(node.Data)) > 0)
        {
            current.Left = InsertNode(current.Left, node);
        }
        else
        {
            current.Right = InsertNode(current.Right, node);
        }
        current = BalanceTree(current);
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
    #endregion Insert

    #region Search
    private Node? SearchNode(Node? current, Node node)
    {
        if (current != null)
        {
            if ((current.Data.CompareTo(node.Data)) == 0)
            {
                return current;
            }
            else if ((current.Data.CompareTo(node.Data)) > 0)
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
    public string Search(string data)
    {
        Node? node = new Node(data, data.Length);
        node = SearchNode(Root, node);
        if (node == null)
        {
            return $"Non-Exist node \"{node}\"";
        }
        else
        {
            int depth = GetHeight(node);
            return $"Node \"{node}\" is Found, Depth: {depth}";
        }
    }
    #endregion Search

    #region Delete
    private string MinValue(Node node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node.Data;
    }
    private Node DeleteNode(Node tree, Node node)
    {
        if (tree == null)
        {
            return null;
        }
        else if (tree.Data.CompareTo(node.Data) > 0)
        {
            tree.Left = DeleteNode(tree.Left, node);
        }
        else if (tree.Data.CompareTo(node.Data) < 0)
        {
            tree.Right = DeleteNode(tree.Right, node);
        }
        else
        {// node found
            if (tree.Left == null)
            {
                tree = tree.Right;
            }
            else if (tree.Right == null)
            {
                tree = tree.Left;
            }
            else
            {// node has 2 children 
                tree.Data = MinValue(tree.Right);
                tree.Right = DeleteNode(tree.Right, tree);
            }
        }
        if (tree != null)
        {
            tree = BalanceTree(tree);
        }
        return tree;
    }
    public string Delete(string data)
    {
        Node? node = new Node(data, data.Length);
        node = SearchNode(Root, node);
        if (node != null)
        {
            Root = DeleteNode(Root, node);
            return $"Node \"{data}\" is Deleted";
        }
        else
        {
            return $"Non-Existing Node {data}";
        }
    }
    #endregion Delete

    #region Balance
    private Node BalanceTree(Node current)
    {
        int factor = BalanceFactor(current);
        if (factor > 1)
        {// left heavy
            if (BalanceFactor(current.Left) > 0)
            {// left sub-tree is left heavy
                current = RotateLL(current);
            }
            else
            {
                current = RotateLR(current);
            }
        }
        else if (factor < -1)
        {// right heavy
            if (BalanceFactor(current.Right) > 0)
            {
                current = RotateRL(current);
            }
            else
            {
                current = RotateRR(current);
            }
        }
        return current;
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
        parent.Left = RotateRR(pivot);
        return RotateLL(parent);
    }
    private Node RotateRL(Node parent)
    {
        Node pivot = parent.Right;
        parent.Right = RotateLL(pivot);
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
    #endregion Balance

    #region Traverse and Print
    private string InOrderTraverse(Node current)
    {
        StringBuilder sb = new StringBuilder();
        if (current != null)
        {
            sb.Append(InOrderTraverse(current.Left));
            sb.AppendLine(current.ToPrint() + " Depth: " + GetHeight(current));
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
            sb.AppendLine($"*** InOrder AVL Tree: {Name} ***");
            sb.AppendLine(InOrderTraverse(Root));
            sb.AppendLine($"*** InOrder AVL Tree: {Name}, Words: {Count}");
        }
        return sb.ToString();
    }
    private string PreOrderTraverse(Node current)
    {
        StringBuilder sb = new StringBuilder();
        if (current != null)
        {
            sb.AppendLine(current.ToPrint() + " Depth: " + GetHeight(current));
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
            sb.AppendLine($"*** PreOrder AVL Tree: {Name} ***");
            sb.AppendLine(PreOrderTraverse(Root));
            sb.AppendLine($"*** PreOrder AVl Tree: {Name}, Words: {Count}");
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
            sb.AppendLine(current.ToPrint() + " Depth: " + GetHeight(current));
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
            sb.AppendLine($"*** PostOrder AVL Tree: {Name} ***");
            sb.AppendLine(PostOrderTraverse(Root));
            sb.AppendLine($"*** PostOrder AVL Tree: {Name}, Words: {Count}");
        }
        return sb.ToString();
    }
    #endregion Traverse and Print
}
