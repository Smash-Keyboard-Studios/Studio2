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

public class LoadModularSceneSystem : MonoBehaviour
{

	/// <summary>
	/// All the scene object to load. uses the name of the objects.
	/// </summary>
	[SerializeField]
	public UnityEngine.Object[] additiveScenes;

	void Awake()
	{
		// iterate through all the objects in the array and attempts to load them additive.
		foreach (UnityEngine.Object scene in additiveScenes)
		{
			try
			{
				SceneManager.LoadSceneAsync(scene.name, LoadSceneMode.Additive);
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex.Message, scene);
			}

		}

	}
}
