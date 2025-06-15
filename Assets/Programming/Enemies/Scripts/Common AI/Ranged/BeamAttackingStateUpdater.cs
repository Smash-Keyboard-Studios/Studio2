using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttackingStateUpdater : MonoBehaviour
{
    AICommonBeam animationStateUpdater;

    void Awake()
    {
        animationStateUpdater = GetComponentInParent<AICommonBeam>();
    }

    public void EndAttack()
    {
        animationStateUpdater.EndAttack();
    }

    public void DealAttack()
    {
        animationStateUpdater.DealAttack();
    }
}
