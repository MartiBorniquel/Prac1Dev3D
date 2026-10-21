using System.Net.NetworkInformation;
using UnityEditorInternal;
using UnityEngine;

public class DroneEnemy : MonoBehaviour
{
    enum TState
    {
        IDLE,
        ALERT,
        PATROL,
        ATTACK,
        CHASE,
        HIT,
        DIE,
    }

    TState m_CurrentState = TState.IDLE;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetIdleState();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_CurrentState)
        {
            case TState.IDLE:
                UpdateIdleState();
                break;
            case TState.ALERT:
                UpdateAlertState();
                break;
            case TState.PATROL:
                UpdatePatrolState();
                break;
            case TState.ATTACK:
                UpdateAttackState();
                break;
            case TState.CHASE:
                UpdateChaseState();
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
    void SetPatrolState()
    {
        m_CurrentState = TState.PATROL;
        m_CurrentPatrolId = GetClosestPatrolWaypoint();
        m_NavMeshAgent.destination = m_Waypoints[m_CurrentPatrolId].position;
    }
    void MoveToNextWaypoint()
    {
        ++m_currentPatrolId;
        if (m_CurrentPatrolId == m_Waypoints.Count)
            m_CurrentPatrolId = 0;
        m_CurrentPatrolId=(m_CurrentPatrolId+1)
    }
    int GetClosestPatrolWaypoint()
    {
        int l_WaypointId = 0;
        for (int i = 0; i < l_WaypointId; i++) { float l_Distance = Vector3(transform.position, m_Waypoints[i].position).sqrMagnitude;
            if (i == 0 || l_Distance<MaxDistance) { l_MaxDistance = l_Distance l_WaypointId = i;)}

    void SetChaseState()
    {
        m_CurrentState = TState.CHASE;
    }

    void SetAlertState()
    {
        m_CurrentState = TState.ALERT;
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

    }

    void UpdatePatrolState()
    {

    }

    void UpdatepatrolState()
    {

    }

    void UpdateChaseState()
    {

    }

    void UpdateAlertState()
    {

    }

    void UpdateAttackState()
    {

    }

    void UpdateHitState()
    {

    }

    void UpdateDieState()
    {

    }

}