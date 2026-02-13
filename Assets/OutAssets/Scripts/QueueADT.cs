public class QueueADT<T>
{
    System.Collections.Generic.List<T> fifo = new System.Collections.Generic.List<T>();

    public int Count => fifo.Count;

    public bool IsEmpty => Count == 0;
    
    public void Enqueue(T item) => fifo.Add(item);

    public T Dequeue()
    {
        if (IsEmpty) throw new System.Exception("EmptyQueueException");
        T item = fifo[0];
        fifo.RemoveAt(0);
        return item;
    }

    public T Peek()
    {
        if (IsEmpty) throw new System.Exception("EmptyQueueException");
        return fifo[0];
    }

    public void Clear() => fifo.Clear();
}
