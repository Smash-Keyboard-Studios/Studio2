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


    public float entityDetectionRangeRadius = 20f;

    // private bool enemyDetected = false;
    // private bool itemDetected = false;

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
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        HandleEntityDetection();

        // dangerLightIndicatorImage.sprite = (dangerLightIsOn) ? dangerLightOn : dangerLightOff;
        // itemLightIndicatorImage.sprite = (itemLightIsOn) ? itemLightOn : itemLightOff;

        HandleLightBlinking();

        HandlePointer();
    }

    private void HandleEntityDetection()
    {
        Collider[] results = Physics.OverlapSphere(playerTransform.position, entityDetectionRangeRadius, Physics.AllLayers, QueryTriggerInteraction.Collide);

        if (results.Length > 0)
        {
            bool enemy = false;
            bool item = false;

            foreach (Collider c in results)
            {
                if (c.gameObject.CompareTag("Item"))
                {
                    item = true;
                }
                else if (c.gameObject.CompareTag("Enemy"))
                {
                    enemy = true;
                }
            }

            dangerLightIsOn = enemy;
            itemLightIsOn = item;
        }
        else
        {
            dangerLightIsOn = false;
            itemLightIsOn = false;
        }
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
            Vector3 dir = (targetPoint.Value - playerTransform.position).normalized;

            // 1 is forward, 0 is left or right, and -1 is back.
            float rotationAmount = Vector2.Dot(Vector2.up, new Vector2(dir.x, dir.z));

            bool isRight = Vector2.Dot(Vector2.right, new Vector2(dir.x, dir.z)) >= 0;

            float rotation = (90f - (90f * rotationAmount)) * (isRight ? -1f : 1f);

            pointerImage.rotation = Quaternion.Euler(0, 0, rotation);
        }
        else
        {
            pointerImage.gameObject.SetActive(false);

            pointerImage.Rotate(0, 0, 50 * Time.deltaTime);
        }
    }
}
