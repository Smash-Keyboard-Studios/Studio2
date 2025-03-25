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
	[SerializeField] public float gravityScale = 1.0f;
	[SerializeField] public static float globalGravity = -9.81f;
	public event Action onSFXImpact;
    protected virtual void Awake()
    {
        projectileRigidBody = GetComponent<Rigidbody>();
        projectileRigidBody.useGravity = false;
    }
    protected virtual void Start()
    {
        projectileRigidBody.velocity = transform.forward * rangedSpeed;
    }
    protected virtual void Update()
    {
		Destroy(gameObject, rangedLifespan);
    }
    protected virtual void FixedUpdate()
    {
		Vector3 manualGravity = globalGravity * gravityScale * Vector3.up;
		projectileRigidBody.AddForce(manualGravity, ForceMode.Acceleration);
	}
    protected virtual void OnTriggerEnter(UnityEngine.Collider collision)
	{

        onSFXImpact?.Invoke();
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(rangedDamage);
        Destroy(gameObject, 0.05f); //Nearly instantly removes projectile to avoid player clipping
    }
}