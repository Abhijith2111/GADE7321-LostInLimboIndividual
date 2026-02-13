public class StackADT<T>
{
    System.Collections.Generic.List<T> lifo = new System.Collections.Generic.List<T>();

    public int Count => lifo.Count;

    public bool IsEmpty => Count == 0;

    public void Push(T item) => lifo.Add(item);

    public T Pop()
    {
        if (IsEmpty) throw new System.Exception("EmptyStackException");
        T item = lifo[lifo.Count - 1];
        lifo.RemoveAt(lifo.Count - 1);
        return item;
    }

    public T Peek()
    {
        if (IsEmpty) throw new System.Exception("EmptyStackException");
        return lifo[lifo.Count - 1];
    }

    public void Clear() => lifo.Clear();
}
