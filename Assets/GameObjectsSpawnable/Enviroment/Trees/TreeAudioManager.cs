using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAudioManager : MonoBehaviour
{
  public AudioSource audioSource;

  public SoundClip hitSound;
  public SoundClip fallSound;

  public GameObject fallAudioObject;

  public void Start()
  {
    audioSource = GetComponent<AudioSource>();
  }
  public void playHitSound()
  {
    audioSource.PlayOneShot(hitSound.clip);
  } 
  public void playFallSound()
  {
    AudioSource.PlayClipAtPoint(fallSound.clip,transform.position);
  }
  
  
}
