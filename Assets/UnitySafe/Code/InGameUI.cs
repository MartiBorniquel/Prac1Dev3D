using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public LifeBarElementUI m_lifeBarElementUI;
    private void Start()
    {
        GameController.GetGameController().SetIngameUI(this);
    }
}
