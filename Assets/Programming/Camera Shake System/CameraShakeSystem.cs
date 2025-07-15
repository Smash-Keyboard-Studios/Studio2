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
// Licensed for use within the Wraiths of Retail by Smash Keyboard Studios (SKS) only.
// Redistribution or modification outside of this project is prohibited without explicit written permission.
// For full license terms, see DOMIBRON_CODE_LICENSE.md at the project root.


public class CameraShakeSystem : MonoBehaviour
{
    public static CameraShakeSystem instance;

    public float fadeAwayCutOffThreshold = 0.3f;


    private Transform cameraShakeTransform;
    private Vector3 cameraPosition;

    private float shakeTimer = 0f;
    private float totalShakeTime = 0f;
    private float verticalShakeFrequency = 0f;
    private float horizontalShakeFrequency = 0f;
    private float shakeIntensity = 0f;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;

            cameraShakeTransform = Camera.main.transform.parent;
            cameraPosition = cameraShakeTransform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            float shakeFadeAway = shakeTimer / totalShakeTime;
            if (shakeFadeAway < fadeAwayCutOffThreshold) shakeFadeAway = 0f; // we set to zero to get rid of micro shakes.

            cameraShakeTransform.localPosition = cameraPosition
                + new Vector3(Mathf.Sin(shakeTimer * horizontalShakeFrequency) * shakeIntensity * shakeFadeAway, Mathf.Sin(shakeTimer * verticalShakeFrequency) * shakeIntensity * shakeFadeAway, 0);

        }
        else
        {
            cameraShakeTransform.localPosition = cameraPosition;
            totalShakeTime = 0f;
            shakeTimer = 0f;
            shakeIntensity = 0f;
            verticalShakeFrequency = 50f;
            horizontalShakeFrequency = 20f;
        }
    }


    /// <summary>
    /// Starts the camera shake.
    /// </summary>
    /// <param name="duration">How long the shake should last.</param>
    /// <param name="shakeIntensity">How much the camera should move.</param>
    /// <param name="verticalShakeFrequency">How much the camera should modulate vertically (up and down).</param>
    /// <param name="horizontalShakeFrequency">How much the camera should modulate horizontally (lest and right).</param>
    public void StartShake(float duration, float shakeIntensity = 0.05f, float verticalShakeFrequency = 50f, float horizontalShakeFrequency = 20f)
    {
        // add to shake and combine.
        shakeTimer += duration;
        totalShakeTime += duration;
        this.shakeIntensity += shakeIntensity;

        // dont combine or strange things will occur.
        this.verticalShakeFrequency = verticalShakeFrequency;
        this.horizontalShakeFrequency = horizontalShakeFrequency;
    }
}
