using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T>
{
    private Func<T> create;
    private Queue<T> items;

    public Pool(Func<T> create)
    {
        items = new Queue<T>();

        if (create != null)
        {
            this.create = create;
        }
        else
        {
            throw new System.Exception("Pool provided with an invalid creation delegate");
        }
    }

    public T Get()
    {
        T item;

        if (items.Count > 0) // Is there an item to retrieve?
        {
            item = items.Dequeue();
        }
        else // No more items, create one
        {
            item = Create();
        }

        return item;
    }

    public void Store(T item)
    {
        if (!items.Contains(item))
        {
            items.Enqueue(item);
        }
    }

    public void PreWarm(int count, Action<T> processItem)
    {
        for(int i = 0; i < count; i++)
        {
            T item = Create();
            processItem.Invoke(item);
            Store(item);
        }
    }

    private T Create()
    {
        T instance = create.Invoke();
        return instance;
    }
}