using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpace : MonoBehaviour
{
    public List<IDamagable> TakeDamageInRange = new List<IDamagable>();

    public void OnTriggerEnter(Collider other)
    {
        var TakeDamage = other.GetComponent<IDamagable>();
        if (TakeDamage != null)
        {
            TakeDamageInRange.Add(TakeDamage);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var TakeDamage = other.GetComponent <IDamagable>();
        if (TakeDamage != null && TakeDamageInRange.Contains(TakeDamage)) 
        {
            TakeDamageInRange.Remove(TakeDamage);
        }
    }
}
