using System.Collections;
using UnityEngine;

public class LightPanel : MonoBehaviour
{
    [SerializeField] float fadeDuration;
    [SerializeField] AudioClip audioClip;

    new Renderer renderer;
    new Collider collider;
    bool collided = false;

    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!collided && other.transform.root.CompareTag("Player"))
        {
            AudioManager.instance.audioSource.clip = audioClip;
            AudioManager.instance.audioSource.Play();

            FadePanel();
            transform.parent = other.transform.root;
            collided = true;
            collider.enabled = false;

            other.transform.GetComponent<PlayerController>().rb.isKinematic = false;
            other.transform.GetComponent<PlayerController>().rb.AddForce(other.transform.forward * 100f, ForceMode.VelocityChange);
        }
    }

    [ContextMenu("Fade")]
    void FadePanel()
    {
        StartCoroutine(Fade(fadeDuration));
    }

    IEnumerator Fade(float duration)
    {
        float lerp = 0f;
        Material mat = renderer.material;

        Color startingColor = mat.color;

        Color targetColor = startingColor;
        targetColor.a = 0f;

        mat.EnableKeyword("_EmissionColor");
        Color startingEmission = mat.GetColor("_EmissionColor");

        while (lerp < 1f)
        {
            lerp += Time.deltaTime / duration;

            mat.color = Color.Lerp(startingColor, targetColor, lerp);
            mat.SetColor("_EmissionColor", Color.Lerp(startingEmission, new Color(0f, 0f, 0f, 0f), lerp));

            yield return null;
        }

        Destroy(gameObject);
    }
}   