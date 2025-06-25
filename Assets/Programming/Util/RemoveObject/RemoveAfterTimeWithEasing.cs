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


/// <summary>
/// Removes the object after a period of time but also scales the object in and out using easing.
/// </summary>
public class RemoveAfterTimeWithEasing : MonoBehaviour
{
    public float timeToWaitBeforeRemoving = 5f;

    public bool shouldEaseIn = true;

    public float easeInTime = 1f;
    public float easeOutTime = 1f;

    private float easingTime = 0f;

    private float localTime = 0f;

    private Vector3 targetScale;

    void Awake()
    {
        targetScale = transform.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToWaitBeforeRemoving);
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        localTime += Time.deltaTime;

        if (localTime <= easeInTime && shouldEaseIn)
        {
            easingTime += Time.deltaTime;
            if (shouldEaseIn) transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, easeOutQuint(easingTime / easeInTime));
        }
        else if (timeToWaitBeforeRemoving - localTime >= 0 && timeToWaitBeforeRemoving - localTime <= easeOutTime + 0.1f)
        {
            easingTime -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, easeOutQuint(easingTime / easeOutTime));
        }


    }

    // TODO move to a easing script.
    private float easeOutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }
}
