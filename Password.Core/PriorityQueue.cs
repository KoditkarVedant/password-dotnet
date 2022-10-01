using System.Diagnostics.CodeAnalysis;

namespace Password.Core;

public class PriorityQueue<TElement, TPriority>
{
    private readonly (TElement element, TPriority priority)[] _nodes;

    private readonly IComparer<TPriority> _comparer;

    private int _size;

    public int Count => _size;

    public PriorityQueue(int capacity)
    {
        _size = 0;
        _nodes = new (TElement, TPriority)[capacity];
        _comparer = InitializeComparer(null);
    }

    private static IComparer<TPriority> InitializeComparer(IComparer<TPriority>? comparer)
    {
        return comparer ?? Comparer<TPriority>.Default;
    }

    public void Enqueue(TElement element, TPriority priority)
    {
        int currentSize = _size++;

        if (_nodes.Length == currentSize)
        {
            throw new OverflowException();
        }

        MoveUp((element, priority), currentSize);
    }

    private TElement Dequeue()
    {
        if (_size == 0)
        {
            throw new InvalidOperationException("Queue is empty!");
        }

        TElement element = _nodes[0].element;
        RemoveRootNode();
        return element;
    }

    public bool TryDequeue([MaybeNullWhen(false)] out TElement element)
    {
        if (_size == 0)
        {
            element = default;
            return false;
        }

        element = Dequeue();
        return true;
    }

    private void RemoveRootNode()
    {
        // Added to protect misuse of this method which might result in _size set to negative number.
        if (_size <= 0)
        {
            return;
        }

        int lastNodeIndex = --_size;

        if (lastNodeIndex > 0)
        {
            var lastNode = _nodes[lastNodeIndex];

            MoveDown(lastNode, 0);
        }
    }

    private void MoveUp((TElement element, TPriority priority) node, int nodeIndex)
    {
        while (nodeIndex > 0)
        {
            int parentIndex = GetParentIndex(nodeIndex);

            var parent = _nodes[parentIndex];

            if (_comparer.Compare(node.priority, parent.priority) < 0)
            {
                _nodes[nodeIndex] = parent;
                nodeIndex = parentIndex;
            }
            else
            {
                break;
            }
        }

        _nodes[nodeIndex] = node;
    }

    private void MoveDown((TElement element, TPriority priority) node, int nodeIndex)
    {
        var nodes = _nodes;
        var size = _size;

        int i;

        // Find the first (left size) child of a parent except the `node` child.
        while ((i = GetLeftChildIndex(nodeIndex)) < size)
        {
            // Assume left as the minChild to be next parent.
            int minChildIndex = i;
            var minChild = nodes[i];

            // Calculate the upperBound of child index 
            // We only have two children per parent.
            int childIndexUpperBound = i + 2;

            // Find the minChild amongst the children
            while (++i < childIndexUpperBound)
            {
                var nextChild = nodes[i];

                if (_comparer.Compare(nextChild.priority, minChild.priority) < 0)
                {
                    minChild = nextChild;
                    minChildIndex = i;
                }
            }

            // If we the `Node` parameter has the less or equal priority than the current parent 
            // break the loop as we found a place where the node should be insert.
            if (_comparer.Compare(node.priority, minChild.priority) <= 0)
            {
                break;
            }

            // Traverse the Minimum child tree
            nodes[nodeIndex] = minChild;
            nodeIndex = minChildIndex;
        }

        nodes[nodeIndex] = node;
    }

    private static int GetLeftChildIndex(int nodeIndex) => (nodeIndex * 2) + 1;

    private static int GetParentIndex(int nodeIndex) => (nodeIndex - 1) / 2;
}
