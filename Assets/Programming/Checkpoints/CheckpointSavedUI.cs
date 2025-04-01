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


public class CheckpointSavedUI : MonoBehaviour
{
    [SerializeField]
    private float duration = 2f;

    private float timer = 0f;

    [SerializeField]
    private float rotationSpeed = 4f;

    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private Image saveIcon;

    void Update()
    {
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

        saveIcon.color = new Color(1, 1, 1, Mathf.Clamp(Mathf.Sin((timer / duration) * Mathf.PI), 0, 1));


        if (timer > 0)
        {
            rectTransform.Rotate(0, 0, (rotationSpeed * 360f) * Time.deltaTime);
        }
    }

    public void DisplayIcon()
    {
        timer = duration;
    }
}
