using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CPoolElementss
{

    List<GameObject> m_Elements;
    int m_CurrentelementId = 0;

    public CPoolElementss(int ElementsCount, GameObject ElementsPrefab)
    {
        m_Elements = new List<GameObject>();
        for (int i = 0; i < ElementsCount; i++)
        {
            GameObject l_GameObject = GameObject.Instantiate(ElementsPrefab);
            l_GameObject.gameObject.SetActive(false);
            m_Elements.Add(l_GameObject);
        }
    }

    public GameObject GetnextElement()
    {
        m_CurrentelementId=(m_CurrentelementId+1)%m_Elements.Count;
        return m_Elements[m_CurrentelementId];
    }

}

