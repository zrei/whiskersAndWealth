using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField] private T m_PoolObjPrefab;
    [SerializeField] private Transform m_UnusedParent;
    [SerializeField] private int m_StartingAmountOfObjectsToInstantiate;

    private HashSet<T> m_UnusedPoolObjs = new();

    private void Start()
    {
        InitialisePool();
    }

    private void InitialisePool()
    {
        while (m_UnusedPoolObjs.Count < m_StartingAmountOfObjectsToInstantiate)
            AddObjectToPool();
    }

    private T AddObjectToPool()
    {
        T poolObj = Instantiate<T>(m_PoolObjPrefab);
        poolObj.transform.localScale = Vector3.one;
        poolObj.transform.rotation = Quaternion.identity;
        poolObj.transform.parent = m_UnusedParent;
        poolObj.gameObject.SetActive(false);
        m_UnusedPoolObjs.Add(poolObj);
        return poolObj;
    }

    public T GetObjectFromPool(bool setActive = false, Transform parent = null)
    {
        T poolObj;
        if (m_UnusedPoolObjs.Count > 0)
        {
            poolObj = m_UnusedPoolObjs.First();
        }
        else
        {
            poolObj = AddObjectToPool();
        }

        m_UnusedPoolObjs.Remove(poolObj);
        poolObj.gameObject.SetActive(setActive);
        poolObj.transform.parent = parent;
        return poolObj;
    }

    public void ReturnPoolObj(T poolObj)
    {
        poolObj.gameObject.SetActive(false);
        m_UnusedPoolObjs.Add(poolObj);
        poolObj.transform.parent = m_UnusedParent;
    }   
}
