
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling SharedInstance;

    public List<GameObject> m_pooledObjects;
    public GameObject m_objectToPool;

    public int m_amountToPool = 25;

    private int m_nextObjectIndex = 0;

    void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        m_pooledObjects = new List<GameObject>();

        for (int i = 0; i < m_amountToPool; i++)
        {
            GameObject tmp = Instantiate(m_objectToPool);
            tmp.SetActive(false);
            m_pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        if (m_pooledObjects == null || m_pooledObjects.Count == 0)
            return null;

        GameObject pooledObject = m_pooledObjects[m_nextObjectIndex];
        m_nextObjectIndex++;

        if (m_nextObjectIndex >= m_pooledObjects.Count)
            m_nextObjectIndex = 0;

        return pooledObject;
    }
}
