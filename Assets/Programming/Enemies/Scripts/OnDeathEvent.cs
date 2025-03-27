using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDeathEvent : MonoBehaviour
{
    [SerializeField]
    private AIBase aIToSubscribeTo;

    public UnityEvent onDeath;

    // Start is called before the first frame update
    void Start()
    {
        if (aIToSubscribeTo == null)
            aIToSubscribeTo = GetComponent<AIBase>();

        if (aIToSubscribeTo != null)
            aIToSubscribeTo.onDeath += InvokeOnDeath;
    }

    private void InvokeOnDeath(Transform eventData)
    {
        onDeath?.Invoke();
    }
}
