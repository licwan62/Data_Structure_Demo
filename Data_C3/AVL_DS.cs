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
    #endregion
    #region Search
    private Node? SearchNode(Node? current, Node node)
    {
        if (current != null)
        {// have not reached a leaf 
            if (current.Data.CompareTo(node.Data) == 0)
            {// base case, find the node
                return current;
            }
            else if (current.Data.CompareTo(node.Data) > 0)
            {
                return SearchNode(current.Left, node);
            }
            else
            {
                return SearchNode(current.Right, node);
            }
        }// reached leaf but unable to found the node
        return current;
    }
    public string Search(string data)
    {
        Node? node = new Node(data, data.Length);
        if (Root == null)
        {
            return "Empty Tree";
        }
        else
        {
            node = SearchNode(Root, node);
            if (node == null)
            {
                return $"Non-Existed node \"{data}\"";
            }
            else
            {
                return $"Node {node} is Found, " +
                    $"Height: {GetHeight(node)}, " +
                    $"Depth: {GetDepth(Root, node, 0)}";
            }
        }
    }
    #endregion
    #region Delete
    private string MinValue(Node node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node.Data;
    }
    private Node DeleteNode(Node? tree, Node node)
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
        node = DeleteNode(Root, node);
        if (node != null)
        {
            Root = DeleteNode(Root, node);
            return $"Node \"{node}\" is Deleted at Height {GetHeight(node)}";
        }
        else
        {
            return $"Non-Existing Node \"{node}\"";
        }
    }
    #endregion
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
    #endregion
    #region Traverse and Print
    private string InOrderTraverse(Node current)
    {
        StringBuilder sb = new StringBuilder();
        if (current != null)
        {
            sb.Append(InOrderTraverse(current.Left));
            sb.AppendLine(current.ToPrint() + " Height: " + GetHeight(current) + "\tDepth: " + GetDepth(Root, current, 0));
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
            sb.AppendLine(current.ToPrint() + " Height: " + GetHeight(current) + "\tDepth: " + GetDepth(Root, current, 0));
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
            sb.AppendLine(current.ToPrint() + " Height: " + GetHeight(current) + "\tDepth: " + GetDepth(Root, current, 0));
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
    #endregion
    private int GetHeight(Node node)
    {
        if (node != null)
        {
            int left = GetHeight(node.Left);
            int right = GetHeight(node.Right);
            int max = left > right ? left : right;
            return max + 1;
        }
        return -1;
    }
    private int GetDepth(Node current, Node target, int depth)
    {
        if (current != null)
        {
            if (current == target)
            {
                return depth;
            }
            else if (current.Data.CompareTo(target.Data) > 0)
            {
                return GetDepth(current.Left, target, depth + 1);
            }
            else
            {
                return GetDepth(current.Right, target, depth + 1);
            }
        }
        return -1;
    }
}