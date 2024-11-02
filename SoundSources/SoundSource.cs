using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    [SerializeField]private float volume;
    [SerializeField]private AudioSource audioSource;
    [SerializeField]private bool played=true;
    private float timer=0.2f;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<SphereCollider>().radius = volume;
    }
    void Update()
    {
        CastSound();
    }

    public bool Sound()
    {
        return played;
    }

    public void CastSound()
    {
        if (!played)
        {
            timer-=Time.deltaTime;
            if(timer<=0)
            {
                PlaySound();
                played=true;
                timer=0.2f;
            }
            
        }
    }

    public void PlaySound()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audioSource.clip);
    }

}
