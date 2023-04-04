using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
    public List<TItem> Items { get; }
    public int Limit { get; }
    public int V { get; }

    private readonly LimitedSizeStack<Tuple<ActionType, int, TItem>> stack;

    public enum ActionType
    {
        AddItem, RemoveItem, MoveUp
    }

    public ListModel(List<string> list, int limit)
    {
        Items = new List<TItem>();
        Limit = limit;
        stack = new LimitedSizeStack<Tuple<ActionType, int, TItem>>(limit);
    }

    public ListModel(int v)
    {
        V = v;
    }

    public void AddItem(TItem item)
    {
        stack.Push(Tuple.Create(ActionType.AddItem, Items.Count, item));
        Items.Add(item);
    }

    public void RemoveItem(int index)
    {
        stack.Push(Tuple.Create(ActionType.RemoveItem, index, Items[index]));
        Items.RemoveAt(index);
    }

    public bool CanUndo()
    {
        return stack.Count > 0;
    }

    public void Undo()
    {
        var item = stack.Pop();
        if (item.Item1 == ActionType.AddItem) Items.RemoveAt(stack.Count);
        else if (item.Item1 == ActionType.RemoveItem)
        {
            if (stack.Count == 1) Items.Insert(item.Item2 - 1, item.Item3);
            else Items.Insert(item.Item2, item.Item3);
        }
        else if (item.Item1 == ActionType.MoveUp) 
        {
            var tmp = Items[item.Item2];
            Items[item.Item2] = Items[item.Item2 - 1];
            Items[item.Item2 - 1] = tmp;
        }
    }

     internal void MoveUpItem(int index)
     {
        
        if (index <= 0)
        {
            return;
        }
        var tmp = Items[index];
        Items[index] = Items[index - 1];
        Items[index - 1] = tmp;
        stack.Push(Tuple.Create(ActionType.MoveUp, index, Items[index]));

     }
}