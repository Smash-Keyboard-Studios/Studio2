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

/// <summary>
/// Handles displaying a hit checkpoint icon.
/// </summary>
public class CheckpointSavedUI : MonoBehaviour
{
    /// <summary>
    /// How long the icon will be displayed.
    /// </summary>
    [SerializeField]
    private float duration = 2f;

    /// <summary>
    /// Local variable to keep track of how long the icon has been displayed.
    /// </summary>
    private float timer = 0f;

    /// <summary>
    /// How many revolutions per second.
    /// </summary>
    [SerializeField]
    private float rotationSpeed = 4f;

    /// <summary>
    /// The icon to spin.
    /// </summary>
    [SerializeField]
    private RectTransform rectTransform;

    /// <summary>
    /// The icon image to fade in and out for the animation.
    /// </summary>
    [SerializeField]
    private Image saveIcon; // this seems obsolete as you can get this from the rect transform.

    void Update()
    {
        // handles the timer and resetting.
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            rectTransform.gameObject.SetActive(true);
        }
        else
        {
            rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            rectTransform.gameObject.SetActive(false);
        }

        // handles fading in and out.
        saveIcon.color = new Color(1, 1, 1, Mathf.Clamp(Mathf.Sin((timer / duration) * Mathf.PI), 0, 1));

        // handles rotating the save icon.
        if (timer > 0)
        {
            rectTransform.Rotate(0, 0, (rotationSpeed * 360f) * Time.deltaTime);
        }
    }

    /// <summary>
    /// Call this to display the save icon.
    /// </summary>
    public void DisplayIcon()
    {
        timer = duration;
    }
}
