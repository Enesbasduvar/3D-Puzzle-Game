using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonNotes : MonoBehaviour
{
    [SerializeField] AudioSource NoteSource;

    [Header("Simon Notes")]
    public AudioClip simonR;
    public AudioClip simonG;
    public AudioClip simonB;
    public AudioClip simonY;

    public void PlaySFX(AudioClip clip)
    {
        NoteSource.PlayOneShot(clip);
    }
}
