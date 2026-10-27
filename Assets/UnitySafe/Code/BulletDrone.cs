using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneBullet : MonoBehaviour
{
    public float m_Speed = 12.0f;
    public int m_Damage = 10;
    public float m_LifeTime = 5.0f;

    Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, m_LifeTime);
    }

    void FixedUpdate()
    {
        m_Rigidbody.MovePosition(
            m_Rigidbody.position + transform.forward * m_Speed * Time.fixedDeltaTime
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.GetComponentInParent<DroneBullet>() != null) return;
        if (other.GetComponentInParent<DroneEnemy>() != null) return;
  
        PlayerHealth l_PlayerHealth = other.GetComponentInParent<PlayerHealth>();
        if (l_PlayerHealth != null)
        {
            l_PlayerHealth.TakeDamage(m_Damage);
        }

        Destroy(gameObject);
    }
}