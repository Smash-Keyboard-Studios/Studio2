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

public class BeamAttack : MonoBehaviour
{

}
