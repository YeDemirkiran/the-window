using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class CaptureProjectile : MonoBehaviour
{
    new Renderer renderer;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip shootSound;

    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public void Prepare(float duration)
    {
        StopAllCoroutines();

        StartCoroutine(PrepareIE(duration));
    }

    IEnumerator PrepareIE(float duration)
    {
        float lerp = 0f;

        Vector3 startSize = Vector3.one * 0.001f;
        Vector3 targetSize = transform.lossyScale;

        Material mat = renderer.material;

        Color startingColor = new Color(0f, 0f, 0f, 0f);
        Color targetColor = mat.GetColor("_EmissionColor");

        audioSource.Play();

        while (lerp < 1f)
        {
            lerp += Time.deltaTime / duration;

            transform.localScale = Vector3.Lerp(startSize, targetSize, lerp);
            mat.SetColor("_EmissionColor", Color.Lerp(startingColor, targetColor, lerp));

            yield return null;
        }

        Shoot();
    }

    public void Shoot()
    {
        Debug.Log("Shoot!");

        StopAllCoroutines();
        StartCoroutine(ShootIE(0.5f));
    }

    IEnumerator ShootIE(float reachDuration)
    {
        audioSource.PlayOneShot(shootSound);

        float lerp = 0f;

        Vector3 startingPosition = transform.position;

        Transform player = PlayerController.instance.cam.transform;

        transform.parent = player;

        while (lerp < 1f)
        {
            lerp += Time.deltaTime / reachDuration;
            transform.position = Vector3.Lerp(transform.position, player.position, lerp);

            yield return null;
        }

        transform.position = player.position;

        AudioManager.instance.audioMixer.SetFloat("lowpass", 500f);
    }
}