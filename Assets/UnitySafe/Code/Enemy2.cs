//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//public class Enemy2 : MonoBehaviour
//{
//    enum Tstate
//    {
//        IDLE = 0,
//        PATROL,
//        CHASE,
//        ALERT,
//        ATTACK,
//        HIT,
//        DIE,

//    }

//    NavMeshAgent m_NavMeshAgent;
//    Tstate m_CurrentState = Tstate.IDLE;

//    public List<Transform> m_WayPoints;


//    int m_CurrentPatrolId;

//    public Transform m_LifeBarPositions;

//    public int m_HelixHitColliderAmount = 10;
//    public int m_HeadHitColliderAmount = 50;
//    public int m_BodyHitColliderAmount = 20;

//    public int m_MaxLife = 50;
//    int m_Life;
//    float m_AlertTimer;
//    public float m_ChaseDistance;
//    public float m_AlertTime = 3.0f; //4.0f
//    public float m_AlertRotationSpeed = 120.0f;
//    public float m_ViewAngle = 80.0f; // 45.0f;
//    public float m_AlertDistance = 5.0f; //10.0f
//    public float m_MinDistance = 3.0f;
//    public float m_MaxDistance = 15.0f; //15.0f
//    public float m_ShootMaxDistance = 8.0f;

//    public float m_ShootInterval = 1.5f;
//    public int m_ShootDamage = 10;
//    float m_ShootTimer;
//    public Transform m_ShootOrigin;
//    public GameObject m_BulletPrefab;

//    float m_HitTimer;
//    public float m_HitTime = 0.5f;
//    float m_DieTimer;
//    public float m_DieTime = 2.0f;

//    Vector3 m_OriginalScale;

//    Transform m_Player;
//    Tstate m_PreviousState;
//    bool m_PlayerVisible;

//    void Start()
//    {
//        m_Life = m_MaxLife;
//        m_NavMeshAgent = GetComponent<NavMeshAgent>();
//        m_Player = GameController.GetGameController().GetPlayer().transform;
//        SetIdleState();
//        GameController.GetGameController().AddEnemy(this);
//        m_OriginalScale = transform.localScale;
//    }

//    private void Update()
//    {
//        switch (m_CurrentState)
//        {
//            case Tstate.IDLE:
//                UpdateIdleState();
//                break;
//            case Tstate.PATROL:
//                UpdatePatrolState();
//                break;
//            case Tstate.CHASE:
//                UpdateChaseState();
//                break;
//            case Tstate.ALERT:
//                UpdateAlertState();
//                break;
//            case Tstate.ATTACK:
//                UpdateAttackState();
//                break;
//            case Tstate.HIT:
//                UpdateHitState();
//                break;
//            case Tstate.DIE:
//                UpdateDieState();
//                break;
//        }
//    }
//    void SetIdleState()
//    {
//        m_CurrentState = Tstate.IDLE;
//    }
//    void SetPatrolState()
//    {
//        m_CurrentState = Tstate.PATROL;
//    }
//    void SetChaseState()
//    {
//        m_CurrentState = Tstate.CHASE;
//    }
//    void SetAlertState()
//    {
//        m_CurrentState = Tstate.ALERT;
//    }
//    void SetAttackState()
//    {
//        m_CurrentState = Tstate.ATTACK;
//    }
//    void SetHitState()
//    {
//        m_CurrentState = Tstate.HIT;
//    }
//    void SetDieState()
//    {
//        m_CurrentState = Tstate.DIE;
//    }

//    int GetClosestPatrolWaypoint()
//    {
//        int l_WaypointId = 0;
//        float l_MinDistance = 0.0f;
//        for (int i = 0; i < m_WayPoints.Count; i++)
//        {
//            float l_Distance = Vector3.Distance(transform.position, m_WayPoints[i].position);
//            if (i == 0 || l_Distance < l_MinDistance)
//            {
//                l_MinDistance = l_Distance;
//                l_WaypointId = i;
//            }

//        }
//        return l_WaypointId;
//    }

//    bool HasArrivedToWayPoint()
//    {
//        //return !m_NavMeshAgent.hasPath && m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
//        return !m_NavMeshAgent.pathPending /*&& m_NavMeshAgent.hasPath*/ && m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance;
//    }



