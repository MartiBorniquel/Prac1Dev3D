using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckpointZone : MonoBehaviour
{
    [Tooltip("Opcional: punto donde reaparece el player. Si está vacío, usa la posición de esta zona.")]
    public Transform m_RespawnPoint;
    public bool m_OnlyOnce = true;

    bool m_Activated = false;

    // Al añadir el script, deja el collider como Trigger automáticamente
    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (m_Activated && m_OnlyOnce)
            return;

        FPSController l_Player = other.GetComponentInParent<FPSController>();
        if (l_Player == null)
            return;

        Transform l_Point = m_RespawnPoint != null ? m_RespawnPoint : transform;
        l_Player.SetCheckpoint(l_Point.position, l_Point.rotation);
        m_Activated = true;

        Debug.Log("Checkpoint guardado: " + gameObject.name);
    }
}