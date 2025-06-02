using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionTag : MonoBehaviour
{
    public TMP_Text uiText;

    // Start is called before the first frame update
    void Start()
    {
        uiText.text = "V " + Application.version;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
