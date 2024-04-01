using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] titles;

    public void Lose()
    {
        // .. ÆÐ¹è Title
        titles[0].SetActive(true);
    }

    public void Win()
    {
        // .. ½Â¸® Title
        titles[1].SetActive(true);
    }
}
