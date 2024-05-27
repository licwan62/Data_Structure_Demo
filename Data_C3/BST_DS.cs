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
    #region Insert and Search
    /// <summary>
    /// Insert under the correct leaf
    /// </summary>
    /// <param name="node">node to insert</param>
    /// <param name="current">current node</param>
    /// <returns>current node</returns>
    /// <returns></returns>
    private Node InsertNode(Node current, Node node)
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
        // base case: node.key == current.key
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
        {// have not reached a leaf 
            if (current.Key == node.Key)
            {// base case, find the node
                return current;
            }
            else if (node.Key < current.Key)
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
    public string Find(string data)
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
    #endregion Insert and Search
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
            sb.AppendLine($"*** Print BSTree: {Name} ***");
            sb.AppendLine(InOrderTraverse(Root));
            sb.AppendLine($"*** Printed BSTree: {Name}, Words: {Count}");
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
            sb.AppendLine($"*** Print BSTree: {Name} ***");
            sb.AppendLine(PreOrderTraverse(Root));
            sb.AppendLine($"*** Printed BSTree: {Name}, Words: {Count}");
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
            sb.AppendLine($"*** Print BSTree: {Name} ***");
            sb.AppendLine(PostOrderTraverse(Root));
            sb.AppendLine($"*** Printed BSTree: {Name}, Words: {Count}");
        }
        return sb.ToString();
    }
    #endregion Traverse and Print
}
