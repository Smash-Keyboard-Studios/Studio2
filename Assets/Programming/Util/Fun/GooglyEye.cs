using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|
// © 2025 Dominic McNeill dommcneill@outlook.com


public class GooglyEye : MonoBehaviour
{
    public Transform eye;

    public float maxDistanceFromCentre = 0.25f;

    public float speed = 1f;
    public float gravityMultiply = 1f;
    public float bounciness = 0.5f;

    // how much the eye will move based on the movement.
    public float reactionAmount = 500f;

    private Vector3 velocity;
    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // convert gravity into local space.
        Vector3 localGravity = transform.InverseTransformDirection(Physics.gravity);

        // add gravity to vel.
        velocity += localGravity * gravityMultiply * Time.deltaTime;

        // get movement from parent and convert it into local space.
        velocity += transform.InverseTransformVector(lastPos - transform.position) * reactionAmount * Time.deltaTime;

        // reset z as we only move in up down left right. 2d.
        velocity.z = 0;

        // get eye pos so we can do stuff to it.
        Vector3 eyePos = eye.localPosition;

        // get the future position for the eye.
        eyePos += velocity * speed * Time.deltaTime;

        // get the direction from the offset from centre.
        Vector2 direction = new Vector2(eyePos.x, eyePos.y);

        // get the angle from the direction.
        float angle = Mathf.Atan2(direction.y, direction.x);

        // if the direction (in this case, the offset from centre) is over max distance.
        if (direction.magnitude > maxDistanceFromCentre)
        {
            // get inverted direction as we need to bounce.
            var normal = -direction.normalized;

            // get the new velocity from the eye hitting the side of the container.
            velocity = Vector2.Reflect(new Vector2(velocity.x, velocity.y), normal) * bounciness;

            // make sure the eye does not go out of bounds for this frame.
            eyePos = new Vector3(Mathf.Cos(angle) * maxDistanceFromCentre, Mathf.Sin(angle) * maxDistanceFromCentre, 0f);
        }

        // reset z and apply everything.
        eyePos.z = eye.localPosition.z;
        eye.localPosition = eyePos;

        // store new last pos.
        lastPos = transform.position;
    }
}
