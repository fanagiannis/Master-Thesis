using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    [SerializeField] private FootstepsData sounds;
    [SerializeField] private float fadeOutDuration = 0.2f; // Duration to fade out the sound
    private AudioSource audioSource;
    private bool isPlayingFootsteps = false;
    //private Player player;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //player = GetComponentInParent<Player>(); // Cache the player reference
    }

    void Update()
    {
        // if (player.Data.Velocity() != Vector3.zero )
        // {
        //     if (!isPlayingFootsteps) 
        //     {
        //         StartCoroutine(Footsteps());
        //     }
        // }
        // else
        // {
        //     if (audioSource.isPlaying && !isPlayingFootsteps)
        //     {
        //         StartCoroutine(FadeOutSound());
        //     }
        // }
    }

    // private IEnumerator Footsteps()
    // {
    //     // isPlayingFootsteps = true;
    //     // while (player.Data.Velocity() != Vector3.zero)
    //     // {
    //     //     if(player.HasJumped()){break;}
    //     //     audioSource.volume = 0.1f;
    //     //     audioSource.clip = sounds.RandomSound();
    //     //     audioSource.Play();
    //     //     yield return new WaitForSeconds(0.4f);
    //     // }
    //     // isPlayingFootsteps = false; 
    // }

    private IEnumerator FadeOutSound()
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}