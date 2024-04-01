using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAnim : MonoBehaviour
{
    float time;
    Text myText;

    void Awake()
    {
        myText = GetComponent<Text>();
    }
    void Update()
    {
        TextAnim();
    }

    void TextAnim()
    {
        if(time < 0.5f)
        {
            myText.color = new Color(1, 1, 1, 1 - time);
        }
        else
        {
            myText.color = new Color(1, 1, 1, time);
            if (time > 1f)
                time = 0;
        }

        time += Time.deltaTime;
    }
}
