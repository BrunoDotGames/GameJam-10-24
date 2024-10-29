using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public AudioClip audioClip;
    public float Volume;
    AudioSource auido;
    public bool alreadyPlayer;

    void Start()
    {
        auido = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!alreadyPlayer)
        {
            auido.PlayOneShot(audioClip, Volume);
            alreadyPlayer = true;
        }
    }
}
