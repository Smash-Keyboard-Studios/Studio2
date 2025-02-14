using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// by 
//     _    _             _  __
//    / \  | | _____  __ | |/ /
//   / _ \ | |/ _ \ \/ / | ' / 
//  / ___ \| | __ />  <  | . \ 
// /_/   \_\_|\___/_/\_\ |_|\_\
public class BaseEnemyProjectile : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] protected Rigidbody projectileRigidBody;
    [SerializeField] public float projectileSpeed; // How fast the object will be launched
    [SerializeField] public float projectileLifespan; // How long the object will last
    [SerializeField] public float projectileDamage;
    public event Action onSFXImpact;
    protected virtual void Awake()
    {
        projectileRigidBody = GetComponent<Rigidbody>();
    }
    protected virtual void Start()
    {
        projectileRigidBody.velocity = transform.forward * projectileSpeed;
    }
    protected virtual void Update()
    {
        Destroy(gameObject, projectileLifespan);
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {

        onSFXImpact?.Invoke();
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(projectileDamage);
            Destroy(gameObject, 0.05f); //Nearly instantly removes projectile to avoid player clipping
        }
    }
}