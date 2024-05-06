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
    public DBLList(FileInfo fileInfo)
    {
        Head = null;
        Tail = null;
        Current = null;
        Counter = 0;
        File = fileInfo;
    }
    private bool InsertNode(Node node)
    {
        bool inserted = false;
        if (Head == null)
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
                if (Current.Data == node.Data || node.Data.StartsWith("#"))
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
    public string Insert(string data)
    {
        Node node = new Node(data);
        if (InsertNode(node))
        {
            return $"Node {node.Data} Inserted as new Tail";
        }
        else
        {
            return $"Node {node.Data} is Existed or Started with #";
        }
    }
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
            Counter--;
            Current = Tail;
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
            DeleteAtFront();
        }
        else if (Tail.Data == node.Data)
        {
            DeleteAtTail();
        }
        else
        {
            Current = Head;
            while (Current != null && !deleted)
            {
                if (Current.Data == node.Data)
                {
                    if (Current == Tail)
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
    public string Delete(string data)
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
                pos = 0;
            }
        }
        return pos;
    }
    public string Find(string data)
    {
        Node node = new Node(data);
        int pos = FindNode(node);
        if (pos == 0)
        {
            return $"Empty List or Non-exist Node {node.Data}";
        }
        else
        {
            return $"Node {node.Data} Found on position {pos}";
        }
    }
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
                sb.AppendLine(Current.ToString());
                Current = Current.Next;
            }
            sb.AppendLine($"== Print over, total count: {Counter} ==");
        }
        return sb.ToString();
    }
}
