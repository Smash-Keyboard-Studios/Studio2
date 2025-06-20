using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAOE : MonoBehaviour
{
    public float fireTickDamage = 5f;

    public float fireDuration = 3f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<FireTickManager>().SetOnFire(fireDuration, fireTickDamage);
        }
    }
}
