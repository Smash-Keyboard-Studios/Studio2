using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Will replace with new system", false)]
public class PlayerDetectNearby : MonoBehaviour
{
    //soph

    public float entityDetectionRangeRadius = 10f;

    public bool boss1Nearby;
    public bool boss2Nearby;
    public bool enemiesNearby;
    public bool itemsNearby;

    // Update is called once per frame
    void Update()
    {
        HandleEntityDetection();
    }

    private void HandleEntityDetection()
    {
        Collider[] results = Physics.OverlapSphere(transform.position, entityDetectionRangeRadius, Physics.AllLayers, QueryTriggerInteraction.Collide);

        if (results.Length > 0)
        {
            bool enemy = false;
            bool item = false;

            foreach (Collider c in results)
            {
                if (c.gameObject.CompareTag("Item"))
                {
                    item = true;
                }
                else if (c.gameObject.GetComponent<AIBase>())
                {
                    //check for bosses - once boss music starts it doesnt stop again
                    if (c.gameObject.GetComponent<KnightAI>()) { boss1Nearby = true; }
                    if (c.gameObject.GetComponent<ImprovedPriestAI>()) { boss2Nearby = true; }

                    if (c.gameObject.GetComponent<AIBase>().currentAIState == AIState.Alerted)
                    {
                        enemy = true;
                    }
                }
            }

            enemiesNearby = enemy;
            itemsNearby = item;
        }
        else
        {
            enemiesNearby = false;
            itemsNearby = false;
        }
    }
}
