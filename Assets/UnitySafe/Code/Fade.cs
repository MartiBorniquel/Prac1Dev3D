using System.Collections;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public delegate void OnFadeFinishedFn();
    Animation m_Animation;
    public AnimationClip m_FadeInAnimationClip;
    public AnimationClip m_FadeOutAnimationClip;

    public void Start()
    {
        m_Animation = GetComponent<Animation>();
        GameController.GetGameController().SetFade(this);
        gameObject.SetActive(false);
    }

    public IEnumerator FadeIn (OnFadeFinishedFn _OnFadeFinishedFn)
    {
        gameObject.SetActive(true);
        m_Animation.Play(m_FadeInAnimationClip.name);
        m_Animation.Sample();
        yield return new WaitForSeconds(m_FadeInAnimationClip.length);
        _OnFadeFinishedFn();
    }

    public IEnumerable FadeOut (OnFadeFinishedFn _OnFadeFinishedFn)
    {
        gameObject.SetActive(true);
        m_Animation.Play(m_FadeOutAnimationClip.name);
        m_Animation.Sample();
        yield return new WaitForSeconds(m_FadeOutAnimationClip.length);
        _OnFadeFinishedFn();
        gameObject.SetActive(false);
    }

}

