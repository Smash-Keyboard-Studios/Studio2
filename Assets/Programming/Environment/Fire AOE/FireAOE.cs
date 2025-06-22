using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



public class FireAOE : MonoBehaviour
{
    public float fireTickDamage = 5f;

    public float fireDuration = 3f;

    private FireTickManager fireTickManager;

    void Start()
    {
        if (PlayerReferenceFetcher.instance == null) Debug.LogError($"Cannot get the player reference because the {nameof(PlayerReferenceFetcher)} was not found!");

        fireTickManager = PlayerReferenceFetcher.instance.GetPlayerReference()?.GetComponent<FireTickManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            fireTickManager.SetOnFire(fireDuration, fireTickDamage);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            fireTickManager.SetOnFire(fireDuration, fireTickDamage);
        }
    }
}
