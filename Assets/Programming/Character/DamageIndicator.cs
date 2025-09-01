using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public float blinkIntensity;
    public float blinkDuration;
    public float flashTime;
    float blinkTimer;

    public SkinnedMeshRenderer skinnedMeshRenderer;

    void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        skinnedMeshRenderer.material.color = Color.white;
    }

    void Update()
    {
    }

    public void FlashStart()
    {
        blinkTimer = blinkDuration;
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.red * intensity;
        Invoke("FlashStop", flashTime);
    }

    public void FlashStop()
    {
        skinnedMeshRenderer.material.color = Color.white;
    }
}
