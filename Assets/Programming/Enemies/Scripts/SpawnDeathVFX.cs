using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|


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
