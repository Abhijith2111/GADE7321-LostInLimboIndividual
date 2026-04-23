public class LinkedListADT<T>
{
	public LinkedListADTNode<T> Front { get; private set; } = null;
	public LinkedListADTNode<T> Back { get; private set; } = null;
	public int Count { get; private set; } = 0;
	public bool IsEmpty => Count <= 0;

	public LinkedListADT() { }

	public void AddBefore(LinkedListADTNode<T> node, LinkedListADTNode<T> newNode)
	{
		if (node == null) throw new System.ArgumentNullException("node is null");
		if (newNode == null) throw new System.ArgumentNullException("newNode is null");
		if (node.List != this) throw new System.ArgumentException("node doesn't belong to this list");
		if (newNode.List != null) throw new System.ArgumentException("newNode already belongs to a list");
		if (node == Front) AddFront(newNode);
		else InternalAddBefore(node, newNode);
	}

	public void AddAfter(LinkedListADTNode<T> node, LinkedListADTNode<T> newNode)
	{
		if (node == null) throw new System.ArgumentNullException("node is null");
		if (newNode == null) throw new System.ArgumentNullException("newNode is null");
		if (node.List != this) throw new System.ArgumentException("node doesn't belong to this list");
		if (newNode.List != null) throw new System.ArgumentException("newNode already belongs to a list");
		if (node == Back) AddBack(newNode);
		else InternalAddAfter(node, newNode);
	}

	public void AddFront(LinkedListADTNode<T> node)
	{
		if (node == null) throw new System.ArgumentNullException("node is null");
		if (node.List != null) throw new System.ArgumentException("node already belongs to a list");
		if (Front == null) CreateList(node);
		else
		{
			InternalAddBefore(Front, node);
			Front = node;
		}
	}

	public LinkedListADTNode<T> AddFront(T value)
	{
		LinkedListADTNode<T> node = new LinkedListADTNode<T>(value: value, list: this);
		if (Front == null) CreateList(node);
		else
		{
			InternalAddBefore(Front, node);
			Front = node;
		}
		return node;
	}

	public void RemoveFront()
	{
		if (Front == null) return;
		LinkedListADTNode<T> newFront = Front._next;
		InternalRemove(Front);
		Front = newFront;
		--Count;
	}

	public void AddBack(LinkedListADTNode<T> node)
	{
		if (node == null) throw new System.ArgumentNullException("node is null");
		if (node.List != null) throw new System.ArgumentException("node already belongs to a list");
		if (Back == null) CreateList(node);
		else
		{
			InternalAddAfter(Back, node);
			Back = node;
		}
	}

	public LinkedListADTNode<T> AddBack(T value)
	{
		LinkedListADTNode<T> node = new LinkedListADTNode<T>(value: value, list: this);
		if (Back == null) CreateList(node);
		else
		{
			InternalAddAfter(Back, node);
			Back = node;
		}
		return node;
	}

	public void RemoveBack()
	{
		if (Back == null) return;
		LinkedListADTNode<T> newBack = Back._previous;
		InternalRemove(Back);
		Back = newBack;
		--Count;
	}

	public LinkedListADTNode<T> FindFirst(T value)
	{
		LinkedListADTNode<T> current = Front;
		while (current != null)
		{
			if (current._value.Equals(value)) return current;
			current = current._next;
		}
		return null;
	}

	public void RemoveFirst(T value)
	{
		LinkedListADTNode<T> node = FindFirst(value);
		if (node == null) return;
		if (node == Front) RemoveFront();
		else InternalRemove(node);
	}

	public LinkedListADTNode<T> FindLast(T value)
	{
		LinkedListADTNode<T> current = Back;
		while (current != null)
		{
			if (current._value.Equals(value)) return current;
			current = current._previous;
		}
		return null;
	}

	public void RemoveLast(T value)
	{
		LinkedListADTNode<T> node = FindLast(value);
		if (node == null) return;
		if (node == Back) RemoveBack();
		else InternalRemove(node);
	}

	public bool Contains(T value) => FindFirst(value) != null;

	public void Clear()
	{
		LinkedListADTNode<T> current = Front;
		while (current != null)
		{
			LinkedListADTNode<T> next = current._next;
			current.FreeMemory();
			current = next;
		}
		Front = null;
		Back = null;
		Count = 0;
	}

	private void CreateList(LinkedListADTNode<T> node)
	{
		node._next = null;
		node._previous = null;
		Front = node;
		Back = node;
		Count = 1;
	}

	private void InternalAddBefore(LinkedListADTNode<T> node, LinkedListADTNode<T> newNode)
	{
		newNode._next = node;
		newNode._previous = node._previous;
		if (node._previous != null) node._previous._next = newNode;
		node._previous = newNode;
		++Count;
	}

	private void InternalAddAfter(LinkedListADTNode<T> node, LinkedListADTNode<T> newNode)
	{
		newNode._previous = node;
		newNode._next = node._next;
		if (node._next != null) node._next._previous = newNode;
		node._next = newNode;
		++Count;
	}

	private void InternalRemove(LinkedListADTNode<T> node)
	{
		if (node._previous != null) node._previous._next = node._next;
		if (node._next != null) node._next._previous = node._previous;
		node.FreeMemory();
	}
}

public sealed class LinkedListADTNode<T>
{
	public T Value { get => _value; set => _value = value; }
	public ref T ValueRef => ref _value;

	public LinkedListADTNode<T> Previous => _previous == null || List == null || List.Front == null || List.Back == null ? null : _previous;
	public LinkedListADTNode<T> Next => _next == null || List == null || List.Front == null || List.Back == null ? null : _next;

	public LinkedListADT<T> List { get; internal set; }

	public LinkedListADTNode(T value) { _value = value; }

	internal T _value;
	internal LinkedListADTNode<T> _previous;
	internal LinkedListADTNode<T> _next;

	internal LinkedListADTNode(T value, LinkedListADT<T> list)
	{
		_value = value;
		List = list;
	}

	internal void FreeMemory()
	{
		List = null;
		_previous = null;
		_next = null;
	}
}
