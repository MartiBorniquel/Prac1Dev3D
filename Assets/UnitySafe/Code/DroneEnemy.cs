using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class DroneEnemy : MonoBehaviour
{
    enum TState
    {
        IDLE = 0,
        PATROL,
        ALERT,
        CHASE,
        ATTACK,
        HIT,
        DIE
    }

    NavMeshAgent m_NavMeshAgent;
    TState m_CurrentState = TState.IDLE;
    public List<Transform> m_Waypoints;
    int m_CurrentPatrolId;
    public float m_ChaseDistance = 5;

    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        SetIdleState();
    }

    private void Update()
    {
        switch (m_CurrentState)
        {
            case TState.IDLE:
                break;
            case TState.PATROL:
                break;
            case TState.ALERT:
                break;
            case TState.CHASE:
                break;
            case TState.ATTACK:
                break;
            case TState.HIT:
                break;
            case TState.DIE:
                break;
        }
    }

    void SetIdleState()
    {
        m_CurrentState = TState.IDLE;
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
}
