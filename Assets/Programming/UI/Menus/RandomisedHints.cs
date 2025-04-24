using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomisedHints : MonoBehaviour
{
    [SerializeField] List<string> HintTexts = new List<string>();

    public TextMeshProUGUI HintText;

    // Start is called before the first frame update
    void OnEnable()
    {
        int randomHint = Random.Range(0, HintTexts.Count);

        string hintText = HintTexts[randomHint];

        HintText.text = hintText;
    }
}
