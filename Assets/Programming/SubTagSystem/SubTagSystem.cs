using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|
// © 2025 Dominic McNeill dommcneill@outlook.com
// Licensed for use within the Wraiths of Retail by Smash Keyboard Studios (SKS) only.
// Redistribution or modification outside of this project is prohibited without explicit written permission.
// For full license terms, see DOMIBRON_CODE_LICENSE.md at the project root.


public class SubTagSystem : MonoBehaviour
{
    public string[] subTags;


    /// <summary>
    /// Checks to see if the tag you are looking for are in the sub tag list.
    /// </summary>
    /// <param name="tag">The tag you are looking for.</param>
    /// <returns>True if the tag was found in the sub tags.</returns>
    public bool CompareSubTag(string tag)
    {
        return subTags.Contains(tag);
    }
}
