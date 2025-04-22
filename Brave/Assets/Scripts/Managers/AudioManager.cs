using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    public int bgmIndex;

    private bool canPlaySFX;

    private  float[] defaultVolume;

    private bool stopCoroutine = false;

    private void Start()
    {
        defaultVolume = new float[sfx.Length];

        for (int i = 0; i < sfx.Length; i++)
        {
            defaultVolume[i] = sfx[i].volume;
        }
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        Invoke("AllowSFX", 1f);
    }


    private void Update()
    {
        if(!playBgm)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                //PlayBGM(bgmIndex);
                PlayRandomBGM();
        }
    }
    public void PlaySFX(int _sfxIndex,Transform _source)
    {
        if (canPlaySFX == false)
            return;

        if(_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance)
            return;

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            if(_sfxIndex == 17)
                stopCoroutine = true;
            sfx[_sfxIndex].volume = defaultVolume[_sfxIndex];
            sfx[_sfxIndex].Play();
        }
        
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index],_index));

    private IEnumerator DecreaseVolume(AudioSource _audio,int _index)
    {
        stopCoroutine = false;

        while (_audio.volume > .1f)
        {   
           
            if (stopCoroutine)
            {
                break;
            }
           
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.6f);

            if (_audio.volume < .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume[_index];
                break;
            }

        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        //Debug.Log("Playing BGM " + bgmIndex);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for(int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    public void StopAllSFX()
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            sfx[i].Stop();
        }
    }

    private void AllowSFX() => canPlaySFX = true;
}
