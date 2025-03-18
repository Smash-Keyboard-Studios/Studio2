using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Script by Vince

public class FadeSpectralWall : MonoBehaviour
{
    Renderer spectralRenderer;

    public float fadeSpeed = 0.3f; // How fast the spectral walls fade in/out

    public float highestFadeValue = 12; // Highest value for wall to be invisible
    public float lowestFadeValue = 0;  // Value for the most visible the wall can be

    public UnityEvent endOfFadeout;
    public UnityEvent endOfFadein;

    void Awake()
    {
        spectralRenderer = GetComponent<Renderer>();

        // Create new instance of material so that it doesn't overwrite others
        spectralRenderer.material = new Material(spectralRenderer.material);
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }
    public void StartFadeIn()
    {
        spectralRenderer.material.SetFloat("_Fade", highestFadeValue);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        // Make wall lose opacity through shader
        while (spectralRenderer.material.GetFloat("_Fade") < highestFadeValue)
        {
            yield return new WaitForSeconds(0.01f);
            spectralRenderer.material.SetFloat("_Fade", spectralRenderer.material.GetFloat("_Fade") + fadeSpeed);
        }
        spectralRenderer.material.SetFloat("_Fade", highestFadeValue);

        endOfFadeout.Invoke();
    }

    private IEnumerator FadeIn()
    {
        // Make wall gain opacity through shader
        while (spectralRenderer.material.GetFloat("_Fade") > lowestFadeValue)
        {
            yield return new WaitForSeconds(0.01f);
            spectralRenderer.material.SetFloat("_Fade", spectralRenderer.material.GetFloat("_Fade") - fadeSpeed);
        }
        spectralRenderer.material.SetFloat("_Fade", lowestFadeValue);

        endOfFadein.Invoke();
    }
}
