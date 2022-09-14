using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowFilter : MonoBehaviour
{
    float opacity;
    Color normColor;
    private void Start()
    {
        normColor = GetComponent<Image>().color;
    }

    private void FixedUpdate()
    {
        float time = TimeManager.worldTime;

        if(opacity != (1- time))
        {
            opacity = (1 - time);
            Color temp = new Color(normColor.r, normColor.g, normColor.b, opacity);
            GetComponent<Image>().color = temp;
        }
    }
}
