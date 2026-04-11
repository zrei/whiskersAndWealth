using System.Collections.Generic;
using UnityEngine;

public abstract class Database<T> : ScriptableObject where T : ScriptableObject, IIdentifiable 
{
    public List<T> AllItems;

    public T GetItemById(int id)
    {
        foreach (T item in AllItems)
        {
            if (item.GetId() == id)
                return item;
        }

        return null;
    }
}
