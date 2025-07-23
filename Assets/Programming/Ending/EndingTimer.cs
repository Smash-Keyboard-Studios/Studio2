using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTimer : MonoBehaviour
{
    public float endCutsceneLength = 5;
    [SerializeField] private GameObject endingUI;

    private void Awake()
    {
        endingUI.SetActive(false);
        StartCoroutine("EndingCutscene");
    }

    private IEnumerator EndingCutscene()
    {
        yield return new WaitForSeconds(endCutsceneLength);
        endingUI.SetActive(true);
    }
}
