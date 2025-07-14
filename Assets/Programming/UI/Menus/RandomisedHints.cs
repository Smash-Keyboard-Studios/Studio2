using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomisedHints : MonoBehaviour
{
    [SerializeField] List<string> HintTexts = new List<string>();

    public TextMeshProUGUI HintText;
    public int currentHint = -1;

    // Start is called before the first frame update
    void OnEnable()
    {
        NextHint();
    }

    public void NextHint()
    {
        int randomHint = Random.Range(0, HintTexts.Count);
        while (randomHint == currentHint)
        {
            randomHint = Random.Range(0, HintTexts.Count);
        }

        string hintText = HintTexts[randomHint];

        currentHint = randomHint;
        HintText.text = hintText;
    }
}
