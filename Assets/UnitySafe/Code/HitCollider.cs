using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public enum HitColliderType
    {
        HEAD=0,
        BODY,
        WINGS

    }
    public HitColliderType m_Type;
    public DroneEnemy m_DroneEnemy;

    public void Hit()
    {
        m_DroneEnemy.Hit(m_Type);
    }
}
