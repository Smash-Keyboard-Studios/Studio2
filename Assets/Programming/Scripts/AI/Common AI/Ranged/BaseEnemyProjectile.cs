using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// by 
//     _    _             _  __
//    / \  | | _____  __ | |/ /
//   / _ \ | |/ _ \ \/ / | ' / 
//  / ___ \| | __ />  <  | . \ 
// /_/   \_\_|\___/_/\_\ |_|\_\
public class BaseEnemyProjectile : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private Rigidbody projectileRigidBody; 
    [SerializeField] private float projectileSpeed; // How fast the object will be launched
    [SerializeField] private float projectileLifespan; // How long the object will last
    [SerializeField] public float projectileDamage;
	private void Awake()
	{
        projectileRigidBody = GetComponent<Rigidbody>(); 
    }
    private void Start()
    {
        projectileRigidBody.velocity = transform.forward * projectileSpeed;
    }
    private void Update()
    {
        Destroy(gameObject, projectileLifespan);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(projectileDamage);
            Destroy(gameObject, 0.05f); //Nearly instantly removes projectile to avoid player clipping
        }
    }
}