using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform m_DestroyObjects;
    public static GameController m_GameController=null;
    public KeyCode m_RestartKeyCode = KeyCode.R;
    FPSController m_Player;
    InGameUI _InGameUI;
    public List<DroneEnemy> = EnemyList;


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

    public void SetIngameUI(InGameUI _InGameUI)
    {

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

    public void AddEnemy(DroneEnemy enemy)
    {

    }
}
