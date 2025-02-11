using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSFXPlayer : MonoBehaviour
{
    AICommonRangedCombat referencedAI;
    void Start()
    {
        referencedAI = GetComponent<AICommonRangedCombat>();
        referencedAI.onSFXProjectileLaunch += SFXProjectileLaunch;
    }

    void SFXProjectileLaunch()
        {
        // Audio people work here, ty
        }
}
