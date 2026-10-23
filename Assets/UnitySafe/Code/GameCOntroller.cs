using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public Transform m_DestroyObjects;
    public static GameController m_GameController=null;
    public KeyCode m_RestartKeyCode = KeyCode.R;
    FPSController m_Player;
    InGameUI m_IngameUI;
    List<DroneEnemy> m_EnemyList = new List<DroneEnemy>();
    DroneEnemy m_Enemies;
    Fade m_Fade;


    private void Awake()
    {
        if(m_GameController  == null)
        {
            m_GameController=this;
            DontDestroyOnLoad(gameObject);
        }
        else 
            GameObject.Destroy(gameObject);   
    }
    static public GameController GetGameController() 
    {
        return m_GameController;
    }
    public void SetIngameUI(InGameUI _IngameUI)
    {
        m_IngameUI = _IngameUI;
    }
    public InGameUI GetIngameUI()
    {
        return m_IngameUI;
    }
    public void AddEnemy(DroneEnemy Enemy)
    {
        m_EnemyList.Add(Enemy);
    }
    public List<DroneEnemy> GetEnemies()
    {
        return m_EnemyList;
    }
    
    public void Restart()
    {
        for (int i = 0; i < m_DestroyObjects.childCount; ++i)
            GameObject.Destroy(m_DestroyObjects.GetChild(i).gameObject);
        m_Player.Restart();
    }
    private void Update()
    {
        if (Input.GetKeyDown(m_RestartKeyCode)) Restart();
    }
    public FPSController GetPlayer()
    {
        return m_Player;
    }
    public void SetPlayer(FPSController Player)
    {
        m_Player= Player;
    }
}
