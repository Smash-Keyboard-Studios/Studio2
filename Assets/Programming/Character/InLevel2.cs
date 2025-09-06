using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is for the functionality of the Level2SelectDisable script
//it exists solely to tell playerprefs that level 2 has been unlocked
public class InLevel2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("Level2Unlocked", 1);
    }
}
