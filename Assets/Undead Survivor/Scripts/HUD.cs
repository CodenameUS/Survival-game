using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                // .. 현재경험치 / 최대경험치
                float curExp = GameManager.instance.exp;
                float nextExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                mySlider.value = curExp / nextExp;
                break;
            case InfoType.Level:
                // .. {0} 의미 : 인자 값의 문자열이 들어갈 자리를 {0}번째.
                myText.text = string.Format("Lv.{0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                // .. 남은시간
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);        // .. 분 계산
                int sec = Mathf.FloorToInt(remainTime % 60);        // .. 초 계산
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                // .. 현재체력 / 최대경험치
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
        }    
    }
}
