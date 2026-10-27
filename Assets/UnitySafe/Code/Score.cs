
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TMP_Text m_textScore;
    public int m_score;
    public GameObject m_ScoreUI;
    public KeyCode Restart = KeyCode.P;
    FPSController m_player;

    void Start()
    {
        m_player = FindObjectOfType<FPSController>();
        if (m_ScoreUI != null)
        {
            m_textScore = m_ScoreUI.GetComponentInChildren<TMP_Text>(true);
            HideScoreUI();
        }
        
    }

    void Update()
    {
        if (m_textScore != null)
        {
            if (m_score < 50)
            {
                m_textScore.text = "Score: " + m_score + "/50\nExit the shooting range or press P to restart the challenge";
            }
            else
            {
                m_textScore.text = "Congratulations!\nYou can open the door now to access the next area!\nPress P to restart challenge";
            }
        }

        if (Input.GetKeyDown(Restart))
        {
            ResetScore();
            FindObjectOfType<ShootingRangeManager>().RestartChallenge();
        }
    }

    public void SumScore(int EnteringScore)
    {
        if(m_score < 50)
            m_score += EnteringScore;
    }

    public void ResetScore()
    {
        m_score = 0;
        m_player.m_Score = 0;
    }

    public void HideScoreUI()
    {
        if (m_ScoreUI != null)
            m_ScoreUI.SetActive(false);
    }

    public void ShowScoreUI()
    {
        if (m_ScoreUI != null)
            m_ScoreUI.SetActive(true);
    }
}
