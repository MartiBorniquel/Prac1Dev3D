using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int m_MaxHealth = 100;
    int m_MaxShield = 100;
    public int m_health;
    public int m_shield;
    FPSController m_Player;

    void Start()
    {
        m_health = m_MaxHealth;
        m_shield = m_MaxShield;
        m_Player = FindObjectOfType<FPSController>();
    }

    void Update()
    {
        if (m_health <= 0)
        {
            m_Player.Restart();
        }
    }

    public void TakeDamage(int damageReceived)
    {
        if (m_shield > 0)
        {
            int shieldDamage = (int)(damageReceived * 0.75f);
            int healthDamage = damageReceived - shieldDamage;

            if (shieldDamage > m_shield)
            {
                int remainingDamage = shieldDamage - m_shield;
                m_health -= healthDamage + remainingDamage;
                m_shield = 0;
            }
            else
            {
                m_shield -= shieldDamage;
                m_health -= healthDamage;
            }
        }
        else
        {
            m_health -= damageReceived;
        }

        m_health = Mathf.Clamp(m_health, 0, m_MaxHealth);
        m_shield = Mathf.Clamp(m_shield, 0, m_MaxShield);
    }
    public void HealLife(int amount)
    {
        m_health += amount;
        m_health = Mathf.Clamp(m_health, 0, m_MaxHealth);
    }
    public void HealShield(int amount)
    {
        m_shield += amount;
        m_shield = Mathf.Clamp(m_shield, 0, m_MaxShield);

    }
}