using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeSpectralWall : MonoBehaviour
{
    Renderer renderer;

    public float fadeSpeed = 0.3f; // How fast the spectral walls fade in/out

    public float highestFadeValue = 12; // Highest value for wall to be invisible
    public float lowestFadeValue = 0;  // Value for the most visible the wall can be

    public UnityEvent endOfFadeout;
    public UnityEvent endOfFadein;

    void Awake()
    {
        renderer = GetComponent<Renderer>();

        // Create new instance of material so that it doesn't overwrite others
        renderer.material = new Material(renderer.material);
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }
    public void StartFadeIn()
    {
        renderer.material.SetFloat("_Fade", highestFadeValue);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        // Make wall lose opacity through shader
        while (renderer.material.GetFloat("_Fade") < highestFadeValue)
        {
            yield return new WaitForSeconds(0.01f);
            renderer.material.SetFloat("_Fade", renderer.material.GetFloat("_Fade") + fadeSpeed);
        }
        renderer.material.SetFloat("_Fade", highestFadeValue);

        endOfFadeout.Invoke();
    }

    private IEnumerator FadeIn()
    {
        // Make wall gain opacity through shader
        while (renderer.material.GetFloat("_Fade") > lowestFadeValue)
        {
            yield return new WaitForSeconds(0.01f);
            renderer.material.SetFloat("_Fade", renderer.material.GetFloat("_Fade") - fadeSpeed);
        }
        renderer.material.SetFloat("_Fade", lowestFadeValue);

        endOfFadein.Invoke();
    }
}
