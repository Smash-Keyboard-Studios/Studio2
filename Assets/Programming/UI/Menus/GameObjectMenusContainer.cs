using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectMenusContainer : MonoBehaviour
{
    //stuff ensuring there is a single instance of the menus
    [HideInInspector] public static GameObjectMenusContainer Instance { get { return instance; } }
    private static GameObjectMenusContainer instance;

    private void Awake()
    {
        //makes sure there is only one set of menus and that it is set to this
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // prevents this from being destroyed between scenes
        }
    }
}
