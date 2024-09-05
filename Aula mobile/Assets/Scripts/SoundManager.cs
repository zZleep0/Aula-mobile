using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> sounds;

    public enum SoundType
    {
        TypeGameOver, //jingle
        TypePop,
        TypeSelect,
        TypeMove //swish
    }

    public static SoundManager instance;
    AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType clipType)
    {
        audioSource.PlayOneShot(sounds[(int)clipType]);
    }
}
