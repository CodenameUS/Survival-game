using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // ... 매니저 인스턴스를 어디서든 접근 가능하게 함
    public static GameManager instance;

    [Header("# Game Object")]
    public bool isLive;
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;     // .. 게임승리 시 남은 적 처리하는 Bullet
    public Transform uiJoy;

    [Header("# Game Control")]
    // ... 게임 시간과 최대게임시간 변수
    public float gameTime;
    public float maxGameTime = 5 * 60f;

    [Header("# Player Info")]
    // ... 체력, 레벨, 킬수, 경험치, 다음레벨에 필요한 경험치
    public int playerId;
    public float health;
    public float maxHealth = 100f;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 25, 55, 130};
    

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;       // .. 60프레임설정
    }

     public void GameStart(int id)
    {
        health = maxHealth;
        playerId = id;

        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);   // .. 기본 무기 지급
        Resume();

        AudioManager.instance.ChangeBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        // .. Dead Animation을 위한 텀
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.StopBgm();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        // .. Dead Animation을 위한 텀
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.StopBgm();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        // .. 게임종료시 레벨업 안하도록
        if (!isLive)
            return;

        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        // .. 유니티 시간 속도(배율) 조절(기본 1)
        Time.timeScale = 0;

        uiJoy.localScale = Vector3.zero;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }
}
