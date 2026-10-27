using UnityEngine;

public class ShieldItem : Item
{
    public int m_ShieldPoints = 20;
    PlayerHealth m_Health;
    private void Start()
    {
        m_Health = FindAnyObjectByType<PlayerHealth>();
    }
    public override bool CanPick()
    {
        if (m_Health.m_shield < 100)
            return true;
        else return false;
    }
    public override void Pick()
    {

        base.Pick();
        m_Health.HealShield(m_ShieldPoints);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CanPick())
        {
            Pick();
        }
    }
}
