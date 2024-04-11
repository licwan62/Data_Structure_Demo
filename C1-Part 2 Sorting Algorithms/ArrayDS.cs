using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;

namespace C1_Part_2_Sorting_Algorithms;

internal class ArrayDS
{
    Node[] nodes;
    public string Name { get; set; }
    public int Number { get; set; }
    public int ArraySize { get { return nodes.Length; } }
    public int Max_arraySize { get; set; }
    int arraySize = 0;
    public ArrayDS(int number, int max_arraySize)
    {
        this.Number = number;
        this.Max_arraySize = max_arraySize;
        this.Name = this.Number.ToString() + "_Words Array";
        nodes = new Node[Max_arraySize];
    }
    public ArrayDS(Node[] input, int number, string sort_type)
    {
        this.Number = number ;
        this.Name= sort_type + "_" + this.Number.ToString() + "_Words Array";
        nodes = input;
    }
    public string Print()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"*** Print of {this.Name} ***");
        sb.AppendLine($"{$"Index",-5} | {$"Word",-20} | {$"Length",-7}");
        sb.AppendLine("------+----------------------+--------");
        for (int index = 0; index < nodes.Length; index++)
        {
            sb.AppendLine($"{index,-5} | {nodes[index].Word,-20} | {nodes[index].Length,-7}");
        }
        sb.AppendLine("------+----------------------+--------");
        sb.AppendLine($"{$"Index",-5} | {$"Word",-20} | {$"Length",-7}");
        sb.AppendLine($"*** Print of {this.Name} ***");
        sb.AppendLine("--------------------------------------------");
        return sb.ToString();
    }
    public void Insert(string word)
    {
        Node node = new(word, word.Length);
        int index = 0;
        while (true)
        {
            if (nodes[index] == null)
            {
                nodes[index] = node;
                break;
            }
            index++;
        }
    }
    public string SelectionSort()
    {
        StringBuilder stringBuilder = new StringBuilder();
        Node[] _nodes = nodes;
        for (int i = 0; i < _nodes.Length; i++)
        {
            int min = i;
            for (int j = i + 1; j < _nodes.Length; j++)
            {
                if (_nodes[min].Length > _nodes[j].Length)
                {
                    min = j;
                }
            }
            Node min_node = _nodes[min];
            _nodes[min] = _nodes[i];
            _nodes[i] = min_node;
        }
        ArrayDS _arrayDS = new ArrayDS(_nodes, this.Number, "SelectionSorted");
        stringBuilder.AppendLine(_arrayDS.Print());
        Console.WriteLine($"Selection sorting {this.Name} complete");
        return stringBuilder.ToString();
    }
    private Node[] Merge(Node[] input, int left, int mid, int right)
    {
        int n1 = mid - left + 1;
        int n2 = right - mid;
        Node[] LeftArray = new Node[n1];
        Node[] RightArray = new Node[n2];
        for (int i = 0; i < n1; i++)
        {
            LeftArray[i] = input[left + i];
        }
        for (int i = 0; i < n2; i++)
        {
            RightArray[i] = input[mid + 1 + i];
        }
        int x = 0, y = 0, z = left;
        while (x < n1 && y < n2)
        {
            if (LeftArray[x].Length < RightArray[y].Length)
            {
                input[z] = LeftArray[x];
                x++;
            }
            else
            {
                input[z] = RightArray[y];
                y++;
            }
            z++;
        }
        while (x < n1)
        {
            input[z] = LeftArray[x];
            x++;
            z++;
        }
        while (y < n2)
        {
            input[z] = RightArray[y];
            y++;
            z++;
        }
        return input;
    }
    private Node[] MergeSortOp(Node[] input, int left, int right)
    {
        int mid;
        if (left < right)
        {
            mid = (left + right) / 2;
            MergeSortOp(input, left, mid);
            MergeSortOp(input, mid + 1, right);
            input = Merge(input, left, mid, right);
        }
        return input;
    }
    public string MergeSort()
    {
        StringBuilder stringBuilder = new StringBuilder();
        Node[] _nodes = nodes;
        _nodes = MergeSortOp(_nodes, 0, ArraySize - 1);
        ArrayDS _arrayDS = new ArrayDS(_nodes, this.Number, "MergeSorted");
        stringBuilder.AppendLine(_arrayDS.Print());
        Console.WriteLine($"Merge sorting {this.Name} complete");
        return stringBuilder.ToString();
    }
}
