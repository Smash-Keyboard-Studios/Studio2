using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|


public class SetAIState : MonoBehaviour
{
    public AIState desiredState = AIState.Alerted;

    public AIBase targetAI;

    public void SetTheAIState()
    {
        if (targetAI == null) throw new Exception("Please add a reference to the target AI");

        targetAI.ChangeState(desiredState);
    }
}
