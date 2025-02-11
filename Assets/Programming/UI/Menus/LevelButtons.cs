using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtons : MonoBehaviour
{
    //build indexes for the first scene of each level
    public int Level2BuildIndex;
    public int Level3BuildIndex;

    public void PressLevel2()
    {
        UIManager.Instance.SelectLevel(Level2BuildIndex);
    }

    public void PressLevel3()
    {
        UIManager.Instance.SelectLevel(Level3BuildIndex);
    }
}
