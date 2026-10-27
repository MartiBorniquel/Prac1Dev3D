using UnityEngine;

public class LifeItem : Item
{
    public int m_LifePoints = 20;
    PlayerHealth m_Health;
    private void Start()
    {
        m_Health = FindAnyObjectByType<PlayerHealth>();
    }
    public override bool CanPick()
    {
        if (m_Health.m_health<100)
        return true;
        else return false;
    }
    public override void Pick()
    {
        base.Pick();
        m_Health.HealLife(m_LifePoints);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CanPick())
        {
            Pick();
        }
    }
}
