//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.AI;
//using System.Collections.Generic;
//using System.Collections;

//public class DroneEnemy : MonoBehaviour
//{
//    enum TState
//    {
//        IDLE = 0,
//        PATROL = 1,
//        ALERT = 2,
//        CHASE = 3,
//        ATTACK = 4,
//        HIT = 5,
//        DIE = 6
//    }

//    NavMeshAgent m_NavMeshAgent;
//    TState m_CurrentState = TState.IDLE;
//    public List<Transform> m_Waypoints;
//    int m_CurrentPatrolId;
//    public float m_ChaseDistance = 10;
//    public float m_AlertDistance = 3;
//    public float m_SightDistance = 7;
//    public int m_MaxHealth = 50;
//    public int m_Health;
//    public int m_BodyHit = 20;
//    public int m_HeadHit = 50;
//    public int m_WingsHit = 10;
//    public float m_AngleRotated = 0;
//    public Transform m_LifeBarPosition;

//    public float m_AlertRotationSpeed = 120.0f;
//    public float m_ViewAngle = 80.0f; // 45.0f;
//    public float m_MinDistanceToChase = 10.0f; //15.0f
//    public float m_MaxDistanceToChase = 15.0f; //15.0f
//    public float m_AlertTimer;
//    public float m_RotSpeed = 2000;
//    public int ConeAngle = 160;
//    public int ConeRange = 10;
//    public ParticleSystem AlertParticle;
//    GameObject m_player;
//    Transform m_playerTrans;

//    public float m_ShootMaxDistance;
//    public float m_ShootMinDistance;
//    public float m_ShootInterval = 1.5f;
//    public float m_ShootTimer;
//    public float m_ShootDamage = 10;
//    public Transform m_ShootOrigin;
//    public GameObject m_BulletPrefab;



//    private void Awake()
//    {
//        m_NavMeshAgent = GetComponent<NavMeshAgent>();
//    }

//    private void Start()
//    {
//        SetIdleState();
//        m_Health = m_MaxHealth;
//        GameController.GetGameController().AddEnemy(this);
//    }

//    private void Update()
//    {
//        switch (m_CurrentState)
//        {
//            case TState.IDLE:
//                UpdateIdleState();
//                break;
//            case TState.PATROL:
//                UpdatePatrolState();
//                break;
//            case TState.ALERT:
//                UpdateAlertState();
//                break;
//            case TState.CHASE:
//                UpdateChaseState();
//                break;
//            case TState.ATTACK:
//                UpdateAttackState();
//                break;
//            case TState.HIT:
//                UpdateHitState();
//                break;
//            case TState.DIE:
//                UpdateDieState();
//                break;
//        }


//    }

//    void SetIdleState()
//    {
//        m_CurrentState = TState.IDLE;
//    }

//    void SetPatrolState()
//    {
//        m_CurrentState = TState.PATROL;
//        m_CurrentPatrolId = GetClosestPatrolWaypoint();
//        m_NavMeshAgent.destination = m_Waypoints[m_CurrentPatrolId].position;
//    }

//    int GetClosestPatrolWaypoint()
//    {
//        int l_WaypointId = 0;
//        float l_ClosestDistance = float.MaxValue /* 0.0f*/;
//        for (int i = 0; i < m_Waypoints.Count; ++i)
//        {
//            float l_Distance = Vector3.Distance(transform.position, m_Waypoints[i].position);
//            if (/* i == 0 || */ l_Distance < l_ClosestDistance)
//            {
//                l_ClosestDistance = l_Distance;
//                l_WaypointId = i;
//            }
//        }
//        return l_WaypointId;
//    }

//    void SetAlertState()
//    {
//        m_CurrentState = TState.ALERT;
//    }
//    void SetChaseState()
//    {
//        m_CurrentState = TState.CHASE;
//    }
//    void SetAttackState()
//    {
//        m_CurrentState = TState.ATTACK;
//    }
//    void SetHitState()
//    {
//        m_CurrentState = TState.HIT;
//    }
//    void SetDieState()
//    {
//        m_CurrentState = TState.DIE;
//    }

