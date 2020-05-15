using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SfxType
{
    Walking,
    Jump1,
    Jump2,
    Attacking,
    Hurt
}

public enum MusicType
{
    Level1Background,
    Level2Background
}

public enum AuidoSourceName
{
    _sfx,
    _attack
}


public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<SfxAudioData> _sfxAudioDataList = new List<SfxAudioData>();
    [SerializeField] private List<MusicAudioData> _musicAudioDataList = new List<MusicAudioData>();
    

    [SerializeField] public AudioSource movementAudioSource;
    [SerializeField] public AudioSource jumpAudioSource;
    [SerializeField] public AudioSource attackAudioSource;
    [SerializeField] public AudioSource hurtAudioSource;
    [SerializeField] public AudioSource deathAudioSource;
    [SerializeField] private AudioSource _music;

    public static AudioManager _audioManagerInner;

    public static void PlaySfx(SfxType sfxType, AudioSource audiosSource)
    {
        _audioManagerInner.PlaySfxInner(sfxType, audiosSource);
    }

    public void PlaySfxInner(SfxType sfxType, AudioSource audioSource)
    {
        var audioData = GetSfxAudioData(sfxType);
        if (audioData == null)
            return;
        audioSource.PlayOneShot(audioData.Clip);
    }

    public void PlayMusic(MusicType musicType)
    {

    }

    private void Awake()
    {
        if (_audioManagerInner != null)
            return;

        _audioManagerInner = this;
    }
    void Start()
    {

    }

    private SfxAudioData GetSfxAudioData(SfxType sfxType)
    {
        var result = _sfxAudioDataList.Find(sfx => sfx.Type == sfxType);
        return result;
    }

    //private AudioSource GetAudioSourceByName(AuidoSourceName audioSourceName)
    //{
    //    var result = _sfxAudioDataList.Find(audioSource => audioSource.Name == audioSourceName);
    //    return result;
    //}


    void Update()
    {

    }


    public static void StopPlayingSfx(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public static void PausePlaingSfx(AudioSource audioSource)
    {
        audioSource.Pause();
    }

    public static bool IsPlayingSfx(AudioSource audioSource)
    {
        if (audioSource.isPlaying) 
            return true;
        else 
            return false;
    }
    public static void PlayWalkingSfx()
    {
        _audioManagerInner.movementAudioSource.volume = 0.2f;
        _audioManagerInner.movementAudioSource.pitch = 1.3f;
        PlaySfx(SfxType.Walking, _audioManagerInner.movementAudioSource);
    }

    public static void PlayJumpSfx(AudioSource audioSource)
    {
        audioSource.pitch = 1f;
        System.Random rnd = new System.Random();
        int randSfx = rnd.Next(1, 3);
        if (randSfx == 1)
            PlaySfx(SfxType.Jump1, audioSource);
        else if (randSfx == 2)
            PlaySfx(SfxType.Jump2, audioSource);
    }


}

[Serializable]
public class SfxAudioData
{
    public SfxType Type;
    public AudioClip Clip;
}

[Serializable]
public class MusicAudioData
{
    public MusicType Type;
    public AudioClip Clip;
}

public class AudioSourceName
{
    public AuidoSourceName Name;
}



