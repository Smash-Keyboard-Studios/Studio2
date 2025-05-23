using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    public Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        //moveDirection = Random.insideUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * Time.deltaTime, Space.World);
    }
}
