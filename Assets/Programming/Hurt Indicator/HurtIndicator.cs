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

[Serializable]
public struct RendererMaterialData
{
    public Renderer renderer;
    public int materialIndexInList;
}

[Obsolete("Please use the HurtIndicatorAuto, it collects the materials automatically.")]
public class HurtIndicator : MonoBehaviour
{
    public RendererMaterialData[] rendererMaterialData;

    [SerializeField]
    protected float opacity = 150f;

    [SerializeField]
    protected float hurtDuration = 0.2f;

    protected float damageTimer = 0;



    // Update is called once per frame
    protected virtual void Update()
    {
        if (damageTimer > 0) damageTimer -= Time.deltaTime;

        float blend = Mathf.Sin((damageTimer / hurtDuration) * Mathf.PI);

        float alpha = damageTimer > 0 ? Mathf.Lerp(0, opacity / 255f, blend) : 0;


        // terrible for performance.
        foreach (var hurtMaterial in rendererMaterialData)
        {
            Color materialColor = hurtMaterial.renderer.materials[hurtMaterial.materialIndexInList].color;

            if (materialColor.a != alpha) materialColor = new Color(materialColor.r, materialColor.g, materialColor.b, alpha);

            hurtMaterial.renderer.materials[hurtMaterial.materialIndexInList].color = materialColor;
        }
    }

    public virtual void TakenDamage()
    {
        damageTimer = hurtDuration;
    }

}
