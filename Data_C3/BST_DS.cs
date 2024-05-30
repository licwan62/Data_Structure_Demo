using System;
using System.Diagnostics;
using System.Text;

namespace Data_C3;

internal class BST_DS
{
    public Node? Root { get; set; }
    public int Count { get; set; }
    public string Name { get; set; }
    public BST_DS()
    {
        Name = "";
        Root = null;
        Count = 0;
    }
    public BST_DS(string name)
    {
        Name = name;
        Root = null;
        Count = 0;
    }
    #region Insert
    /// <summary>
    /// Insert under the correct leaf
    /// </summary>
    /// <param name="node">node to insert</param>
    /// <param name="current">current node</param>
    /// <returns>current node</returns>
    /// <returns></returns>
    private Node InsertNode(Node current, Node node)
    {
        if (current == null)
        {
            Count++;
            return node;
        }
        else if (current.Data.CompareTo(node.Data) > 0)
        {
            current.Left = InsertNode(current.Left, node);
        }
        else
        {
            current.Right = InsertNode(current.Right, node);
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
    private Node DeleteNode(Node tree, Node node)
    {
        if (tree == null)
        {
            return tree;
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
        return tree;
    }
    public string Delete(string data)
    {
        Node? node = new Node(data, data.Length);
        node = SearchNode(Root, node);
        if (node != null)
        {
            Root = DeleteNode(Root, node);
            return $"Node \"{node}\" is Deleted";
        }
        else
        {
            return $"Non-Existing Node \"{node}\"";
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
        return null;
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
                int depth = GetHeight(node);
                return $"Node {node} is Found, Depth: {depth}";
            }
        }
    }
    #endregion Search
    
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
            sb.AppendLine($"*** InOrder BSTree: {Name} ***");
            sb.AppendLine(InOrderTraverse(Root));
            sb.AppendLine($"*** InOrder BSTree: {Name}, Words: {Count}");
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
            sb.AppendLine($"*** PreOrder BSTree: {Name} ***");
            sb.AppendLine(PreOrderTraverse(Root));
            sb.AppendLine($"*** PreOrder BSTree: {Name}, Words: {Count}");
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
            sb.AppendLine($"*** PostOrder BSTree: {Name} ***");
            sb.AppendLine(PostOrderTraverse(Root));
            sb.AppendLine($"*** PostOrder BSTree: {Name}, Words: {Count}");
        }
        return sb.ToString();
    }
    #endregion
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
    
}
