using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager> {

	public bool MusicActive { get; private set; }
	public bool SoundActive { get; private set; }

    public AudioSource MusicAudioSource { get; private set; }

    public AudioSource SoundAudioSource { get; private set; }

    public Queue<AudioSource> EffectAudioSourceQueue { get;private set; }


    const string MUSIC_ACTIVE_KEY = "MusicActive";
    const string SOUND_ACTIVE_KEY = "SoundActive";
    const int MAX_EFFECT_AUDIOSOURCE = 10;

    public void SetDontDestroyOnLoad(Transform parent)
    {
        transform.SetParent(parent);    
        if (transform.childCount==0)
        {
            GameObject musicSource = new GameObject("MusicSource");
            GameObject soundSource = new GameObject("SoundSource");
            GameObject effectSource = new GameObject("EffectSource");

            musicSource.transform.SetParent(transform);
            soundSource.transform.SetParent(transform);
            effectSource.transform.SetParent(transform);



            MusicAudioSource = musicSource.AddComponent<AudioSource>();
            MusicAudioSource.loop = true;
            MusicAudioSource.playOnAwake = false;

            SoundAudioSource = soundSource.AddComponent<AudioSource>();
            SoundAudioSource.loop = false;
            SoundAudioSource.playOnAwake = false;
       

        int musicActive = PlayerPrefs.GetInt(MUSIC_ACTIVE_KEY, 1);
        int soundActive = PlayerPrefs.GetInt(SOUND_ACTIVE_KEY, 1);
        MusicActive = musicActive == 1 ? true:false;
        SoundActive = soundActive == 1 ? true : false;

        EffectAudioSourceQueue=new Queue<AudioSource>();

        for (int i = 0; i < MAX_EFFECT_AUDIOSOURCE; i++)
        {
            AudioSource ad = effectSource.AddComponent<AudioSource>();
            EffectAudioSourceQueue.Enqueue(ad);
        }
        }
    }

    public void StopMusic()
    {
        if (MusicAudioSource==null)
        {
            return;
        }
        MusicAudioSource.gameObject.SetActive(false);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (MusicActive==false)
        {
         return;
        }
        if (MusicAudioSource==null)
        {
           return;
        }
        if (clip==null)
        {
            return;
        }
        if (MusicAudioSource.isPlaying&&MusicAudioSource.clip.name==clip.name)
        {
            return;
        }
        MusicAudioSource.gameObject.SetActive(true);
        MusicAudioSource.clip = clip;
        MusicAudioSource.loop = true;
        MusicAudioSource.Play();

    }


    public void PlaySound(AudioClip clip)
    {
        if (SoundActive==false)
        {
            return;
        }
        if (SoundAudioSource==null)
        {
            return;
        }
        SoundAudioSource.Stop();
        SoundAudioSource.clip = clip;
        SoundAudioSource.volume = 1f;
        SoundAudioSource.PlayOneShot(clip);
    }

    public void PlaySound(string path)
    {
        AudioClip clip =GameTools.Load<AudioClip>(path);///////////
        PlaySound(clip);
    }

    public void SetMusicActive(bool active)
    {
        if (MusicActive == active)
        {
            return;
        }
        MusicActive = active;
        PlayerPrefs.SetInt(MUSIC_ACTIVE_KEY,active?1:0);
        if (MusicAudioSource==null)
        {
            return;
        }
        MusicAudioSource.gameObject.SetActive(active);
    }


    public void SetSoundActive(bool active)
    {
        if (SoundActive==active)
        {
            return;
        }
        SoundActive = active;
        PlayerPrefs.SetInt(SOUND_ACTIVE_KEY,active?1:0);
    }

    public AudioSource PlayEffectAudio(AudioClip clip)
    {
        if (clip==null)
        {
            return null;
        }
        if (SoundActive==false)
        {
            return null;
        }
        AudioSource audio = EffectAudioSourceQueue.Dequeue();
        audio.clip = clip;
        audio.volume = 1f;
        audio.Play();
        EffectAudioSourceQueue.Enqueue(audio);
        return audio;
    }

    public void PlayEffectAudioAtPoint(AudioClip clip)
    {
        if (clip==null)
        {
            return;
        }
        if (SoundActive==false)
        {
            return;
        }
        AudioSource audio = EffectAudioSourceQueue.Dequeue();
        audio.clip = clip;
        audio.volume = 1f;
        audio.Play();
        EffectAudioSourceQueue.Enqueue(audio);

    }

  

}
