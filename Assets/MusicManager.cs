using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance {  get; private set; }
    public AudioSource audioSource { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        audioSource = GetComponent<AudioSource>();
    }
}