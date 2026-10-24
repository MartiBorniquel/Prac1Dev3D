using System.Collections;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public delegate void OnFadeFinishedFn();
    Animation m_animation;
    public AnimationClip m_fadeInClip;
    public AnimationClip m_faeOutClip;
    void Start()
    {
        m_animation = GetComponent<Animation>();
        GameController.GetGameController().SetFade(this);
        gameObject.SetActive(false);
    }

    public IEnumerator FadeIn(OnFadeFinishedFn _OnFadeFinishedFn)
    {
        gameObject.SetActive(true);
        m_animation.Play(m_fadeInClip.name);
        m_animation.Sample();
        yield return new WaitForSeconds(m_fadeInClip.length);
        _OnFadeFinishedFn();
    }
    public IEnumerator FadeOut(OnFadeFinishedFn onFadeFinishedFn = null)
    {
        gameObject.SetActive(true);
        m_animation.Play(m_faeOutClip.name);
        m_animation.Sample();
        yield return new WaitForSeconds(m_faeOutClip.length);
        if (onFadeFinishedFn!= null)
        {
            onFadeFinishedFn();
            gameObject.SetActive(false);
        }
    }
    void Update()
    {
        
    }
}
