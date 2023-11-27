using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] float shieldFadeDuration;
    new Renderer renderer;

    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Panel.panels.Count == 0)
        {
            FadePanel();
        }
    }

    public void FadePanel()
    {
        StartCoroutine(Fade(shieldFadeDuration));
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