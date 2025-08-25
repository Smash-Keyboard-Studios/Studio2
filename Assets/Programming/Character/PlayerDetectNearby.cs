using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerDetectNearby : MonoBehaviour
{
    public float detectionRadius = 5;
    public float detectionFrequecy = 2;

    public bool enemiesNearby = false;
    public bool boss1Nearby = false;
    public bool boss2Nearby = false;

    // Start is called before the first frame update
    void Start()
    {
        //check for enemies every x seconds
        InvokeRepeating("CheckForEnemies", 0f, detectionFrequecy);
    }

    private void CheckForEnemies()
    {
        //get all the objects within detection radius around us
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, detectionRadius);
        if (hitObjects.Length > 0)
        {
            foreach (var hitObject in hitObjects)
            {
                //check for bosses
                //we dont want to disable boss music after it starts cus otherwise it will cut out janky
                if (hitObject.GetComponent<KnightAI>() != null)
                {
                    boss1Nearby = true;
                    return;
                }
                else if (hitObject.GetComponent<PriestAI>() != null)
                {
                    boss2Nearby = true;
                    return;
                }

                //check if object is an enemy
                AIBase hitObjectAI = hitObject.GetComponent<AIBase>();
                if (hitObjectAI != null)
                {
                    //if the enemy is alerted then there are enemies nearby
                    if (hitObjectAI.currentAIState == AIState.Alerted)
                    {
                        enemiesNearby = true;
                        return;
                    }
                }
            }
        }
        //if no enemies found above then none nearby
        enemiesNearby = false;
        return;
    }
}
