using System.Collections;
using UnityEngine;
using UnityEngine.WSA;

public class ShootingRangeManager : MonoBehaviour
{
    public ShootingTarget[] targets;

    public float activeTime = 3f;
    public float delayBetweenTargets = 0.5f;
    bool inside=false;
    public int maxScore = 50;
    FPSController m_Player;
    Score score;

    private void Awake()
    {
        score = FindObjectOfType<Score>();
    }
    void Start()
    {
        m_Player = FindObjectOfType<FPSController>();
        
    }

    IEnumerator ShootingSequence()
    {
        while (inside&& m_Player.m_Score<maxScore)
        {
            foreach (ShootingTarget target in targets)
            {
                target.ActivateTarget();

                yield return new WaitForSeconds(activeTime);

                if (target.isUp)
                {
                    target.DeactivateTarget();
                }

                yield return new WaitForSeconds(delayBetweenTargets);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = true;
            score.ShowScoreUI();
            StartCoroutine(ShootingSequence());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;
            score.HideScoreUI();
            if (score.score < 50)
            {
                score.ResetScore();
            }
            
            StopAllCoroutines();
        }
    }
}
