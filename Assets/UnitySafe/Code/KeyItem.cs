using UnityEngine;

public class KeyItem : Item
{
    public static bool hasKey = false;

    public override bool CanPick()
    {
        return true;
    }

    public override void Pick()
    {
        hasKey = true;
        base.Pick();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CanPick())
        {
            Pick();
        }
    }
}