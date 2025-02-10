using System;
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
/// Loads any scenes added to the array as additive on awake.
/// </summary>
public class LoadModularSceneSystem : MonoBehaviour
{

	/// <summary>
	/// All the scene object to load. uses the name of the objects.
	/// </summary>
	[SerializeField]
	public int[] additiveScenes;

	void Start()
	{
		// iterate through all the objects in the array and attempts to load them additive.
		foreach (int scene in additiveScenes)
		{
			try
			{
				SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex.Message);
			}

		}

	}
}
