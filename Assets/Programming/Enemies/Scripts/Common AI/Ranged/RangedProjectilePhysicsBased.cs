using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|


[RequireComponent(typeof(Rigidbody))]
public class RangedProjectilePhysicsBased : MonoBehaviour
{
    private Rigidbody rb;

    private float projectileDamage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<IDamageable>().TakeDamage(projectileDamage);
            Destroy(gameObject);
        }

        Destroy(gameObject);
    }

    public void SetUpProjectile(Vector3 force, float damage)
    {
        rb.AddForce(force, ForceMode.VelocityChange);
        projectileDamage = damage;
    }
}
