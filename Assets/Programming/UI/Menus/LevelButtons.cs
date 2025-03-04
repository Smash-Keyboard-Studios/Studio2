using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtons : MonoBehaviour
{
    //build indexes for the first scene of each level
    public int Level1BuildIndex;
    public int Level2BuildIndex;

    public void PressLevel1()
    {
        UIManager.Instance.SelectLevel(Level1BuildIndex);
    }

    public void PressLevel2()
    {
        UIManager.Instance.SelectLevel(Level2BuildIndex);
    }
}
