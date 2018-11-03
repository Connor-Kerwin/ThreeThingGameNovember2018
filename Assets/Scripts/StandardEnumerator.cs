using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnumerator<T> : IEnumerator<T>
{
    private List<T> items;

    int position = -1;

    public StandardEnumerator(List<T> items)
    {
        this.items = items;
    }

    public T Current
    {
        get
        {
            try
            {
                return items[position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }

    object IEnumerator.Current { get { return Current; } }

    public void Dispose()
    {
        // No disposal
    }

    public bool MoveNext()
    {
        position++;
        return (position < items.Count);
    }

    public void Reset()
    {
        position = -1;
    }
}