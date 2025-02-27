using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



/// <summary>
/// Loads the level with the specified name.
/// </summary>
public class LoadNextLevel : MonoBehaviour
{
    public string nameOfLevel;

    public void LoadLevel()
    {
        SceneManager.LoadSceneAsync(nameOfLevel);
    }
}
