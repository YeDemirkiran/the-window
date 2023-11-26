using System.Collections;
using UnityEngine;

public class LightPanel : MonoBehaviour
{
    [SerializeField] float fadeDuration;

    new Renderer renderer;
    bool collided = false;

    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collided && collision.transform.root.CompareTag("Player"))
        {
            FadePanel();
            transform.parent = collision.transform.root;
            collided = true;
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