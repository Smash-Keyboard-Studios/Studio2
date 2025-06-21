using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|


public class ShieldHitIndicator : MonoBehaviour
{
    // TODO change this into hooking into health rather than health hooking into this.

    [SerializeField]
    private string shieldMaterialName = "Shield";

    private Material shieldMaterial;

    [SerializeField]
    private float opacity = 210f;

    [SerializeField]
    private float hurtDuration = 0.2f;

    private float damageTimer = 0;

    private float savedAlpha;

    void Start()
    {
        Renderer[] allRenderers = transform.GetComponentsInChildren<Renderer>();

        foreach (var renderer in allRenderers)
        {
            foreach (var material in renderer.materials)
            {
                // print(material.name + " is on " + transform.name);
                if (!material.name.Contains(shieldMaterialName)) continue;

                shieldMaterial = material;
            }
        }

        savedAlpha = shieldMaterial.GetColor("_Color").a;

        GetComponent<HealthWithBasicShield>().onShieldHit += ShieldHit;
    }

    // Update is called once per frame
    void Update()
    {
        if (damageTimer > 0) damageTimer -= Time.deltaTime;

        float blend = Mathf.Sin((damageTimer / hurtDuration) * Mathf.PI);

        float alpha = damageTimer > 0 ? Mathf.Lerp(savedAlpha, opacity / 255f, blend) : savedAlpha;



        if (shieldMaterial.color.a != alpha) shieldMaterial.SetColor("_Color", new Color(shieldMaterial.color.r, shieldMaterial.color.g, shieldMaterial.color.b, alpha));

    }

    public void ShieldHit()
    {
        damageTimer = hurtDuration;
    }
}
