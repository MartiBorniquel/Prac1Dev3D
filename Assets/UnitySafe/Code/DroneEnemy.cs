using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class DroneEnemy : MonoBehaviour
{
    enum TState
    {
        IDLE = 0,
        PATROL = 1,
        ALERT = 2,
        CHASE = 3,
        ATTACK = 4,
        HIT = 5,
        DIE = 6
    }

    NavMeshAgent m_NavMeshAgent;
    TState m_CurrentState = TState.IDLE;
    public List<Transform> m_Waypoints;
    int m_CurrentPatrolId;
    public float m_ChaseDistance = 5;
    public int m_MaxHealth = 50;
    public int m_Health;
    public int m_BodyHit = 20;
    public int m_HeadHit = 50;
    public int m_WingsHit = 10;
    public Transform m_LifeBarPosition;

    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        SetIdleState();
        m_Health = m_MaxHealth;
        GameController.GetGameController().AddEnemy(this);
    }

    private void Update()
    {

        switch (m_CurrentState)
        {
            case TState.IDLE:
                UpdateIdleState();
                break;
            case TState.PATROL:
                UpdatePatrolState();
                break;
            case TState.ALERT:
                //UpdateAlertState();
                break;
            case TState.CHASE:
                UpdateChaseState();
                break;
            case TState.ATTACK:
                UpdateAttackState();
                break;
            case TState.HIT:
                UpdateHitState();
                break;
            case TState.DIE:
                UpdateDieState();
                break;
        }

        
    }

    void SetIdleState()
    {
        m_CurrentState = TState.IDLE;
    }
    public void GetHealth()
    {

    }
    void SetPatrolState()
    {
        m_CurrentState = TState.PATROL;
        m_CurrentPatrolId = GetClosestPatrolWaypoint();
        m_NavMeshAgent.destination = m_Waypoints[m_CurrentPatrolId].position;
    }

    int GetClosestPatrolWaypoint()
    {
        int l_WaypointId = 0;
        float l_ClosestDistance = float.MaxValue /* 0.0f*/;
        for (int i = 0; i < m_Waypoints.Count; ++i)
        {
            float l_Distance = Vector3.Distance(transform.position, m_Waypoints[i].position);
            if (/* i == 0 || */ l_Distance < l_ClosestDistance)
            {
                l_ClosestDistance = l_Distance;
                l_WaypointId = i;
            }
        }
        return l_WaypointId;
    }

    void SetAlertState()
    {
        m_CurrentState = TState.ALERT;
    }
    void SetChaseState()
    {
        m_CurrentState = TState.CHASE;
    }
    void SetAttackState()
    {
        m_CurrentState = TState.ATTACK;
    }
    void SetHitState()
    {
        m_CurrentState = TState.HIT;
    }
    void SetDieState()
    {
        m_CurrentState = TState.DIE;
    }

    void UpdateIdleState()
    {
        SetPatrolState();
    }

    void UpdatePatrolState()
    {
        if (HasArrivedToWaypoint())
            MoveToNextPatrolPosition();
    }

    void UpdateChaseState()
    {
        SetNextChasePosition();
    }

    void UpdateAttackState()
    {
        // Attack logic here
    }

    void UpdateHitState()
    {
        // Hit logic here
    }

    void UpdateDieState()
    {
        // Die logic here
    }

    bool HasArrivedToWaypoint()
    {
        return !m_NavMeshAgent.pathPending && m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance;
        // return !m_NavNeshAgent.hasPath 
    }
    void MoveToNextPatrolPosition()
    {
        m_CurrentPatrolId = (m_CurrentPatrolId + 1) % m_Waypoints.Count;
        m_NavMeshAgent.destination = m_Waypoints[m_CurrentPatrolId].position;
    }

    void SetNextChasePosition()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        Vector3 l_Direction = (l_PlayerPosition - transform.position).normalized;
        m_NavMeshAgent.destination = transform.position + l_Direction * m_ChaseDistance;
    }
    public void Hit(HitCollider.HitColliderType HitColliderType )
    {
        int l_LifePoints = 0;
        switch (HitColliderType)
        {
            case HitCollider.HitColliderType.BODY:
                l_LifePoints -= m_BodyHit;
                break;
            case HitCollider.HitColliderType.HEAD:
                l_LifePoints -= m_HeadHit;
                break;
            case HitCollider.HitColliderType.WINGS:
                l_LifePoints -= m_WingsHit;
                break;
            default:
                Debug.Log("HitCollider not registered");
                break;
                
         
        }
        m_Health+= l_LifePoints;
        if (m_Health <= 0)
        {
            SetDieState();

        }
        else
            SetHitState();
        Debug.Log("Current health" + m_Health);
    }
    public int GetLife()
    {
        return m_Health;
    }
}
