using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CPoolElements
{
    List<GameObject> m_Elements;
    int m_CurremtElementId = 0;
    public CPoolElements(int ElementCount, GameObject ElementPrefab)
    {
        m_Elements = new List<GameObject>();
        for(int i=0; i<ElementCount; ++i)
        {
            GameObject l_GameObject = GameObject.Instantiate(ElementPrefab);
            l_GameObject.gameObject.SetActive(false);
            m_Elements.Add(l_GameObject);
        }
    }
    public GameObject GetNextElement()
    {
        m_CurremtElementId = (m_CurremtElementId + 1) % m_Elements.Count;
        return m_Elements[m_CurremtElementId];
    }
}
