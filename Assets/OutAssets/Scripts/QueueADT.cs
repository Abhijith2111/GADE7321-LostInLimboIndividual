public class QueueADT<T>
{
    System.Collections.Generic.List<T> fifo = new System.Collections.Generic.List<T>();
    
    public void Enqueue(T item) => fifo.Add(item);

    public T Dequeue()
    {
        if (fifo.Count == 0) throw new System.Exception("EmptyQueueException");
        T item = fifo[0];
        fifo.RemoveAt(0);
        return item;
    }

    public T Peek()
    {
        if (fifo.Count == 0) throw new System.Exception("EmptyQueueException");
        return fifo[0];
    }
}
