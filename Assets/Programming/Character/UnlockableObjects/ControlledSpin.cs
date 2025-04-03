using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledSpin : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private bool randomSpin = false;

    [SerializeField]
    private Vector3 spinAxis = Vector3.up;

    private float localTime = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        localTime += Time.deltaTime;


        if (!randomSpin)
        {
            transform.Rotate(spinAxis * speed);
        }
        else
        {
            transform.Rotate(new Vector3(Mathf.Cos(localTime), Mathf.Sin(localTime)) * speed);
        }
    }
}
