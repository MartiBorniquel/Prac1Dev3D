using System.Collections;
using UnityEngine;
using UnityEngine.WSA;

public class ShootingRangeManager : MonoBehaviour
{
    public ShootingTarget[] targets;

    public float m_activeTime = 3f;
    public float m_delayBetweenTargets = 0.5f;
    bool m_inside=false;
    public int m_maxScore = 50;
    FPSController m_Player;
    Score m_score;

    private void Awake()
    {
        m_score = FindObjectOfType<Score>();
    }
    void Start()
    {
        m_Player = FindObjectOfType<FPSController>();
        
    }

    IEnumerator ShootingSequence()
    {
        while (m_inside&& m_Player.m_Score<m_maxScore)
        {
            foreach (ShootingTarget target in targets)
            {
                target.ActivateTarget();

                yield return new WaitForSeconds(m_activeTime);

                if (target.isUp)
                {
                    target.DeactivateTarget();
                }

                yield return new WaitForSeconds(m_delayBetweenTargets);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_inside = true;
            m_score.ShowScoreUI();
            StartCoroutine(ShootingSequence());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_inside = false;
            m_score.HideScoreUI();
            if (m_score.m_score < 50)
            {
                m_score.ResetScore();
            }
            
            StopAllCoroutines();
        }
    }
    public void RestartChallenge()
    {
        StopAllCoroutines();

        foreach (ShootingTarget target in targets)
        {
            if (target.isUp)
            {
                target.DeactivateTarget();
            }
        }

        if (m_inside)
        {
            StartCoroutine(ShootingSequence());
        }
    }
}
