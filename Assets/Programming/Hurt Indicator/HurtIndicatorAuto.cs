using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|

public class HurtIndicatorAuto : MonoBehaviour
{
    [SerializeField]
    private string materialName = "HurtColor";

    // private Renderer[] allRenderers;
    private List<Material> allMaterials = new List<Material>();

    [SerializeField]
    protected float opacity = 150f;

    [SerializeField]
    protected float hurtDuration = 0.2f;

    protected float damageTimer = 0;


    void Start()
    {
        Renderer[] allRenderers = transform.GetComponentsInChildren<Renderer>();

        foreach (var renderer in allRenderers)
        {
            foreach (var material in renderer.materials)
            {
                if (!material.name.Contains(materialName)) continue;
                allMaterials.Add(material);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (damageTimer > 0) damageTimer -= Time.deltaTime;

        // this makes the color fade in and out when using just time.
        float blend = Mathf.Sin((damageTimer / hurtDuration) * Mathf.PI);

        float alpha = damageTimer > 0 ? Mathf.Lerp(0, opacity / 255f, blend) : 0;



        foreach (var material in allMaterials)
        {
            if (material.color.a == alpha) continue;

            material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);

        }
    }



    public virtual void TakenDamage()
    {
        damageTimer = hurtDuration;
    }

}
