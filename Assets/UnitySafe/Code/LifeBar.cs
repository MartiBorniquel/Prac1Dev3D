using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LifeBar : MonoBehaviour
{
    public Image m_LifeBarImage;
    RectTransform m_RectTransform;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }



    public void Show(Vector3 Position, float NormalizedLife)
    {
        gameObject.SetActive(true);
        m_RectTransform.anchoredPosition=Position;
        m_LifeBarImage.fillAmount=NormalizedLife;
    }
    public void Hide()
    {

    }
}