//    void UpdateIdleState()
//    {
//        Debug.Log("Im gonna patrol Again");
//        SetPatrolState();
//        m_CurrentPatrolId = GetClosestPatrolWaypoint();
//        m_NavMeshAgent.destination = m_Waypoints[m_CurrentPatrolId].position;
//    }

//    void UpdatePatrolState()
//    {
//        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
//        if(Vector3.Distance(transform.position, l_PlayerPosition) < 5)
//        {
//            Debug.Log("Te he oido");
//            SetAlertState();
//        }

//        if (HasArrivedToWaypoint())
//            MoveToNextPatrolPosition();
//        if (Vector3.Distance(transform.position, l_PlayerPosition) <= m_AlertDistance)
//        {
//            Debug.Log(Vector3.Distance(transform.position, l_PlayerPosition));
//            SetAlertState();
//        }
        

//    }

//    void UpdateChaseState()
//    {
//        SetNextChasePosition();
//    }

//    void UpdateAttackState()
//    {
//        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
//        Debug.Log("PiuPiu");
//        float l_Distance = Vector3.Distance(transform.position, l_PlayerPosition);

//        if (l_Distance > m_ShootMaxDistance)
//        {
//            m_NavMeshAgent.isStopped = false;
//            SetChaseState();
//            return;
//        }
//        m_NavMeshAgent.isStopped = true;
//        m_ShootTimer += Time.deltaTime;
//        if (m_ShootTimer >= m_ShootInterval)
//        {
//            ShootPlayer();
//            m_ShootTimer = 0.0f;
//        }
//        // Attack logic here
//    }

//    void UpdateAlertState()
//    {
//        m_AlertTimer += Time.deltaTime;
//        Debug.Log(m_AlertTimer);
//        Debug.Log("Estoy en alerta");
//        m_AlertTimer += Time.deltaTime;
//        m_NavMeshAgent.isStopped = true;

//        if (m_AlertTimer >= 3)
//        {
//            Debug.Log("Se acabo el timer");
//            m_NavMeshAgent.isStopped = false;
//            m_AlertTimer = 0;
//            SetPatrolState();
//        }
//        else 
//        {
//            Debug.Log("Debo Rotar");
//            transform.Rotate(0.0f, 200f * Time.deltaTime, 0.0f);
//        }

//        if (ConoVisual())
//        {
//            Debug.Log("Te persigo");
//            SetChaseState();
//        }
        

//        ////float step = m_RotSpeed * Time.deltaTime;
//        ////transform.Rotate(0f, step, 0f, Space.World);
//        ////m_AngleRotated += step;


       

//        //if (m_AngleRotated >= 360f)
//        //{
//        //    m_AngleRotated = 0f;
//        //    SetPatrolState();
//        //}

//    }

//    void UpdateHitState()
//    {
//        // Hit logic here
//    }

//    void UpdateDieState()
//    {
//        // Die logic here
//    }

//    bool HasArrivedToWaypoint()
//    {
//        return !m_NavMeshAgent.pathPending && m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance;
//        // return !m_NavNeshAgent.hasPath 
//    }
//    void MoveToNextPatrolPosition()
//    {
//        m_CurrentPatrolId = (m_CurrentPatrolId + 1) % m_Waypoints.Count;
//        m_NavMeshAgent.destination = m_Waypoints[m_CurrentPatrolId].position;
//    }

//    void SetNextChasePosition()
//    {
//        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
//        float l_Distance = Vector3.Distance(transform.position, l_PlayerPosition);

