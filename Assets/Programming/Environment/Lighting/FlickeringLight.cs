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


public class FlickeringLight : MonoBehaviour
{
    public Light LightSource;

    public Renderer LightMaterial = null;

    public float MaxOnTime = 1;
    public float MaxOffTime = 1;

    private float _localTime = 0;
    private float _waitTime = 0;

    private Color _emissionColor; // was going to store the color of the light emission, TODO: figure out later.



    // Update is called once per frame
    void Update()
    {
        _localTime += Time.deltaTime;

        if (_localTime >= _waitTime)
        {
            LightSource.enabled = !LightSource.enabled;


            _waitTime = Random.Range(0, LightSource.enabled ? MaxOnTime : MaxOffTime);

            if (LightMaterial != null)
            {
                LightMaterial.material.SetColor("_EmissionColor", LightSource.enabled ? Color.white : Color.black);
            }


            _localTime = 0;
        }
    }
}
