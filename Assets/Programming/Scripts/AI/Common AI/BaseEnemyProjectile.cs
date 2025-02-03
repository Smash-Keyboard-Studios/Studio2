using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody ProjectileRigidBody;
    [SerializeField] private float ProjectileSpeed;
    [SerializeField] private float ProjectileLifespan;
    void Start()
    {
        ProjectileRigidBody.velocity = transform.forward * ProjectileSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, ProjectileLifespan);
    }
}
