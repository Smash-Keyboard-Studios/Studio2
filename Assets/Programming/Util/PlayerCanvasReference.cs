using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvasReference : MonoBehaviour
{
    public static PlayerCanvasReference instance { private set; get; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("Duplicate " + nameof(PlayerCanvasReference) + " found, please remove the one of them!", this);
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public GameObject GetPlayerCanvasReference()
    {
        return gameObject;
    }
}
