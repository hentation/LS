using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private readonly int size;
    private readonly LinkedList<T> items = new LinkedList<T>();

    public LimitedSizeStack(int undoLimit)
    {
        size = undoLimit;
    }

    public void Push(T item)
    {
        if (items.Count != size)
            items.AddLast(item);
        else
        {
            items.AddLast(item);
            items.RemoveFirst();
        }
    }

    public T Pop()
    {
        var value = items.Last.Value;
        items.RemoveLast();
        return value;
    }

    public int Count => items.Count;
}