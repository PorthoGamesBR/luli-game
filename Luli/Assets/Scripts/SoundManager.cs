using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    //VERSÃO 2.0.0

    AudioSource AuSource;
    public AudioClip[] musics;

    AudioClip playingNow;

    void Start()
    {
        SoundManager[] soundManagers = GameObject.FindObjectsOfType<SoundManager>();
        if(soundManagers.Length > 1){
            Destroy(gameObject);
        }

        AuSource = GetComponent<AudioSource>();
        //PlayMusicList(Random.Range(0, musics.Length), 0.7f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayMusic(AudioClip music, float volume)
    { 
        if(music == null){
            AuSource.Stop();
        }
        AuSource = GetComponent<AudioSource>();
        AuSource.clip = music;
        AuSource.volume = volume;
        AuSource.Play();
    }

    public AudioClip GetMusic() 
    {
        AuSource = GetComponent<AudioSource>();
        return AuSource.clip;
    }

    public void PlayMusicList(int musicNumber, float volume)
    {
        AuSource = GetComponent<AudioSource>();
        AuSource.clip = musics[musicNumber];
        AuSource.volume = volume;
        AuSource.Play();
    }

    public float getVolume(){
        return AuSource.volume;
    }

    public void setVolume(float set){
        AuSource.volume = set;
    }

    public void PlaySound(AudioClip sound, float volume)
    {

            AuSource.PlayOneShot(sound, volume);


    }

    public void PauseMusic(bool pause)
    {
        if (pause)
        { AuSource.Pause(); }
        else
        { AuSource.UnPause(); }
    }

    public void SetPitch(float pitch)
    {
        AuSource.pitch = pitch;
    }
    public void StopMusic()
    {
        AuSource.Stop();
    }

    public void StopAllAudio()
    {
        AuSource.volume = 0;
    }

    public static SoundManager getSoundManager()
    {
        SoundManager sm = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        return sm;
    }


}
