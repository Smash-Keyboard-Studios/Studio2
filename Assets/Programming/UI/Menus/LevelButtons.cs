using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtons : MonoBehaviour
{
    public void RestartLevel()
    {
        UIManager.Instance.RestartLevel();
    }

    public void PressLevel1()
    {
        UIManager.Instance.SelectLevel(UIManager.Instance.Level1BuildIndexes[0]);
    }

    public void PressLevel2()
    {
        UIManager.Instance.SelectLevel(UIManager.Instance.Level2BuildIndexes[0]);
    }
}
