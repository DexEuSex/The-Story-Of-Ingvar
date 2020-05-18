using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SfxType
{
    Walking,
    Jump1,
    Jump2,
    SwordAttack1,
    SwordAttack2,
    Hurt1,
    Hurt2,
    EnemyHurt1,
    EnemyHurt2,
    EnemyDeath1,
    EnemyDeath2,
    BossAppear,
    BossHurt1,
    BossHurt2,
    BossHurt3,
    BossWounded,
    Ambient
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
    [SerializeField] public AudioSource enemyHurtAudioSource;
    [SerializeField] public AudioSource bossSFXAudioSource;
    [SerializeField] private AudioSource _ambientAudioSource;

    public static AudioManager _audioManagerInner;

    private void Update()
    {
        if (!IsPlayingSfx(_ambientAudioSource))
        {
            _ambientAudioSource.volume = 0.2f;
            PlaySfx(SfxType.Ambient, _ambientAudioSource);
        }
    }


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

    public static void PlayAttackSfx(AudioSource audioSource)
    {
        audioSource.pitch = 1f;
        System.Random rnd = new System.Random();
        int randSfx = rnd.Next(1, 3);
        if (randSfx == 1)
            PlaySfx(SfxType.SwordAttack1, audioSource);
        else if (randSfx == 2)
            PlaySfx(SfxType.SwordAttack2, audioSource);
    }

    public static void PlayHurtSfx(AudioSource audioSource)
    {
        audioSource.pitch = 1f;
        audioSource.volume = 0.1f;
        System.Random rnd = new System.Random();
        int randSfx = rnd.Next(1, 3);
        if (randSfx == 1)
            PlaySfx(SfxType.Hurt1, audioSource);
        else if (randSfx == 2)
            PlaySfx(SfxType.Hurt2, audioSource);
    }

    public static void PlayEnemyHurtSfx(AudioSource audioSource)
    {
        audioSource.pitch = 1f;
        audioSource.volume = 0.1f;
        System.Random rnd = new System.Random();
        int randSfx = rnd.Next(1, 3);
        if (randSfx == 1)
            PlaySfx(SfxType.EnemyHurt1, audioSource);
        else if (randSfx == 2)
            PlaySfx(SfxType.EnemyHurt2, audioSource);
    }

    public static void PlayEnemyDeathSfx(AudioSource audioSource)
    {
        audioSource.pitch = 1f;
        System.Random rnd = new System.Random();
        int randSfx = rnd.Next(1, 3);
        if (randSfx == 1) 
            PlaySfx(SfxType.EnemyDeath1, audioSource);
        else if (randSfx == 2) 
            PlaySfx(SfxType.EnemyDeath2, audioSource);
    }

    public static void PlayBossHurtSfx(AudioSource audioSource)
    {
        audioSource.pitch = 1f;
        audioSource.volume = 0.1f;
        System.Random rnd = new System.Random();
        int randSfx = rnd.Next(1, 4);
        if (randSfx == 1)
            PlaySfx(SfxType.BossHurt1, audioSource);
        else if (randSfx == 2)
            PlaySfx(SfxType.BossHurt2, audioSource);
        else if (randSfx == 3)
            PlaySfx(SfxType.BossHurt3, audioSource);
    }

    public static void PlayBossAppearSfx(AudioSource audioSource)
    {
        audioSource.volume = 0.1f;
        PlaySfx(SfxType.BossAppear, audioSource);
    }

    public static void PlayBossWoundedSfx(AudioSource audioSource)
    {
        audioSource.volume = 0.1f;
        PlaySfx(SfxType.BossWounded, audioSource);
    }

    private void PlayAmbient(AudioSource audioSource)
    {
        audioSource.volume = 0.1f;
        PlaySfx(SfxType.Ambient, audioSource);
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



