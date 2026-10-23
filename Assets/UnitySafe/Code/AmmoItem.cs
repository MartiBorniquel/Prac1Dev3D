using UnityEngine;

public class AmmoItem : Item
{

    public int m_AmmoCount = 20;
    FPSController m_Player;
    public override bool CanPick()
    {
        return true;
    }
    public override void Pick()
    {
        base.Pick();
        m_Player.m_InventoryBullets += m_AmmoCount;
        Debug.Log("Picked Ammo item"+m_Player.m_InventoryBullets);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CanPick())
        {
            Pick();
        }
    }
}
