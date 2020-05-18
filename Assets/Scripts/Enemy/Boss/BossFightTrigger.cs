using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _bossTeleportEffect;
    [SerializeField] private GameObject _invisibleWallOne;
    [SerializeField] private GameObject _invisibleWallTwo;
    [SerializeField] private GameObject _bossItself;
    [SerializeField] private AudioSource _battleMusicAudioSource;
    [SerializeField] private AudioClip _battleMusic;
    [SerializeField] private GameObject _woundedCollider;

    private Collider2D _currentCollider;
    private bool _isAllowedToPlayMusic = false;

    private void Start()
    {
        _currentCollider = GetComponent<Collider2D>();
    }

    public void TriggerBossFight()
    {
        StartCoroutine("ActivateInvisibleWalls");
        StartBossFight();
        _currentCollider.enabled = false;
    }


    public void DeactivateInvisibleWalls()
    {
        _invisibleWallOne.SetActive(false);
        _invisibleWallOne.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            TriggerBossFight();
        }
    }

    private IEnumerator ActivateInvisibleWalls()
    {
        yield return new WaitForSeconds(1.0f);
        _invisibleWallOne.SetActive(true);
        _invisibleWallOne.SetActive(true);
    }

    private void StartBossFight()
    {
        

        _isAllowedToPlayMusic = true;

        // контроль громкости почему-то не работает
        _battleMusicAudioSource.volume = 0.2f;
        //for (float i = 0; i <= 0.1; i += 0.0001f)
        //{
        //    _battleMusicAudioSource.volume += i;
        //    if (_battleMusicAudioSource.volume.Equals(0.1f))
        //        return;
        //}

        StartCoroutine("SpawnBoss");
    }

    private IEnumerator SpawnBoss()
    {
        _bossTeleportEffect.SetActive(true);
        yield return new WaitForSeconds(1.8f);
        AudioManager.PlayBossAppearSfx(AudioManager._audioManagerInner.bossSFXAudioSource);
        _bossItself.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        _bossTeleportEffect.SetActive(false);
        _woundedCollider.SetActive(false);
    }

    public void EndBossFight()
    {
        for (float i = 0.1f; i > 0; i -= 0.0001f)
        {
            _battleMusicAudioSource.volume -= i;
        }
    }


    private void Update()
    {
        if (!AudioManager.IsPlayingSfx(_battleMusicAudioSource) && _isAllowedToPlayMusic)
        {
            _battleMusicAudioSource.PlayOneShot(_battleMusic, _battleMusicAudioSource.volume);
        }
    }

}
