using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public AudioSource audioSource { get; private set; }

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
}