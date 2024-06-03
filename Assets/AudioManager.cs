using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip background1;
    public AudioClip background2;
    public AudioClip background3;
    public AudioClip popin;
    public AudioClip popout;
    public AudioClip lightswitch;
    public AudioClip buzzing;
    public AudioClip LeverOn;
    public AudioClip LeverOff;
    public AudioClip DoorOpen;

    void Start()
    {
        PlayRandomBackgroundMusic();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlaySFXinLoop(AudioClip clip)
    {
        SFXSource.clip = clip;
        SFXSource.loop = true;
        SFXSource.Play();
    }

    void PlayRandomBackgroundMusic()
    {
        // Generate random number between 1 and 3
        int random = Random.Range(1, 4);

        // Play background music based on random number
        switch (random)
        {
            case 1:
                MusicSource.clip = background1;
                break;
            case 2:
                MusicSource.clip = background2;
                break;
            case 3:
                MusicSource.clip = background3;
                break;
        }

        MusicSource.Play();
    }

    void Update()
    {
        // Check if the current background music has finished playing
        if (!MusicSource.isPlaying)
        {
            // Play another random background music
            PlayRandomBackgroundMusic();
        }
    }

}
