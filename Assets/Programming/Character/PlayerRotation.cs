using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    void Update()
    {
        HandleRotationInput();
    }

    void HandleRotationInput()
    {
        RaycastHit Hit;
        Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(Ray, out Hit))
        {
            transform.LookAt(new Vector3(Hit.point.x, transform.position.y, Hit.point.z));
        }
    }
}
