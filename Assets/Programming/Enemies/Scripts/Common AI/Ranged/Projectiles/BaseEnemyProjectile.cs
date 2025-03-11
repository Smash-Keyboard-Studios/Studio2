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
    [SerializeField] public float rangedSpeed; // How fast the object will be launched
    [SerializeField] public float rangedLifespan; // How long the object will last
    [SerializeField] public float rangedDamage;
    public event Action onSFXImpact;
    protected virtual void Awake()
    {
        projectileRigidBody = GetComponent<Rigidbody>();
    }
    protected virtual void Start()
    {
        projectileRigidBody.velocity = transform.forward * rangedSpeed;
    }
    protected virtual void Update()
    {
        Destroy(gameObject, rangedLifespan);
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {

        onSFXImpact?.Invoke();
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(rangedDamage);
        Destroy(gameObject, 0.05f); //Nearly instantly removes projectile to avoid player clipping
    }
}