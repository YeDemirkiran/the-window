using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public AudioSource audioSource { get; private set; }

    public AudioMixer audioMixer;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
}