using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeavyAttackChargeRingIndicator : MonoBehaviour
{
    public Image targetGraphic;

    PlayerAttackHandler playerAttack;

    // Start is called before the first frame update
    void Start()
    {
        playerAttack = PlayerReferenceFetcher.instance.GetPlayerReference().GetComponent<PlayerAttackHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        targetGraphic.fillAmount = playerAttack.GetChargedHeavyAmountNormalized();
    }
}
