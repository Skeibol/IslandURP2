using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
  public static AudioManager instance;
  public AudioSource AmbianceAudioSource;
  public AudioSource MusicAudioSource;

  public List<SoundClip> hitSounds;
  public List<SoundClip> ambianceSounds;

  // Start is called before the first frame update
  private void Awake()
  {
    if (instance == null) {
      instance = this;
    }
    else if (instance != this) {
      Destroy(this);
    }

    DontDestroyOnLoad(this);
  }

  void Start()
  {
    AmbianceAudioSource.Play();
    MusicAudioSource.Play();
  }

  public void playClip(string name)
  {
    // HitsAudioSource.clip = findClipByName(name);
    // HitsAudioSource.Play();
  }

  public AudioClip findClipByName(string name)
  {
    foreach (SoundClip sound in hitSounds) {
      if (sound.Name == name) {
        return sound.clip;
      }
    }

    Debug.LogWarning("Clip not found");


    return null;
  }

  public void startDay()
  {
    StopAllCoroutines();
    StartCoroutine(AmbianceStartDay());
  }

  public void startNight()
  {
    StopAllCoroutines();
    StartCoroutine(AmbianceStartNight());
  }

  public IEnumerator AmbianceStartDay()
  {
    while (AmbianceAudioSource.volume > 0f) {
      AmbianceAudioSource.volume -= 0.01f;
      yield return new WaitForSeconds(0.1f);
    }

    AmbianceAudioSource.clip = getAmbianceByName("day");
    AmbianceAudioSource.Play();
    while (AmbianceAudioSource.volume < 0.6f) {
      AmbianceAudioSource.volume += 0.01f;
      if (MusicAudioSource.volume < 0.10f) {
        MusicAudioSource.volume += 0.01f;
      }
      yield return new WaitForSeconds(0.1f);
    }
  }

  public IEnumerator AmbianceStartNight()
  {
    while (AmbianceAudioSource.volume > 0f) {
      AmbianceAudioSource.volume -= 0.01f;
      yield return new WaitForSeconds(0.1f);
    }

    AmbianceAudioSource.clip = getAmbianceByName("night");
    AmbianceAudioSource.Play();
    while (AmbianceAudioSource.volume < 0.3f) {
      AmbianceAudioSource.volume += 0.01f;
      if (MusicAudioSource.volume > 0.03f) {
        MusicAudioSource.volume -= 0.01f;
      }

      yield return new WaitForSeconds(0.1f);
    }
  }

  private AudioClip getAmbianceByName(string name)
  {
    try {
      return ambianceSounds.First(o => o.Name == name).clip;
    }
    catch (Exception e) {
      Debug.Log(e);
      return null;
    }
  }
}