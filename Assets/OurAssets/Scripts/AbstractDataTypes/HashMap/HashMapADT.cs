using System;
using System.Collections;
using System.Collections.Generic;

// This should not count less marks than the GraphADT. The hashing
// and buckets are standard and make doing this way more complex
// than a graph.

// This code ended up being a mix of functionality from c# and c++
// combining some of the feature I prefer from both

public class HashMapADT<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
{
    public int Count { get; private set; }

    public bool IsReadOnly => false;

    public HashMapADT() : this(0) { } // Default to 0 capacity

    public HashMapADT(int capacity)
    {
        if (capacity < 0) throw new ArgumentException("capacity needs to be greater than or equal to 0");
        if (capacity > 0) Initialise(capacity);
        Count = 0;
        m_NextFreeIndex = -1;
        m_KeyComparer = EqualityComparer<TKey>.Default;
        m_ValueComparer = EqualityComparer<TValue>.Default;
    }

    public HashMapADT(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        Count = 0;
        m_NextFreeIndex = -1;
        m_KeyComparer = EqualityComparer<TKey>.Default;
        m_ValueComparer = EqualityComparer<TValue>.Default;
        foreach (KeyValuePair<TKey, TValue> item in collection) Add(item);
    }

    public TValue this[TKey key]
    {
        get
        {
            int index = GetEntryIndex(key);
            return index >= 0 ? m_Entries[index].Value : default;
        }
        set => Insert(key, value); // Update key's value or insert if it doesn't exist similar to c++
    }

    public bool ContainsKey(TKey key) => GetEntryIndex(key) >= 0;

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        if (item.Key == null) throw new ArgumentNullException(nameof(item.Key));
        Insert(item.Key, item.Value);
    }

    public void Add(TKey key, TValue value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        Insert(key, value);
    }

    public void Clear()
    {
        // I love you garbage collector for letting me just set them to null <3
        m_Buckets = null;
        m_Entries = null;
        Count = 0;
        m_NextFreeIndex = -1;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        if (item.Key == null) throw new ArgumentNullException(nameof(item.Key));
        int index = GetEntryIndex(item.Key);
        return index >= 0 && m_ValueComparer.Equals(m_Entries[index].Value, item.Value);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0 || arrayIndex > array.Length) throw new IndexOutOfRangeException($"{nameof(arrayIndex)} out of range");
        if (array.Length - arrayIndex < Count) throw new ArgumentException($"not enough space for the HashMapATD in {nameof(array)}");
        for (int i = 0; i < m_Entries.Length; ++i)
        {
            if (m_Entries[i].HashCode >= 0) array[arrayIndex++] = new KeyValuePair<TKey, TValue>(m_Entries[i].Key, m_Entries[i].Value);
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => new Enumerator(this);

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (item.Key == null) throw new ArgumentNullException(nameof(item.Key));
        int index = GetEntryIndex(item.Key);
        if (index >= 0 && m_ValueComparer.Equals(m_Entries[index].Value, item.Value))
        {
            Remove(item.Key);
            return true;
        }
        return false;
    }

    public void Remove(TKey key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (m_Buckets == null) return;
        var (hashCode, bucketIndex, entryIndex) = Hash(key);
        int prev = -1;
        for (int i = entryIndex; i >= 0; prev = i, i = m_Entries[i].NextIndex)
        {
            if (m_Entries[i].HashCode == hashCode && m_KeyComparer.Equals(m_Entries[i].Key, key))
            {
                if (prev < 0) m_Buckets[bucketIndex] = m_Entries[i].NextIndex; // Head so set the bucket to point at next index instead
                else m_Entries[prev].NextIndex = m_Entries[i].NextIndex; // Not head so set entry that pointed to this, to point at whatever this pointed to
                m_Entries[i].HashCode = -1;
                m_Entries[i].NextIndex = m_NextFreeIndex;
                m_Entries[i].Key = default;
                m_Entries[i].Value = default;
                m_NextFreeIndex = i;
                if (--Count < m_Entries.Length / 4) Resize(m_Entries.Length / 2);
                return; // Found so stop looping
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    (int hashCode, int bucketIndex, int entryIndex) Hash(TKey key)
    {
        int hashCode = m_KeyComparer.GetHashCode(key) & int.MaxValue; // Keep positive
        int bucketIndex = hashCode % m_Buckets.Length;
        int entryIndex = m_Buckets[bucketIndex];
        return (hashCode, bucketIndex, entryIndex);
    }

    void Initialise(int capacity)
    {
        m_Buckets = new int[capacity];
        Array.Fill(m_Buckets, -1);
        m_Entries = new Entry[capacity];
    }

    int GetEntryIndex(TKey key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (m_Buckets != null)
        {
            var (hashCode, bucketIndex, entryIndex) = Hash(key);
            for (int i = entryIndex; i >= 0; i = m_Entries[i].NextIndex)
            {
                if (m_Entries[i].HashCode == hashCode && m_KeyComparer.Equals(m_Entries[i].Key, key))
                    return i;
            }
        }
        return -1;
    }

    private void Insert(TKey key, TValue value)
    {
        if (m_Buckets == null) Initialise(1);
        var (hashCode, bucketIndex, entryIndex) = Hash(key);
        for (int i = entryIndex; i >= 0; i = m_Entries[i].NextIndex)
        {
            if (m_Entries[i].HashCode == hashCode && m_KeyComparer.Equals(m_Entries[i].Key, key))
            {
                m_Entries[i].Value = value;
                return;
            }
        }
        int index = -1;
        if (m_NextFreeIndex >= 0)
        {
            index = m_NextFreeIndex;
            m_NextFreeIndex = m_Entries[m_NextFreeIndex].NextIndex;
        }
        else if (Count >= m_Entries.Length)
        {
            Resize(Count * 2);
            (hashCode, bucketIndex, entryIndex) = Hash(key);
        }
        if (index < 0) index = Count;
        m_Entries[index].HashCode = hashCode;
        m_Entries[index].NextIndex = entryIndex;
        m_Entries[index].Key = key;
        m_Entries[index].Value = value;
        m_Buckets[bucketIndex] = index;
        ++Count;
    }

    // Set new capacity and recalculate hashes
    void Resize(int newCapacity)
    {
        m_Buckets = new int[newCapacity];
        Array.Fill(m_Buckets, -1);
        Entry[] entries = m_Entries;
        m_Entries = new Entry[newCapacity];
        Count = 0;
        foreach (Entry entry in entries)
        {
            if (entry.HashCode >= 0) Insert(entry.Key, entry.Value);
        }
    }

    struct Entry
    {
        public int HashCode;
        public int NextIndex;
        public TKey Key;
        public TValue Value;
    }

    int[] m_Buckets;
    Entry[] m_Entries;
    int m_NextFreeIndex;
    IEqualityComparer<TKey> m_KeyComparer;
    IEqualityComparer<TValue> m_ValueComparer;

    public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        public KeyValuePair<TKey, TValue> Current => new KeyValuePair<TKey, TValue>(m_Current.Key, m_Current.Value);

        object IEnumerator.Current => Current;

        internal Enumerator(HashMapADT<TKey, TValue> hashMap)
        {
            m_HashMap = hashMap;
            m_Index = 0;
            m_Current = default;
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            while (m_Index < m_HashMap.m_Entries.Length)
            {
                Entry entry = m_HashMap.m_Entries[m_Index++];
                if (entry.HashCode >= 0)
                {
                    m_Current = entry;
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            m_Index = 0;
            m_Current = default;
        }

        HashMapADT<TKey, TValue> m_HashMap;
        int m_Index;
        Entry m_Current;
    }
}
