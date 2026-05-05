using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;


    public AudioClip musicClip;
    public AudioClip coinClip;
    public AudioClip winClip;
    public AudioClip failClip;
    public AudioClip breakClip;

    private void Start()
    {
        //if (SettingUI.Instances.IsOnSound ?? false)
        //{
        //    AudioListener.pause = false;
        //}
        //else
        //{
        //    AudioListener.pause = true;
        //}
        musicAudioSource.clip = musicClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlaySfx(AudioClip sfxClip)
    {
        //if (SettingUI.Instances.IsOnSound ?? false)
        //{
        //    AudioListener.pause = false;
        //}
        //else
        //{
        //    AudioListener.pause = true;
        //}
        vfxAudioSource.clip = sfxClip;
        vfxAudioSource.PlayOneShot(sfxClip);
    }
}
