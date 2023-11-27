using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip launchClip;
    [SerializeField] float destinationReachTime = 3f;
    [SerializeField] LayerMask raycastLayer;

    public Rigidbody rb {  get; private set; }

    bool attachAtDestination = false;
    Vector3 destination;

    RaycastHit hit;

    [Header("Shake Effects")]
    [SerializeField] float clickShakeDuration = 0.25f;
    [SerializeField] float clickShakeFrequency = 25f, clickShakeMagnitude = 0.5f;

    [SerializeField] float hitShakeDuration = 0.5f;
    [SerializeField] float hitShakeFrequency = 50f, hitShakeMagnitude = 0.75f;

    public Transform currentMovingObject { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {      
        if (Input.GetKeyDown(KeyCode.Mouse0) && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, raycastLayer))
        {
            // Reset
            transform.parent = null;
            rb.isKinematic = false;
            currentMovingObject = null;
            attachAtDestination = false;

            destination = hit.point - transform.forward;

            UnityAction action = null;    

            if (hit.collider.CompareTag("Stopper"))
            {
                if (hit.transform.root.TryGetComponent(out Panel panel))
                {
                    action += panel.FadePanel;
                    action += () =>
                    {
                        foreach (var p in Panel.panels)
                        {
                            if (panel.panelID == p.panelID)
                            {
                                Panel.panels.Remove(p);
                                break;
                            }
                        }
                    };
                }

                currentMovingObject = hit.collider.transform;
                attachAtDestination = true;
            }
            else
            {
                action += () => rb.AddForce(-transform.forward * 10f, ForceMode.VelocityChange);
                action += () => CameraEffects.Shake(hitShakeDuration, hitShakeFrequency, hitShakeMagnitude);
            }

            MoveToDestination(action);

            // Effects

            CameraEffects.Shake(clickShakeDuration, clickShakeFrequency, clickShakeMagnitude);
            audioSource.PlayOneShot(launchClip);
        }

        if (attachAtDestination)
        {
            destination = currentMovingObject.position + currentMovingObject.forward;
        }
    }

    void MoveToDestination(UnityAction action)
    {
        StopAllCoroutines();
        StartCoroutine(Move(destinationReachTime, action));
    }

    IEnumerator Move(float duration, UnityAction actionAtReach)
    {
        float lerp = 0f;
        rb.isKinematic = true;

        Vector3 startingPosition = transform.position;

        while (lerp < 1f)
        {
            lerp += Time.deltaTime / duration;

            transform.position = Vector3.Lerp(startingPosition, destination, lerp);

            yield return null;
        }

        transform.position = destination;

        if (attachAtDestination)
        {
            transform.parent = hit.collider.transform.root;
        }
        else
        {
            rb.isKinematic = false;
        }

        if (actionAtReach != null)
        {
            actionAtReach.Invoke(); 
        }
    }
}