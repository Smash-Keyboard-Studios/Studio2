using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BeamAttackSettings
{
    public float coolDown = 5f;

    public float tickDamage = 5f;

    public float tickRate = 0.5f;

    public float windUpTime = 1f;

    public float attackDuration = 2f;

    public float beamRadius = 1f;

    public float beamMaxRange = 999f;

    public float turnSpeedWhileCharging = 1f;

    public LineRenderer lineRenderer;
}

[Serializable]
public class BeamAttack
{
    public float coolDown = 5f;

    public float tickDamage = 5f;

    public float tickRate = 0.5f;

    public float windUpTime = 1f;

    public float attackDuration = 2f;

    public float beamRadius = 1f;

    public float beamMaxRange = 999f;

    public float turnSpeedWhileCharging = 1f;

    public LineRenderer lineRenderer;

    public void SetColor(Color color)
    {
        lineRenderer.colorGradient = new Gradient()
        {
            colorKeys = new GradientColorKey[] { new GradientColorKey(color, 0), new GradientColorKey(color, 1) },
            alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) }
        };
    }

    public void SetWidth(float width)
    {
        lineRenderer.startWidth = width;
    }

    public void SetEndPosition(Vector3 position)
    {
        lineRenderer.SetPosition(1, position);
    }

    public void SetBeamActive(bool active)
    {
        lineRenderer.enabled = active;
    }
}
