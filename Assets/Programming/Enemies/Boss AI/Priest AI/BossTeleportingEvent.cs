using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossTeleportingEvent : MonoBehaviour
{
    public UnityEvent BossTeleportingToThis;
    public UnityEvent BossTeleportedToThis;

    public void StartTeleport()
    {
        BossTeleportingToThis?.Invoke();
    }

    public void EndTeleport()
    {
        BossTeleportedToThis?.Invoke();
    }
}
