using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|


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

    private void InvokeOnDetectPlayer(AIState prevState, AIState newState)
    {
        if (newState == AIState.Alerted)
        {
            onDetectPlayer?.Invoke();
        }
    }
}
