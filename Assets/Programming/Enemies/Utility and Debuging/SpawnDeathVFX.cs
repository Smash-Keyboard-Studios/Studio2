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


public class SpawnDeathVFX : MonoBehaviour
{
    [SerializeField]
    private GameObject deathVFX;

    // Start is called before the first frame update
    void Start()
    {


        GetComponent<AIBase>().onDeath += SpawnVFX;
    }

    private void SpawnVFX(Transform aiTransform)
    {
        Instantiate(deathVFX, aiTransform.position, aiTransform.rotation);
    }
}
