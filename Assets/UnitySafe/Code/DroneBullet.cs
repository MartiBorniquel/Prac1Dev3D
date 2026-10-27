//using UnityEngine;

//public class DroneBullet : MonoBehaviour
//{
//    public float m_Speed = 12.0f;
//    public int m_Damage = 10;
//    public float m_LifeTime = 5.0f;

//    Rigidbody m_Rigidbody;

//    void Start()
//    {
//        m_Rigidbody = GetComponent<Rigidbody>();
//        Destroy(gameObject, m_LifeTime);
//    }

//    void FixedUpdate()
//    {
//        m_Rigidbody.MovePosition(
//            m_Rigidbody.position + transform.forward * m_Speed * Time.fixedDeltaTime
//        );
//    }

//    void OnCollisionEnter(Collision Collision)
//    {
//        FPSController l_Player = Collision.collider.GetComponentInParent<FPSController>();

//        if (l_Player != null)
//        {
//            //l_Player.TakeDamage(m_Damage);
//        }

//        Destroy(gameObject);
//    }
//}
