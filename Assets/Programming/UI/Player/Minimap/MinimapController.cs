using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

public class MinimapController : MonoBehaviour
{
    public static MinimapController instance; // so we can set target point.

    public Image dangerLightIndicatorImage;
    public Sprite dangerLightOn;
    public Sprite dangerLightOff;
    public bool dangerLightIsOn = false;

    public Image itemLightIndicatorImage;
    public Sprite itemLightOn;
    public Sprite itemLightOff;
    public bool itemLightIsOn = false;

    private float blinkTime = 0f;

    public float blinkRate = 4f;

    public RectTransform pointerImage;

    public Vector3? targetPoint = null;

    public RectTransform SweepImage;

    public float rotationSpeed = 40f;

    public float entityDetectionRangeRadius = 20f;

    public float PointerRotationSpeed = 20f;

    // private bool enemyDetected = false;
    // private bool itemDetected = false;

    private EntityDetectNearby entityDetectNearby;
    private Transform playerTransform;



    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = PlayerReferenceFetcher.instance.GetPlayerReference().transform;
        entityDetectNearby = playerTransform.GetComponent<EntityDetectNearby>();
    }

    // Update is called once per frame
    void Update()
    {
        SweepImage.Rotate(0, 0, rotationSpeed * Time.deltaTime);


        GetLightStatus();

        // dangerLightIndicatorImage.sprite = (dangerLightIsOn) ? dangerLightOn : dangerLightOff;
        // itemLightIndicatorImage.sprite = (itemLightIsOn) ? itemLightOn : itemLightOff;



        HandleLightBlinking();

        HandlePointer();
    }

    private void GetLightStatus()
    {
        // layer mask checking so we can optimise physics a little.
        dangerLightIsOn = entityDetectNearby.CheckWithinRadius(entityDetectionRangeRadius, "Enemy", layerMask: LayerMask.GetMask("Enemy"));
        itemLightIsOn = entityDetectNearby.CheckWithinRadius(entityDetectionRangeRadius, "Item", layerMask: LayerMask.GetMask("Item"));
    }

    private void HandleLightBlinking()
    {
        if (dangerLightIsOn || itemLightIsOn)
        {
            blinkTime += Time.deltaTime * blinkRate;
        }
        else
        {
            blinkTime = 0f;
        }

        if (dangerLightIsOn)
        {
            // add 2f so blinking will start faster.
            dangerLightIndicatorImage.sprite = (Mathf.Sin(blinkTime) > 0) ? dangerLightOn : dangerLightOff;
        }
        else
        {
            dangerLightIndicatorImage.sprite = dangerLightOff;
        }

        if (itemLightIsOn)
        {
            itemLightIndicatorImage.sprite = (Mathf.Sin(blinkTime) > 0) ? itemLightOn : itemLightOff;
        }
        else
        {
            itemLightIndicatorImage.sprite = itemLightOff;
        }
    }

    private void HandlePointer()
    {
        // left is positive, right is negative.
        if (targetPoint.HasValue)
        {
            pointerImage.gameObject.SetActive(true);

            Vector3 dir = (targetPoint.Value - playerTransform.position).normalized;

            // 1 is forward, 0 is left or right, and -1 is back.
            float rotationAmount = Vector2.Dot(Vector2.up, new Vector2(dir.x, dir.z));

            bool isRight = Vector2.Dot(Vector2.right, new Vector2(dir.x, dir.z)) >= 0;

            float rotation = (90f - (90f * rotationAmount)) * (isRight ? -1f : 1f);

            pointerImage.rotation = Quaternion.Lerp(pointerImage.rotation, Quaternion.Euler(0, 0, rotation), PointerRotationSpeed * Time.deltaTime);
        }
        else
        {
            pointerImage.gameObject.SetActive(false);

            pointerImage.Rotate(0, 0, 50 * Time.deltaTime);
        }
    }
}
