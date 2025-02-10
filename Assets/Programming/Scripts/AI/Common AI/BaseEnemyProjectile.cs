using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody projectileRigidBody;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileLifespan;
    [SerializeField] public float projectileDamage;
    void Start()
    {
        projectileRigidBody.velocity = transform.forward * projectileSpeed;
    }

    // Update is called once per frame
    void Update()
    {
    
        Destroy(gameObject, projectileLifespan);
    }
	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(projectileDamage);
        }
    }
}
