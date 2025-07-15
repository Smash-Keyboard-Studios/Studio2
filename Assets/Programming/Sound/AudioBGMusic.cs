using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBGMusic : MonoBehaviour
{
    //soph

    public enum LevelNumber
    {
        One, 
        Two
    }

    public LevelNumber whichLevel;

    private bool inBoss1;
    private bool inBoss2;
    private bool inCombat;

    private AudioSource audioSource;
    private PlayerDetectNearby playerDetectNearby;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerDetectNearby = GameObject.Find("Player").GetComponent<PlayerDetectNearby>();
    }

    // Update is called once per frame
    void Update()
    {
        inBoss1 = playerDetectNearby.boss1Nearby;
        inBoss2 = playerDetectNearby.boss2Nearby;
        inCombat = playerDetectNearby.enemiesNearby;

        if (!LevelLoading.instance.loading)
        {
            playBGM();
        }
    }

    private void playBGM()
    {
        switch (whichLevel)
        {
            case LevelNumber.One:

                if (inBoss1)
                {
                    AudioManager.Instance.PlayAudio(true, true, audioSource, "Boss1");
                    
                }
                else
                {
                    AudioManager.Instance.PlayAudio(true, false, audioSource, inCombat ? "L1_Combat" : "L1_NonCombat");
                }

                break;

            case LevelNumber.Two:

                if (inBoss2)
                {
                    AudioManager.Instance.PlayAudio(true, true, audioSource, "Boss2");
                }
                else
                {
                    AudioManager.Instance.PlayAudio(true, false, audioSource, inCombat ? "L2_Combat" : "L2_NonCombat");
                }

                break;

        }
    }
}