//    void SetNextChsePosition()
//    {
//        Vector3 l_Direction = GameController.GetGameController().GetPlayer().transform.position - transform.position;
//        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
//        m_NavMeshAgent.destination = l_PlayerPosition - l_Direction * m_ChaseDistance;
//    }

//    void MoveToNextWaypoint()
//    {
//        m_CurrentPatrolId = (m_CurrentPatrolId + 1) % m_WayPoints.Count;
//        m_NavMeshAgent.destination = m_WayPoints[m_CurrentPatrolId].position;
//    }
//    void UpdateIdleState()
//    {
//        if (m_WayPoints.Count == 0)
//        {
//            return;
//        }
//        m_NavMeshAgent.isStopped = false;
//        m_CurrentPatrolId = GetClosestPatrolWaypoint();
//        m_NavMeshAgent.SetDestination(m_WayPoints[m_CurrentPatrolId].position);
//        SetPatrolState();
//    }
//    void UpdatePatrolState()
//    {
//        float l_Distance = Vector3.Distance(transform.position, m_Player.position);

//        if (l_Distance <= m_AlertDistance)
//        {
//            m_NavMeshAgent.isStopped = true;
//            m_AlertTimer = 0.0f;
//            SetAlertState();
//            return;
//        }
//        if (HasArrivedToWayPoint())
//        {
//            MoveToNextWaypoint();
//        }
//    }
//    void UpdateChaseState()
//    {
//        float l_Distance = Vector3.Distance(transform.position, m_Player.position);

//        if (l_Distance > m_MaxDistance)
//        {
//            if (m_WayPoints.Count == 0) return;
//            m_NavMeshAgent.isStopped = false;
//            SetPatrolState();
//            m_CurrentPatrolId = GetClosestPatrolWaypoint();
//            m_NavMeshAgent.destination = m_WayPoints[m_CurrentPatrolId].position;
//        }
//        else if (l_Distance <= m_MinDistance)
//        {
//            m_NavMeshAgent.isStopped = true;
//            SetAttackState();
//        }
//        else
//        {
//            m_NavMeshAgent.isStopped = false;
//            m_NavMeshAgent.SetDestination(m_Player.position);
//        }
//    }
//    void UpdateAlertState()
//    {
//        m_AlertTimer += Time.deltaTime;

//        // transform.Rotate(0.0f, m_AlertRotationSpeed * Time.deltaTime, 0.0f);

//        Vector3 l_Direction = m_Player.position - transform.position;

//        float l_Distance = l_Direction.magnitude;

//        float l_Angle = Vector3.Angle(transform.forward, l_Direction);

//        if (l_Distance <= m_MaxDistance && l_Angle <= m_ViewAngle)
//        {

//            Vector3 l_Origin = transform.position;//+ Vector3.up * 0.5f;
//            Vector3 l_Target = m_Player.position; //+ Vector3.up;
//            Vector3 l_RayDirection = (l_Target - l_Origin).normalized;
//            Debug.DrawRay(l_Origin, l_RayDirection * l_Distance, Color.red);
//            RaycastHit l_Hit;
//            Debug.Log("-Distancia: " + l_Distance + " | Angulo: " + l_Angle);
//            if (Physics.Raycast(l_Origin, l_RayDirection.normalized, out l_Hit, l_Distance + 1.0f))
//            {
//                Debug.Log("-Raycast impacta en: " + l_Hit.transform.name);
//                Debug.Log("-Objeto jugador: " + m_Player.name);
//                Debug.Log("- Es el jugador o un hijo?: ");
//                if (l_Hit.transform == m_Player || l_Hit.transform.IsChildOf(m_Player))
//                {
//                    Debug.Log("-JUGADOR DETECTADO, CAMBIANDO A CHASE");
//                    m_NavMeshAgent.isStopped = false;
//                    SetChaseState();
//                    return;
//                }
//            }
//            else
//            {
//                Debug.Log("-Raycast no ha impactado en ning n objeto");
//            }
//        }

