using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{

    [Header("Fade-out Speed")]
    [SerializeField] private float fadeOutSpeed;

    private Renderer fogRenderer;
    private Color fogMaterial;

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
        while (fogRenderer.material.color.a > 0.01f) // Checks it at 0.01f as otherwise it gets mad :(
        {
            fogRenderer.material.color = new Color(fogMaterial.r, fogMaterial.g, fogMaterial.b, Mathf.Lerp(fogRenderer.material.color.a, 0, (fadeOutSpeed / 100))); // Lerps the transparency, divides the set fadeout speed by 100 to compensate
            yield return null; // stupid stupid coroutines!!!!!!!!
        }
        Destroy(this.gameObject); // tells object to stop existing :D
    }
}
