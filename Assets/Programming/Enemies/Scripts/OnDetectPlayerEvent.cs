using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDetectPlayerEvent : MonoBehaviour
{
    [SerializeField]
    private AIBase aIToSubscribeTo;

    public UnityEvent onDetectPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (aIToSubscribeTo == null)
            aIToSubscribeTo = GetComponent<AIBase>();

        if (aIToSubscribeTo != null)
            aIToSubscribeTo.onStateChanged += InvokeOnDetectPlayer;
    }

    private void InvokeOnDetectPlayer(AIState state)
    {
        if (state == AIState.Alerted)
        {
            onDetectPlayer?.Invoke();
        }
    }
}
