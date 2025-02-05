using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|

/// <summary>
/// Mesh for AI swipe attack.
/// </summary>
public static class AttackSwishMesh
{
	public static Vector3[] verticies =
	{
		new (0,0.1f,0), // centre.
		new (-0.5f, 0.1f, -0.5f),
		new (-1, 0.1f, 0),
		new (-0.7f, 0.1f, 0.7f),
		new (0, 0.1f, 1), // forward centre.
		new (0.7f, 0.1f, 0.7f),
		new (1, 0.1f, 0),
		new (0.5f, 0.1f, -0.5f),
		
		// second layer
		new (0,-0.1f,0), // centre.
		new (-0.5f, -0.1f, -0.5f),
		new (-1, -0.1f, 0),
		new (-0.7f, -0.1f, 0.7f),
		new (0, -0.1f, 1), // forward centre.
		new (0.7f, -0.1f, 0.7f),
		new (1, -0.1f, 0),
		new (0.5f, -0.1f, -0.5f),
	};

	public static Vector3[] normalsVectors =
	{
		new (0,1,0), // centre.
		new (0, 1, 0),
		new (0, 1, 0),
		new (0, 1, 0),
		new (0, 1, 0), // forward centre.
		new (0, 1, 0),
		new (0, 1, 0),
		new (0, 1, 0),
		
		// second layer
		new (0, -1, 0), // centre.
		new (0, -1, 0),
		new (0, -1, 0),
		new (0, -1, 0),
		new (0, -1, 0), // forward centre.
		new (0, -1, 0),
		new (0, -1, 0),
		new (0, -1, 0),
	};


	public static int[] triangles =
	{
		// top faces
		0,1,2,
		0,2,3,
		0,3,4,
		0,4,5,
		0,5,6,
		0,6,7,
		
		//side faces
		0,9,1,
		0,8,9,

		1,10,2,
		1,9,10,

		2,11,3,
		2,10,11,

		4,3,11,
		4,11,12,

		5,4,12,
		5,12,13,

		6,5,13,
		6,13,14,

		7,6,14,
		7,14,15,

		0,7,15,
		0,15,8,
		
		// bottom face
		8,15,14,
		8,14,13,
		8,13,12,
		8,12,11,
		8,11,10,
		8,10,9,
	};
}
