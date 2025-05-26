using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Spawns numbers around the entity to display the damage it took.
/// </summary>
public class FloatingTextSystem : MonoBehaviour
{
    public GameObject magicNumberPrefab;

    public float MinDistance = 1.2f;
    public float MaxDistance = 1.5f;

    public float MaxRotationOffsetHorizontal = 120f;
    public float MaxRotationOffsetVertical = 60f;

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        //StartCoroutine(spawnNum());
    }

    void Update()
    {

    }

    IEnumerator spawnNum()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.1f);
            SpawnText("test", Color.cyan);
        }
    }

    /// <summary>
    /// Spawns a magic floating number.
    /// </summary>
    /// <param name="text">The message or number to display.</param>
    /// <param name="textColor">The colour for the text.</param>
    /// <param name="textSize">How large the text should be.</param>
    public void SpawnText(string text, Color textColor, float textSize = 10f)
    {
        // print(textSize);
        // we need to spawn the numbers but based on the camera's position, spawn 180 radius in front towards the camera.
        // So some randomness needs to happen.

        Vector3 directionTowardsCamera = (cameraTransform.position - transform.position).normalized;
        directionTowardsCamera.y = 0f;

        Vector3 horizontalDisplacement = Quaternion.AngleAxis(Random.Range(-MaxRotationOffsetHorizontal, MaxRotationOffsetHorizontal), transform.up) * directionTowardsCamera;
        Vector3 verticalDisplacement = Quaternion.AngleAxis(Random.Range(-MaxRotationOffsetVertical, MaxRotationOffsetVertical), transform.right) * directionTowardsCamera;

        Vector3 targetSpawnPoint = transform.position + (horizontalDisplacement + verticalDisplacement) * Random.Range(MinDistance, MaxDistance);

        GameObject spawnedNumber = Instantiate(magicNumberPrefab, targetSpawnPoint, Quaternion.identity);

        // then we can do cool UI stuff.
        TMP_Text textComp = spawnedNumber.GetComponent<TMP_Text>();

        textComp.text = text;

        textComp.color = textColor;

        textComp.fontSize = textSize;
    }
}
