
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling SharedInstance;

    public List<GameObject> pooledObjects;
    public GameObject objectToPool;

    public int amountToPool = 25;

    private int nextObjectIndex = 0;

    void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            GameObject tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        if (pooledObjects == null || pooledObjects.Count == 0)
            return null;

        GameObject pooledObject = pooledObjects[nextObjectIndex];
        nextObjectIndex++;

        if (nextObjectIndex >= pooledObjects.Count)
            nextObjectIndex = 0;

        return pooledObject;
    }
}
