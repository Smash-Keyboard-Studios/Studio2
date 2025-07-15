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

/// <summary>
/// Gives more customisation on how the object spins.
/// </summary>
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
