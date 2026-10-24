
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TMP_Text textScore;
    public int score;
    public GameObject ScoreUI;
    public KeyCode Restart = KeyCode.P;

    void Start()
    {
        if (ScoreUI != null)
        {
            textScore = ScoreUI.GetComponentInChildren<TMP_Text>(true);
            HideScoreUI();
        }
        
    }

    void Update()
    {
        if (textScore != null&&score<50)
        {
            textScore.text = "Score: " + score + "/50\nExit the shooting range or press P to restart the challenge";
        }
        else
        {
            textScore.text = "Congratulations!\nYou can open the door now to acces the next area!\n Press P to restart challenge";
        }
        if (Input.GetKeyDown(Restart))
        {
            score = 0;
        }
            

    }

    public void SumScore(int EnteringScore)
    {
        if(score<50)
        score += EnteringScore;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void HideScoreUI()
    {
        if (ScoreUI != null)
            ScoreUI.SetActive(false);
    }

    public void ShowScoreUI()
    {
        if (ScoreUI != null)
            ScoreUI.SetActive(true);
    }
}
