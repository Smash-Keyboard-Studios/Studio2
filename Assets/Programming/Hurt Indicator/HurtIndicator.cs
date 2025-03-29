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

public class HurtIndicator : MonoBehaviour
{
    [SerializeField]
    private RendererMaterialData[] rendererMaterialData;

    [SerializeField]
    private float opacity = 120f;

    [SerializeField]
    private float hurtDuration = 0.4f;

    private float damageTimer = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (damageTimer > 0) damageTimer -= Time.deltaTime;

        float alpha = damageTimer > 0 ? opacity / 255f : 0;
        // terrible for performance.
        foreach (var hurtMaterial in rendererMaterialData)
        {
            Color materialColor = hurtMaterial.renderer.materials[hurtMaterial.materialIndexInList].color;

            if (materialColor.a != alpha) materialColor = new Color(materialColor.r, materialColor.g, materialColor.b, alpha);

            hurtMaterial.renderer.materials[hurtMaterial.materialIndexInList].color = materialColor;
        }
    }

    public void TakenDamage()
    {
        damageTimer = hurtDuration;
    }

}
