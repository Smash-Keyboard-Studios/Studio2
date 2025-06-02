using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Vince

public class FogOfWar : MonoBehaviour
{
    Renderer fogRenderer;
    public Renderer miniMapFogRenderer;

    [Header("Fade-out Speed")]
    [SerializeField] private float fadeOutSpeed = 0.05f;

    public UnityEvent fadingOut;

    private void Start()
    {
        fogRenderer = GetComponent<Renderer>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject.GetComponent<BoxCollider>()); // Destroys the box collider to prevent it from occurring again
            StartCoroutine(fadeOut()); // Activates the fadeout
            fadingOut.Invoke();
        }
    }

    IEnumerator fadeOut()
    {
        while (fogRenderer.material.GetFloat("_Opacity") > 0)
        {
            yield return new WaitForSeconds(0.01f);
            fogRenderer.material.SetFloat("_Opacity", fogRenderer.material.GetFloat("_Opacity") - fadeOutSpeed);
            miniMapFogRenderer.material.SetFloat("_Opacity", fogRenderer.material.GetFloat("_Opacity"));
        }

        yield return new WaitForSeconds(2f); // Wait for SFX to finish, very hardbaked

        Destroy(this.gameObject); // tells object to stop existing :D
    }


}
