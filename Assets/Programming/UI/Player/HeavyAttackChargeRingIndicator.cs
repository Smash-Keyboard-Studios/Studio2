using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|
// © 2025 Dominic McNeill dommcneill@outlook.com


public class HeavyAttackChargeRingIndicator : MonoBehaviour
{
    public Image targetGraphic;

    PlayerAttackHandler playerAttack;

    private int currentChargeAmount = 0;

    public Color throbColor;

    private Color savedColor;

    private float throbTime = 0f;

    public float throbSpeed = 6f;

    public float scaleUpSize = 1.2f;

    private Vector3 savedScale;

    // Start is called before the first frame update
    void Start()
    {
        playerAttack = PlayerReferenceFetcher.instance.GetPlayerReference().GetComponent<PlayerAttackHandler>();
        savedColor = targetGraphic.color;
        savedScale = targetGraphic.rectTransform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentChargeAmount != playerAttack.GetChargedHeavyAmount())
        {
            currentChargeAmount = playerAttack.GetChargedHeavyAmount();
            throbTime = 1f;
        }

        if (throbTime > 0)
        {
            throbTime -= Time.deltaTime * throbSpeed;
        }

        targetGraphic.color = Color.Lerp(savedColor, throbColor, Throb(1 - throbTime));

        targetGraphic.rectTransform.localScale = Vector3.Lerp(savedScale, Vector3.one * scaleUpSize, Throb(1 - throbTime));


        targetGraphic.fillAmount = playerAttack.GetChargedHeavyNormalized();
    }

    private float Throb(float t)
    {
        return Mathf.Sin(Mathf.PI * t);
    }
}
