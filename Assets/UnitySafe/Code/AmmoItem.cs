using UnityEngine;

public class AmmoItem : Item
{

    public int m_AmmoCount = 30;
    FPSController m_Player;
    private void Start()
    {
        m_Player = FindObjectOfType<FPSController>();
    }
    public override bool CanPick()
    {
        return true;
    }
    public override void Pick()
    {
        base.Pick();
        m_Player.m_InventoryBullets += m_AmmoCount;
    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CanPick())
        {
            Pick();
        }
    }

}
