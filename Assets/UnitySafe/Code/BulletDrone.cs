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
        // Ignorar triggers (zonas, etc.), otras balas y a los propios drones
        if (other.isTrigger) return;
        if (other.GetComponentInParent<DroneBullet>() != null) return;
        if (other.GetComponentInParent<DroneEnemy>() != null) return;

        FPSController l_Player = other.GetComponentInParent<FPSController>();
        if (l_Player != null)
        {
            //l_Player.TakeDamage(m_Damage);
            Debug.Log("PlayerDañado");
        }

        Destroy(gameObject);
    }
}