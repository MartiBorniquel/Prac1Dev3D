using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarElementUI : MonoBehaviour
{
    public Image m_LifeBarUI;
    RectTransform m_RecTransform;

    public void Awake()
    {
        m_RecTransform = GetComponent<RectTransform>();
    }
    public void Show(Vector2 Position, float NormalizedLife)
    {
        gameObject.SetActive(true);
        m_RecTransform.anchoredPosition = Position;
        m_LifeBarUI.fillAmount = NormalizedLife;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
