using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtons : MonoBehaviour
{
    

    public void PressLevel1()
    {
        UIManager.Instance.SelectLevel(UIManager.Instance.Level1BuildIndex);
    }

    public void PressLevel2()
    {
        UIManager.Instance.SelectLevel(UIManager.Instance.Level2BuildIndex);
    }
}
