using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHitIndicator : MonoBehaviour
{
    [SerializeField]
    private RendererMaterialData rendererMaterialData;

    [SerializeField]
    private float opacity = 210f;

    [SerializeField]
    private float hurtDuration = 0.2f;

    private float damageTimer = 0;

    private float savedAlpha;

    void Start()
    {

        savedAlpha = rendererMaterialData.renderer.materials[rendererMaterialData.materialIndexInList].GetColor("_Color").a;

    }

    // Update is called once per frame
    void Update()
    {
        if (damageTimer > 0) damageTimer -= Time.deltaTime;

        // terrible for performance.

        float blend = Mathf.Sin((damageTimer / hurtDuration) * Mathf.PI);

        float alpha = damageTimer > 0 ? Mathf.Lerp(savedAlpha, opacity / 255f, blend) : savedAlpha;


        Color materialColor = rendererMaterialData.renderer.materials[rendererMaterialData.materialIndexInList].GetColor("_Color");

        if (materialColor.a != alpha) materialColor = new Color(materialColor.r, materialColor.g, materialColor.b, alpha);

        rendererMaterialData.renderer.materials[rendererMaterialData.materialIndexInList].SetColor("_Color", materialColor);

    }

    public void ShieldHit()
    {
        damageTimer = hurtDuration;
    }
}
