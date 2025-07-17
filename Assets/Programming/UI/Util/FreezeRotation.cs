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


public class FreezeRotation : MonoBehaviour
{
    public VectorBool lockedAxis = new VectorBool(false, true, false);

    public bool resetToZero = true;

    private Quaternion savedRotation;


    // Start is called before the first frame update
    void Start()
    {
        savedRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(lockedAxis.x ? (resetToZero ? 0 : savedRotation.eulerAngles.x) : transform.rotation.eulerAngles.x,
            lockedAxis.y ? (resetToZero ? 0 : savedRotation.eulerAngles.y) : transform.rotation.eulerAngles.y,
            lockedAxis.z ? (resetToZero ? 0 : savedRotation.eulerAngles.z) : transform.rotation.eulerAngles.z);
    }
}
