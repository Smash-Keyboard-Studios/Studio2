using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    Renderer fogRenderer;

    [Header("Fade-out Speed")]
    [SerializeField] private float fadeOutSpeed = 0.05f;


    private void Start()
    {
        fogRenderer = GetComponent<Renderer>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject.GetComponent<BoxCollider>()); // Destroys the box collider to prevent it from occurring again
            StartCoroutine(fadeOut()); // Activates the fadeout
        }
    }
    
    IEnumerator fadeOut()
    {
        while (fogRenderer.material.GetFloat("_Opacity") > 0)
        {
            yield return new WaitForSeconds(0.01f);
            fogRenderer.material.SetFloat("_Opacity", fogRenderer.material.GetFloat("_Opacity") - fadeOutSpeed);
        }

        Destroy(this.gameObject); // tells object to stop existing :D
    }

}