//        if (m_AlertTimer >= m_AlertTime)
//        {
//            m_NavMeshAgent.isStopped = false;
//            SetPatrolState();
//        }
//        else
//        {
//            transform.Rotate(0.0f, m_AlertRotationSpeed * Time.deltaTime, 0.0f);
//        }
//    }
//    void UpdateAttackState()
//    {
//        float l_Distance = Vector3.Distance(transform.position, m_Player.position);

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
//    }

//    void ShootPlayer()
//    {
//        /*
//        Vector3 l_Origin = m_ShootOrigin != null ? m_ShootOrigin.position: transform.position;
//        RaycastHit l_Hit;
//        Debug.DrawRay(l_Origin, l_Direction * m_ShootMaxDistance, Color.red, 1.0f);

//        if (Physics.Raycast(l_Origin, l_Direction, out l_Hit, m_ShootMaxDistance))
//        {
//            if (l_Hit.transform == m_Player || l_Hit.transform.IsChildOf(m_Player))
//            {
//                FPSController l_PlayerController = m_Player.GetComponent<FPSController>();

//                if (l_PlayerController != null)
//                {
//                    l_PlayerController.TakeDamage(m_ShootDamage);
//                }
//            }
//        }
//        */
//        if (m_BulletPrefab == null || m_ShootOrigin == null) return;

//        Vector3 l_Direction = (m_Player.position - m_ShootOrigin.position).normalized;
//        Quaternion l_Rotation = Quaternion.LookRotation(l_Direction);

//        Instantiate(m_BulletPrefab, m_ShootOrigin.position, l_Rotation);
//    }

//    void UpdateHitState()
//    {
//        m_HitTimer += Time.deltaTime;

//        if (m_HitTimer >= m_HitTime)
//        {
//            if (m_PreviousState == Tstate.IDLE || m_PreviousState == Tstate.PATROL)
//            {
//                m_AlertTimer = 0.0f;
//                m_NavMeshAgent.isStopped = true;
//                SetAlertState();
//            }
//            else
//            {
//                m_CurrentState = m_PreviousState;

//                if (m_CurrentState == Tstate.CHASE)
//                {
//                    m_NavMeshAgent.isStopped = false;
//                }
//                else
//                {
//                    m_NavMeshAgent.isStopped = true;
//                }
//            }
//        }
//    }
//    void UpdateDieState()
//    {
//        m_DieTimer += Time.deltaTime;

//        float l_Progress = Mathf.Clamp01(m_DieTimer / m_DieTime);

//        transform.localScale = Vector3.Lerp(m_OriginalScale, Vector3.zero, l_Progress);
//        if (m_DieTimer >= m_DieTime)
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void Hit(HitColllider2.THitColliderType HitColliderType)
//    {
//        if (m_CurrentState == Tstate.DIE) return;

//        if (m_CurrentState != Tstate.HIT)
//        {
//            if (m_CurrentState == Tstate.IDLE || m_CurrentState == Tstate.PATROL)
//            {
//                m_PreviousState = Tstate.PATROL;
//            }
//            else
//            {
//                m_PreviousState = m_CurrentState;
//            }
//        }
//        int l_lifePoints = 0;
//        switch (HitColliderType)
//        {
//            case HitColllider2.THitColliderType.BODY:
//                l_lifePoints = m_BodyHitColliderAmount;
//                break;
//            case HitColllider2.THitColliderType.HEAD:
//                l_lifePoints = m_HeadHitColliderAmount;
//                break;
//            case HitColllider2.THitColliderType.HELIX:
//                l_lifePoints += m_HelixHitColliderAmount;
//                break;
//            default:
//                Debug.LogError("-HitColliderType" + HitColliderType + "not recognized");
//                break;
//        }

//        m_Life = m_Life - l_lifePoints;
//        if (m_Life <= 0)
//        {
//            m_Life = 0;
//            m_DieTimer = 0.0f;
//            m_NavMeshAgent.isStopped = true;
//            SetDieState();
//        }
//        else
//        {
//            m_HitTimer = 0.0f;
//            m_NavMeshAgent.isStopped = true;
//            SetHitState();
//        }

//    }

//    public int GetLife()
//    {
//        return m_Life;
//    }
//}
