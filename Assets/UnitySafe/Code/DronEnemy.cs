using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

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
    TState m_PreviousState = TState.IDLE;

    public List<Transform> m_Waypoints;
    int m_CurrentPatrolId;

    [Header("Drop")]
    public Item Medkit;                       // prefab del medkit
    public Item Ammo;                         // prefab de la municion
    [Range(0f, 1f)] public float m_DropChance = 1.0f;   // 1 = siempre suelta algo
    [Range(0f, 1f)] public float m_MedkitChance = 0.5f; // 0.5 = 50% medkit / 50% ammo

    public int m_MaxHealth = 50;
    public int m_Health;
    public int m_BodyHit = 20;
    public int m_HeadHit = 50;
    public int m_WingsHit = 10;
    public Transform m_LifeBarPosition;

    public float m_IdleTime = 2;
    public float m_HitDuration = 0.5f;
    public float m_DieFadeTime = 2f;
    float m_IdleTimer;
    float m_HitTimer;

    public float m_HearDistance = 5f;
    public float m_AlertDuration = 3f;
    public float m_AlertRotationSpeed = 120f;
    float m_AlertTimer;

    [Header("Cone mesures")]
    public float ConeAngle = 160f;
    public float ConeRange = 10f;

    public float m_MinDistanceToChase = 10f;
    public float m_MaxDistanceToChase = 15f;

    public float m_ShootMaxDistance = 12f;
    public float m_ShootInterval = 1.5f;
    public float m_FaceSpeed = 6f;
    public Transform m_ShootOrigin;
    public GameObject m_BulletPrefab;
    float m_ShootTimer;

    public ParticleSystem AlertParticle;

    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        m_Health = m_MaxHealth;
        GameController.GetGameController().AddEnemy(this);
        SetIdleState();
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
                UpdateAlertState();
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
        m_IdleTimer = 0.0f;
        m_NavMeshAgent.isStopped = true;
    }

    void SetPatrolState()
    {
        m_CurrentState = TState.PATROL;
        m_NavMeshAgent.isStopped = false;
        m_CurrentPatrolId = GetClosestPatrolWaypoint();
        m_NavMeshAgent.destination = m_Waypoints[m_CurrentPatrolId].position;
    }

    void SetAlertState()
    {
        m_CurrentState = TState.ALERT;
        m_AlertTimer = 0.0f;
        m_NavMeshAgent.isStopped = true;
        if (AlertParticle != null) AlertParticle.Play();
    }

    void SetChaseState()
    {
        m_CurrentState = TState.CHASE;
        m_NavMeshAgent.isStopped = false;
    }

    void SetAttackState()
    {
        m_CurrentState = TState.ATTACK;
        m_ShootTimer = 0.0f;
        m_NavMeshAgent.isStopped = true;
    }

    void SetHitState()
    {
        m_CurrentState = TState.HIT;
        m_HitTimer = 0.0f;
        m_NavMeshAgent.isStopped = true;
    }

    void SetDieState()
    {
        m_CurrentState = TState.DIE;
        m_NavMeshAgent.isStopped = true;
        m_NavMeshAgent.enabled = false;

        foreach (Collider l_Collider in GetComponentsInChildren<Collider>())
            l_Collider.enabled = false;

        DropItem();   // una sola vez, al entrar en DIE
        StartCoroutine(FadeAndDestroy());
    }

    void UpdateIdleState()
    {
        m_IdleTimer += Time.deltaTime;
        if (m_IdleTimer >= m_IdleTime)
            SetPatrolState();
    }

    void UpdatePatrolState()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        if (Vector3.Distance(transform.position, l_PlayerPosition) <= m_HearDistance)
        {
            SetAlertState();
            return;
        }

        if (HasArrivedToWaypoint())
            MoveToNextPatrolPosition();
    }

    void UpdateAlertState()
    {
        m_AlertTimer += Time.deltaTime;
        transform.Rotate(0.0f, m_AlertRotationSpeed * Time.deltaTime, 0.0f);

        if (ConoVisual())
            SetChaseState();
        else if (m_AlertTimer >= m_AlertDuration)
            SetPatrolState();
    }

    void UpdateChaseState()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        float l_Distance = Vector3.Distance(transform.position, l_PlayerPosition);

        if (l_Distance > m_MaxDistanceToChase)
        {
            SetPatrolState();
        }
        else if (l_Distance <= m_MinDistanceToChase)
        {
            SetAttackState();
        }
        else
        {
            m_NavMeshAgent.isStopped = false;
            m_NavMeshAgent.SetDestination(l_PlayerPosition);
        }
    }

    void UpdateAttackState()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        float l_Distance = Vector3.Distance(transform.position, l_PlayerPosition);

        if (l_Distance > m_ShootMaxDistance)
        {
            SetChaseState();
            return;
        }

        Vector3 l_Direction = l_PlayerPosition - transform.position;
        if (l_Direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(l_Direction),
                m_FaceSpeed * Time.deltaTime);
        }

        m_ShootTimer += Time.deltaTime;
        if (m_ShootTimer >= m_ShootInterval)
        {
            ShootPlayer();
            m_ShootTimer = 0.0f;
        }
    }

    void UpdateHitState()
    {
        m_HitTimer += Time.deltaTime;
        if (m_HitTimer < m_HitDuration) return;

        // Si estaba en PATROL o IDLE -> ALERT. En cualquier otro caso, vuelve al estado anterior.
        if (m_PreviousState == TState.PATROL || m_PreviousState == TState.IDLE)
        {
            SetAlertState();
        }
        else
        {
            m_CurrentState = m_PreviousState;
            m_NavMeshAgent.isStopped = (m_PreviousState == TState.ATTACK || m_PreviousState == TState.ALERT);
        }
    }

    void UpdateDieState()
    {
        // El fade y la destruccion los hace la corrutina FadeAndDestroy
    }

    int GetClosestPatrolWaypoint()
    {
        int l_WaypointId = 0;
        float l_ClosestDistance = float.MaxValue;
        for (int i = 0; i < m_Waypoints.Count; ++i)
        {
            float l_Distance = Vector3.Distance(transform.position, m_Waypoints[i].position);
            if (l_Distance < l_ClosestDistance)
            {
                l_ClosestDistance = l_Distance;
                l_WaypointId = i;
            }
        }
        return l_WaypointId;
    }

    bool HasArrivedToWaypoint()
    {
        return !m_NavMeshAgent.pathPending && m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance;
    }

    void MoveToNextPatrolPosition()
    {
        m_CurrentPatrolId = (m_CurrentPatrolId + 1) % m_Waypoints.Count;
        m_NavMeshAgent.destination = m_Waypoints[m_CurrentPatrolId].position;
    }

    public void Hit(HitCollider.HitColliderType HitColliderType)
    {
        if (m_CurrentState == TState.DIE) return;

        int l_Damage = 0;
        switch (HitColliderType)
        {
            case HitCollider.HitColliderType.BODY:
                l_Damage = m_BodyHit;
                break;
            case HitCollider.HitColliderType.HEAD:
                l_Damage = m_HeadHit;
                break;
            case HitCollider.HitColliderType.WINGS:
                l_Damage = m_WingsHit;
                break;
            default:
                Debug.Log("HitCollider not registered");
                break;
        }

        m_Health -= l_Damage;

        if (m_Health <= 0)
        {
            SetDieState();
            return;
        }

        if (m_CurrentState != TState.HIT)
            m_PreviousState = m_CurrentState;

        SetHitState();
    }

    public int GetLife()
    {
        return m_Health;
    }

    IEnumerator FadeAndDestroy()
    {
        Renderer[] l_Renderers = GetComponentsInChildren<Renderer>();
        float l_Time = 0.0f;

        while (l_Time < m_DieFadeTime)
        {
            l_Time += Time.deltaTime;
            float l_Alpha = 1.0f - Mathf.Clamp01(l_Time / m_DieFadeTime);

            foreach (Renderer l_Renderer in l_Renderers)
            {
                foreach (Material l_Material in l_Renderer.materials)
                {
                    string l_Property = l_Material.HasProperty("_BaseColor") ? "_BaseColor" : "_Color";
                    if (!l_Material.HasProperty(l_Property)) continue;

                    Color l_Color = l_Material.GetColor(l_Property);
                    l_Color.a = l_Alpha;
                    l_Material.SetColor(l_Property, l_Color);
                }
            }
            yield return null;
        }

        Destroy(gameObject);
    }

    public bool ConoVisual()
    {
        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        Vector3 l_Direction = l_PlayerPosition - transform.position;

        return l_Direction.magnitude < ConeRange &&
               Vector3.Angle(transform.forward, l_Direction) < ConeAngle * 0.5f;
    }

    public void ShootPlayer()
    {
        if (m_BulletPrefab == null || m_ShootOrigin == null) return;

        Vector3 l_PlayerPosition = GameController.GetGameController().GetPlayer().transform.position;
        Vector3 l_Direction = (l_PlayerPosition - m_ShootOrigin.position).normalized;
        Quaternion l_Rotation = Quaternion.LookRotation(l_Direction);

        Instantiate(m_BulletPrefab, m_ShootOrigin.position, l_Rotation);
    }

    public void DropItem()
    {
        Item l_Prefab;

        if(Random.value < m_MedkitChance) { l_Prefab=Medkit ; } else { l_Prefab=Ammo ; }

            Instantiate(l_Prefab, transform.position, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        float l_HalfAngle = ConeAngle * 0.5f;

        Vector3 l_Origin = transform.position;
        Vector3 l_Left = Quaternion.AngleAxis(-l_HalfAngle, transform.up) * transform.forward;
        Vector3 l_Right = Quaternion.AngleAxis(l_HalfAngle, transform.up) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(l_Origin, l_Origin + l_Left * ConeRange);
        Gizmos.DrawLine(l_Origin, l_Origin + l_Right * ConeRange);
        Gizmos.DrawLine(l_Origin + l_Left * ConeRange, l_Origin + l_Right * ConeRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(l_Origin, transform.forward * ConeRange);
    }
}