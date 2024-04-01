using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int sfxChannels;        // .. 다량의 효과음을 내기위한 채널 개수 변수
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Bgm { Main, Ingame};
    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win}

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // .. 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClips[(int)Bgm.Main];
        bgmPlayer.Play();
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // .. 효과음 플레이어 초기회
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];  // .. 채널 개수 만큼 AudioSource 생성

        for (int index = 0;index<sfxPlayers.Length;index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void ChangeBgm(bool isIngame)
    {
        if(isIngame)
        {
            bgmPlayer.clip = bgmClips[(int)Bgm.Ingame];
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.clip = bgmClips[(int)Bgm.Main];
            bgmPlayer.Play();
        }
    }
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void StopBgm()
    {
        bgmPlayer.Stop();
    }

    public void ResumeBgm()
    {
        bgmPlayer.Play();
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }
    
    public void ButtonSound()
    {
        PlaySfx(Sfx.Select);
    }
   
} 
