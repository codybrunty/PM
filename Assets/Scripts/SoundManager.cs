using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour {

    public Sound[] sounds;
    public static SoundManager instance;
    public int currentMusicTrack;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);


        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start() {
        StartGameMusic();    
    }

    public void StartGameMusic() {
        currentMusicTrack = UnityEngine.Random.Range(1, 3);
        if (currentMusicTrack == 1) {
            PlaySound("Music1");
        }
        if (currentMusicTrack == 2) {
            PlaySound("Music2");
        }
    }

    public void StopGameMusic() {
        if (currentMusicTrack == 1) {
            Sound s = Array.Find(sounds, sound => sound.name == "Music1");
            if (s == null) {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.source.Stop();
        }
        if (currentMusicTrack == 2) {
            Sound s = Array.Find(sounds, sound => sound.name == "Music2");
            if (s == null) {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.source.Stop();
        }
    }

    public void PlaySound(string name) {
        int soundOn = FindObjectOfType<SoundButtonMechanics>().soundOn;

        if (soundOn == 1) {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null) {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.source.Play();
        }
    }

    public void PlayOneShotSound(string name) {
        int soundOn = FindObjectOfType<SoundButtonMechanics>().soundOn;

        if (soundOn == 1) {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null) {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.source.PlayOneShot(s.source.clip);
        }
    }

}
