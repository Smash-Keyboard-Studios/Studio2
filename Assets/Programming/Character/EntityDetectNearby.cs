using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|


public class EntityDetectNearby : MonoBehaviour
{
    /// <summary>
    /// Checks for any objects with given tags.
    /// </summary>
    /// <param name="radius">The range for the sphere check.</param>
    /// <param name="primaryTag">The main tag on the game object.</param>
    /// <param name="subTag">The sub tag using the sub tag system script.</param>
    /// <param name="layerMask">Layers to check for to optimise the physics check.</param>
    /// <returns>True if this has found a match.</returns>
    public bool CheckWithinRadius(float radius, string primaryTag, string subTag = "", int layerMask = Physics.AllLayers)
    {
        Collider[] colliders = FindCollidersWithinRange(radius, layerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag(primaryTag))
            {
                // if there are no sub tags, then this is a primary tag check only, so we can just return true.
                if (subTag == string.Empty)
                {
                    return true;
                }

                // check the sub tags if we have been provided one.

                SubTagSystem subTagSystem = collider.GetComponent<SubTagSystem>();

                if (subTagSystem == null) continue;

                if (subTagSystem.CompareSubTag(subTag))
                {
                    return true;
                }

                // continue to the next object.
            }
        }

        return false;
    }

    /// <summary>
    /// This is literally a Physics.OverlapSphere.
    /// </summary>
    /// <param name="radius">The radius for the sphere check.</param>
    /// <returns>The colliders that were inside the check.</returns>
    public Collider[] FindCollidersWithinRange(float radius, int layerMask = Physics.AllLayers)
    {
        return Physics.OverlapSphere(transform.position, radius, layerMask, QueryTriggerInteraction.Collide);
    }
}
