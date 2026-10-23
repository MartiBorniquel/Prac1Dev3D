using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class FPSController : MonoBehaviour
{
    public Transform m_PitchController;

    float m_Yaw;
    float m_Pitch;
    public float m_YawSpeed;
    public float m_PitchSpeed;
    public bool m_InvertedYaw;
    public bool m_InvertedPitch;
    public float m_MinPitch;
    public float m_MaxPitch;
    public float m_Speed;
    CharacterController m_CharacterController;
    float m_VerticalSpeed = 0;
    public float m_JumpSpeed = 0;
    public float m_SprintSpeed = 0;
    bool angleLocked = false;
    bool cursorLocked = false;
    [Header("Shoot")]
    public Camera m_Camera;
    public float m_MaxShootDistance = 200.0f;
    public LayerMask m_ShootLayerMask;
    public GameObject m_HitParticlePrefab;

    [Header("Input")]
    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_SprintKeyCode = KeyCode.LeftShift;
    public KeyCode m_LockAngle = KeyCode.I;
    public KeyCode m_DebugLockCursorKeyCode = KeyCode.O;
    public KeyCode m_ReloadKey = KeyCode.R;

    public Animation m_Animation;
    public AnimationClip m_IdleAnimationClip;
    public AnimationClip m_ShootAnimationClip;
    public AnimationClip m_ReloadAnimationClip;

    Vector3 m_StartPosition;
    Quaternion m_StartRotation;

    int m_ShootButton = 0;

    public int m_Score = 0;
    public int m_MaxBullets = 30;
    public int m_CurrentBullets;
    bool reloading=false;
    public int m_DelayReload;
    public int m_InventoryBullets=90;
    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        SetIdleWeaponAnim();
        m_Yaw = transform.rotation.eulerAngles.y;
        m_Pitch = m_PitchController.eulerAngles.x;
        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;
        GameController.GetGameController().SetPlayer(this);
        m_CurrentBullets = m_MaxBullets;
    }

    private void Update()
    {

        if (Input.GetKeyDown(m_LockAngle))
            angleLocked = !angleLocked;
        if (Input.GetKeyDown(m_DebugLockCursorKeyCode))
        {
            Cursor.lockState = Cursor.lockState != CursorLockMode.Locked ? CursorLockMode.Locked : CursorLockMode.None;
        }


        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");

        if (m_InvertedYaw)
            l_MouseX = -l_MouseX;
        if (m_InvertedPitch)
            l_MouseY = -l_MouseY;

        m_Yaw = m_Yaw + l_MouseX * m_YawSpeed * Time.deltaTime;
        m_Pitch = m_Pitch + l_MouseY * m_PitchSpeed * Time.deltaTime;

        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);

        Vector3 l_Direction = Vector3.zero;
        Vector3 l_Right = Vector3.right;
        Vector3 l_Forward = Vector3.forward;
        l_Right.Normalize();
        l_Forward.Normalize();

        if (Input.GetKey(m_RightKeyCode))
            l_Direction += transform.right;
        else if (Input.GetKey(m_LeftKeyCode))
            l_Direction -= transform.right;

        if (Input.GetKey(m_UpKeyCode))
            l_Direction += transform.forward;
        else if (Input.GetKey(m_DownKeyCode))
            l_Direction -= transform.forward;

        float l_Speed = m_Speed;
        if (Input.GetKey(m_SprintKeyCode) && m_CharacterController.isGrounded)
            l_Speed = m_SprintSpeed;
        if (Input.GetKey(m_JumpKeyCode) && m_CharacterController.isGrounded)
            m_VerticalSpeed = m_JumpSpeed;

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;

        //if (Input.GetKey(m_RightKeyCode))
        //    l_Direction = l_Right;
        //else if (Input.GetKey(m_LeftKeyCode))
        //    l_Direction = -l_Right;

        //if (Input.GetKey(m_UpKeyCode))
        //    l_Direction += l_Forward;
        //else if (Input.GetKey(m_DownKeyCode))
        //    l_Direction -= l_Forward;

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;

        l_Direction.Normalize();
        //transform.position = transform.position + l_Direction * m_Speed * Time.deltaTime;
        l_Direction = l_Direction * l_Speed * Time.deltaTime;
        l_Direction.y = m_VerticalSpeed * Time.deltaTime;
        CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_Direction);
        if ((l_CollisionFlags & CollisionFlags.CollidedBelow) != 0)
            m_VerticalSpeed = 0.0f;
        else if ((l_CollisionFlags & CollisionFlags.CollidedBelow) != 0 && m_VerticalSpeed > 0.0f)
            m_VerticalSpeed = 0.0f;

        if (CanShoot() && MustShoot() && !reloading)
            Shoot();
        else if (!CanShoot() && MustShoot()&&!reloading) StartCoroutine(Reload());
        if (CanReload() && MustReload()&&!reloading)
            StartCoroutine(Reload());
    }


    bool CanShoot()
    {
        if(m_CurrentBullets<=0) { return false; }
        return true;
    }

    bool MustShoot()
    {
        return Input.GetMouseButtonDown(m_ShootButton);
    }
    IEnumerator Reload()
    {
        reloading = true; 
        SetReloadWeaponAnim();
        yield return new WaitForSeconds(m_DelayReload);
        if (m_CurrentBullets <= 0)
        {
            m_InventoryBullets -= m_MaxBullets;
        }
        else
        {
               m_InventoryBullets -= (m_MaxBullets - m_CurrentBullets);
        }
        m_CurrentBullets = m_MaxBullets;
        Debug.Log("Inventory bullets" + m_InventoryBullets);


        reloading = false;
        Debug.Log("Reloaded: bullets " + m_CurrentBullets);
    }
    bool MustReload()
    {
        return Input.GetKeyDown(m_ReloadKey);
    }
    bool CanReload()
    {
        if (m_InventoryBullets <= 0) { Debug.Log("Insufficient ammo" + m_InventoryBullets); return false; }

        else { return true; }
    }
    void Shoot()
    {
        m_CurrentBullets -= 1;
        Ray l_Ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_MaxShootDistance, m_ShootLayerMask.value))
        {
            if (l_RaycastHit.collider.CompareTag("HitCollider"))
            {
                l_RaycastHit.collider.GetComponent<HitCollider>().Hit();
            }
            else
            { CreateShootHitParticles(l_RaycastHit.point, l_RaycastHit.normal);}
                



                
            SetShootWeaponAnim();
            Debug.Log("Current bullets:" + m_CurrentBullets);
        }

    }

    void CreateShootHitParticles(Vector3 position, Vector3 normal)
    {
        //GameObject l_GameObject = GameObject.Instantiate(m_HitParticlePrefab, GameController.GetGameController().m_DestroyObjects);
        //l_GameObject.transform.position = position;
        //l_GameObject.transform.rotation = Quaternion.LookRotation(normal);
        GameObject bullet = ObjectPooling.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = position;
            bullet.transform.rotation = Quaternion.LookRotation(normal);
            bullet.SetActive(true);
        }
    }
    void SetIdleWeaponAnim()
    {
        m_Animation.CrossFade(m_IdleAnimationClip.name, 0.1f);
    }
    void SetShootWeaponAnim()
    {
        m_Animation.CrossFade(m_ShootAnimationClip.name, 0.1f);
        m_Animation.CrossFadeQueued(m_IdleAnimationClip.name, 0.3f);
    }
    void SetReloadWeaponAnim()
    {
        m_Animation.CrossFade(m_ReloadAnimationClip.name, 0.1f);
        m_Animation.CrossFadeQueued(m_IdleAnimationClip.name, 0.3f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item l_Item = other.GetComponent<Item>();
            if (l_Item.CanPick())
            {
                l_Item.CanPick();
            }
        }
        else if (other.CompareTag("DeadZone")) { Kill(); }
    }
    IEnumerator Kill()
    {
        yield return  new WaitForSeconds(1.5f);
        GameController.GetGameController().Restart();

    }
    public void Restart()
    {
        m_CharacterController.enabled = false;
        transform.position = m_StartPosition;
        transform.rotation = m_StartRotation;
        m_CharacterController.enabled = true;
    }
}