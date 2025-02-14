using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpecialTankAudio : AICommonMeleeAudio
{
    protected AISpecialTank aiSpecialTank;

    // Start is called before the first frame update
    protected override void Start()
    {
        aiSpecialTank = GetComponent<AISpecialTank>();

        base.Start();
    }

    protected override void SubscribeToAudioEvents()
    {


        base.SubscribeToAudioEvents();
    }

    protected override void OnDeathSFXPlayOnce()
    {
        // Audio code here
    }

    protected override void OnTakeDamageSFXPlayOnce()
    {
        // Audio code here
    }

    protected override void OnWalkingSFXPlay(float speed)
    {
        // Audio code here
    }

    protected override void OnWalkingSFXStop()
    {
        // Audio code here
    }

    protected override void OnAttackSFXPlayOnce(bool obj)
    {
        // Audio code here
    }
}
