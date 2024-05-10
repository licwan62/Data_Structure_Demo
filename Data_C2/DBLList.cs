using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Schema;

namespace Data_C2;

internal class DBLList
{
    public Node Head { get; set; }
    public Node Tail { get; set; }
    public Node Current { get; set; }
    public int Counter { get; set; }
    public FileInfo File { get; set; }
    public DBLList()
    {
        Head = null;
        Tail = null;
        Current = null;
        File = null;
        Counter = 0;
    }
    public DBLList(FileInfo fileInfo)
    {
        Head = null;
        Tail = null;
        Current = null;
        File = fileInfo;
        Counter = 0;
    }
    #region Insert
    private bool InsertAtEnd(Node node)
    {
        bool inserted = false;
        if (node.Data.StartsWith("#"))
        {
            return inserted;
        }
        else if (Head == null)
        {
            Head = node;
            Tail = node;
            Current = node;
            inserted = true;
        }
        else
        {
            Current = Head;
            while (Current != null && !inserted)
            {
                if (Current.Data == node.Data)
                {
                    return inserted;
                }
                if (Current.Data == Tail.Data)
                {
                    node.Prev = Tail;
                    Tail.Next = node;
                    Tail = node;
                    Counter++;
                    inserted = true;
                }
                else
                {
                    Current = Current.Next;
                }
            }
        }
        return inserted;
    }
    public string AddAtEnd(string data)
    {
        Node node = new Node(data);
        if (InsertAtEnd(node))
        {
            return $"Node {node} Inserted at End";
        }
        else
        {
            return $"Node {node} Exists or Started with #";
        }
    }
    private bool InsertAtFront(Node node)
    {
        bool inserted = false;
        if (node.Data.StartsWith("#"))
        {
            return inserted;
        }
        else if (Head == null)
        {
            Head = node;
            Tail = node;
            Counter++;
            inserted = true;
        }
        else
        {
            if (FindNode(node) > 0)
            {
                return inserted;
            }
            node.Next = Head;
            Head.Prev = node;
            Head = node;
            Counter++;
            inserted = true;
        }
        return inserted;
    }
    public string AddAtFront(string data)
    {
        Node node = new Node(data);
        if (InsertAtFront(node))
        {
            return $"Node {node} Added at Front";
        }
        else
        {
            return $"Node {node} Exists or Start with #";
        }
    }
    private int InsertBefore(Node node, Node target)
    {
        int inserted = 0;
        if (node.Data.StartsWith("#"))
        {
            inserted = -1;
        }
        else if (Head == null)
        {
            inserted = -2;
        }
        else if (FindNode(node) > 0)
        {
            inserted = -3;
        }
        else
        {
            Current = Head;
            while (Current != null && inserted == 0)
            {
                if (Current.Data == target.Data)
                {
                    node.Prev = Current.Prev;
                    node.Next = Current;
                    Current.Prev.Next = node;
                    Current.Prev = node;
                    Counter++;
                    inserted = 1;
                }
                else
                {
                    Current = Current.Next;
                }
            }
        }
        return inserted;
    }
    public string AddBefore(string data, string target)
    {
        Node node = new Node(data);
        Node targetNode = new Node(target);
        switch (InsertBefore(node, targetNode))
        {
            case 1: return $"New node {node} Added Before {target}";
            case 0: return $"Target {targetNode} not Existed";
            case -1: return $"Word begin with \"#\" Not Allowed in List";
            case -2: return $"Empty List";
            case -3: return $"New node {node} already Existed";
            default: return "";
        }
    }
    private int InsertAfter(Node node, Node target)
    {
        int inserted = 0;
        if (node.Data.StartsWith("#"))
        {
            inserted = -1;
        }
        else if (Head == null)
        {
            inserted = -2;
        }
        else if (FindNode(node) > 0)
        {
            inserted = -3;
        }
        else if (Tail.Data == target.Data)
        {
            InsertAtEnd(node);
            inserted = 1;
        }
        else
        {
            Current = Head;
            while (Current != null && inserted == 0)
            {
                if (Current.Data == target.Data)
                {
                    node.Prev = Current;
                    node.Next = Current.Next;
                    Current.Next.Prev = node;
                    Current.Next = node;
                    Counter++;
                    inserted = 1;
                }
                else
                {
                    Current = Current.Next;
                }
            }
        }
        return inserted;
    }
    public string AddAfter(string data, string target)
    {
        Node node = new Node(data);
        Node targetNode = new Node(target);
        switch (InsertAfter(node, targetNode))
        {
            case 1: return $"New node {node} Added After {targetNode}";
            case 0: return $"Target {targetNode} Not Existed";
            case -1: return $"Word begin with \"#\" Not Allowed in List";
            case -2: return "Empty List";
            case -3: return $"New node {node} already Existed";
            default: return "";
        }
    }
    #endregion Insert

    #region Delete
    private bool DeleteAtFront()
    {
        if (Head == null) return false;
        else
        {
            Head = Head.Next;
            Head.Prev = null;
            Counter--;
            Current = Head;
            return true;
        }
    }
    private bool DeleteAtTail()
    {
        if (Head == null) return false;
        else
        {
            Tail = Tail.Prev;
            Tail.Next = null;
            Current = Tail;
            Counter--;
            return true;
        }
    }
    private bool DeleteNode(Node node)
    {
        bool deleted = false;
        if (Head == null)
        {
            Current = Head;
            return deleted;
        }
        else if (Head.Data == node.Data)
        {
            deleted = DeleteAtFront();
        }
        else if (Tail.Data == node.Data)
        {
            deleted = DeleteAtTail();
        }
        else
        {
            Current = Head;
            while (Current != null && !deleted)
            {
                if (Current.Data == node.Data)
                {
                    node.Prev = Current;
                    node.Next = Current.Next;
                    Current.Next.Prev = node;
                    Current.Next = node;
                    Counter--;
                    deleted = true;
                }
                else
                {
                    Current = Current.Next;
                }
            }
        }
        return deleted;
    }
    public string Remove(string data)
    {
        Node node = new Node(data);
        if (DeleteNode(node))
        {
            return $"Node {node.Data} Deleted";
        }
        else
        {
            return $"Empty List or Non-exist Node {node.Data}";
        }
    }
    #endregion Delete

    #region Find
    private int FindNode(Node node)
    {
        int pos = 0;
        bool found = false;
        if (Head == null)
        {
            return pos;
        }
        else
        {
            Current = Head;
            pos = 1;
            while (Current != null && !found)
            {
                if (Current.Data == node.Data)
                {
                    found = true;
                }
                else
                {
                    Current = Current.Next;
                    pos++;
                }
            }
            if (!found)
            {
                pos = -1;
            }
        }
        return pos;
    }
    public string Find(string data)
    {
        Node node = new Node(data);
        int pos = FindNode(node);
        switch (pos)
        {
            case 0: return "Empty List";
            case -1: return $"Node {node} Not Existed";
            default: return $"Node {node} Found at [{pos}]";
        }
    }
    #endregion Find
    public string ToPrint()
    {
        StringBuilder sb = new StringBuilder();
        Current = Head;
        if (Head == null)
        {
            sb.AppendLine("Empty List");
        }
        else
        {
            sb.AppendLine($"== Items In Doubly Linked List [{File.Name}] ==");
            while (Current != null)
            {
                sb.AppendLine(Current.ToPrint());
                Current = Current.Next;
            }
            sb.AppendLine($"== Print over, total count: {Counter} ==");
        }
        return sb.ToString();
    }
}