//        if (l_Distance > m_MaxDistanceToChase)
//        {
//            if (m_Waypoints.Count == 0) return;
//            m_NavMeshAgent.isStopped = false;
//            SetPatrolState();
//            m_CurrentPatrolId = GetClosestPatrolWaypoint();
//            m_NavMeshAgent.destination = m_Waypoints[m_CurrentPatrolId].position;
//        }
//        else if (l_Distance <= 10)
//        {
//            m_NavMeshAgent.isStopped = true;
//            SetAttackState();
//        }
//        else
//        {
//            m_NavMeshAgent.isStopped = false;
//            m_NavMeshAgent.SetDestination(l_PlayerPosition);
//        }
//        //Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
//        //Vector3 l_Direction = (l_PlayerPosition - transform.position).normalized;
//        //m_NavMeshAgent.destination = transform.position + l_Direction * m_ChaseDistance;
//        Debug.Log("Que te pillo");
//        //if (Vector3.Distance(transform.position, l_PlayerPosition) > 10f)
//        //{
//        //    Debug.Log("ya no te veo");
//        //    SetIdleState();
//        //}
//    }

//    bool SeesPlayer()
//    {
//        GameObject player;

        


//        return true;
//    }

//    public void Hit(HitCollider.HitColliderType HitColliderType )
//    {
//        int l_LifePoints = 0;
//        switch (HitColliderType)
//        {
//            case HitCollider.HitColliderType.BODY:
//                l_LifePoints -= m_BodyHit;
//                break;
//            case HitCollider.HitColliderType.HEAD:
//                l_LifePoints -= m_HeadHit;
//                break;
//            case HitCollider.HitColliderType.WINGS:
//                l_LifePoints -= m_WingsHit;
//                break;
//            default:
//                Debug.Log("HitCollider not registered");
//                break;
                
         
//        }
//        m_Health+= l_LifePoints;
//        if (m_Health <= 0)
//        {
//            SetDieState();

//        }
//        else
//            SetHitState();
//        Debug.Log("Current health" + m_Health);
//    }
//    public int GetLife()
//    {
//        return m_Health;
//    }

//    public bool ConoVisual()
//    {
//        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
//        Vector3 EnemyDirection = ((l_PlayerPosition - transform.position).normalized);

//        float ProdPunt = Vector3.Dot(EnemyDirection, transform.forward);

//        float anguloTransformado = (1 -( ConeAngle * 0.005555f));
//        if (anguloTransformado <= 0 ) { anguloTransformado = 0; }

//        float enemyDistanceToObj = Vector3.Distance(l_PlayerPosition, transform.position);
//        if (enemyDistanceToObj < ConeRange && ProdPunt > anguloTransformado) { return true; } else { return false; }
//    }

//    public void ShootPlayer()
//    {
//        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
//        if (m_BulletPrefab == null || m_ShootOrigin == null) return;

//        Vector3 l_Direction = (l_PlayerPosition - m_ShootOrigin.position).normalized;
//        Quaternion l_Rotation = Quaternion.LookRotation(l_Direction);

//        Instantiate(m_BulletPrefab, m_ShootOrigin.position, l_Rotation);
//    }

//    void OnDrawGizmosSelected()
//{
//    // Mismo umbral que usas en ConoVisual()
//    float umbral = Mathf.Max(0f, 1f - ConeAngle * 0.005555f);
//    // Ángulo real (mitad del cono) que corresponde a ese umbral del producto escalar
//    float halfAngle = Mathf.Acos(umbral) * Mathf.Rad2Deg;

//    Vector3 origen = transform.position;
//    Vector3 izq = Quaternion.AngleAxis(-halfAngle, transform.up) * transform.forward;
//    Vector3 der = Quaternion.AngleAxis(halfAngle, transform.up) * transform.forward;

//    Gizmos.color = Color.yellow;
//    Gizmos.DrawLine(origen, origen + izq * ConeRange);
//    Gizmos.DrawLine(origen, origen + der * ConeRange);
//    Gizmos.DrawLine(origen + izq * ConeRange, origen + der * ConeRange);

//    Gizmos.color = Color.red;
//    Gizmos.DrawRay(origen, transform.forward * ConeRange);
//}
//}
