
using UnityEngine;

public class ShootingTarget : MonoBehaviour
{
    public Animation animationTarget;
    public AnimationClip targetUp;
    public AnimationClip targetDown;

    public bool isUp = false;
    public int pointsPerHit = 10;

    public void ActivateTarget()
    {
        animationTarget.Play(targetUp.name);
        isUp = true;
    }

    public void DeactivateTarget()
    {
        animationTarget.Play(targetDown.name);
        isUp = false;
    }

    public bool HitTarget()
    {
        if (isUp)
        {
            DeactivateTarget();
            return true;
        }

        return false;
    }
}
