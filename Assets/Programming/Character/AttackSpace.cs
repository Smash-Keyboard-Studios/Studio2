using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpace : MonoBehaviour
{
    public List<IDamageable> TakeDamageInRange = new List<IDamageable>();

    public void OnTriggerEnter(Collider other)
    {
        var TakeDamage = other.GetComponent<IDamageable>();
        if (TakeDamage != null)
        {
            TakeDamageInRange.Add(TakeDamage);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var TakeDamage = other.GetComponent<IDamageable>();
        if (TakeDamage != null && TakeDamageInRange.Contains(TakeDamage))
        {
            TakeDamageInRange.Remove(TakeDamage);
        }
    }
}
