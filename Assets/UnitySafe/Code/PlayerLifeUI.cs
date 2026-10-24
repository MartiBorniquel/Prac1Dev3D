
using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth m_PlayerHealth;
    public FPSController m_Player;
    public TMP_Text m_HealthText;
    public TMP_Text m_ShieldText;
    public TMP_Text m_AmmoText;

    void Start()
    {
        if (m_PlayerHealth == null)
        {
            m_PlayerHealth = FindObjectOfType<PlayerHealth>();
        }

        if (m_Player == null)
        {
            m_Player = FindObjectOfType<FPSController>();
        }
    }

    void Update()
    {
        m_HealthText.text = m_PlayerHealth.m_health + " / 100";
        m_ShieldText.text = m_PlayerHealth.m_shield + " / 100";

        m_AmmoText.text = m_Player.m_CurrentBullets + " / "
            + m_Player.m_MaxBullets + "\nReserve: "
            + m_Player.m_InventoryBullets;
    }
}
