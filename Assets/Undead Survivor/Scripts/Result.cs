using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] titles;

    public void Lose()
    {
        // .. �й� Title
        titles[0].SetActive(true);
    }

    public void Win()
    {
        // .. �¸� Title
        titles[1].SetActive(true);
    }
}
