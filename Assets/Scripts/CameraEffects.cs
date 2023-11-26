using System.Collections;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    static CameraEffects instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public static void Shake(float duration, float frequency, float magnitude)
    {
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.IShake(duration, frequency, magnitude));
    }

    IEnumerator IShake(float duration, float frequency, float magnitude)
    {
        float lerp = 0f, timer = 0f;

        float shakeInterval = 1f / frequency;

        Vector3 startingPosition = transform.localPosition;

        Vector3 prePosition = startingPosition;
        Vector3 currentPosition = startingPosition + (Random.insideUnitSphere * magnitude);
        currentPosition.z = 0f;

        while (lerp < 1f)
        {
            lerp += Time.deltaTime / duration;
            timer += Time.deltaTime;

            if (timer > shakeInterval)
            {
                timer = 0f;

                prePosition = transform.localPosition;
                currentPosition = startingPosition + (Random.insideUnitSphere * magnitude);
                currentPosition.z = 0f;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(prePosition, currentPosition, timer / shakeInterval);
            }

            yield return null;
        }

        transform.localPosition = startingPosition;
    }
}