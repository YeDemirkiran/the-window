using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float destinationReachTime = 3f;
    [SerializeField] LayerMask raycastLayer;

    public Rigidbody rb {  get; private set; }

    bool attachAtDestination = false;
    Vector3 destination;

    RaycastHit hit;

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

            destination = hit.point;   

            if (hit.collider.CompareTag("Stopper"))
            {
                currentMovingObject = hit.collider.transform;
                attachAtDestination = true;
            }

            MoveToDestination();
        }

        if (attachAtDestination)
        {
            destination = currentMovingObject.position;
        }

        Debug.Log(attachAtDestination);
    }

    void MoveToDestination()
    {
        StopAllCoroutines();
        StartCoroutine(Move(destinationReachTime));
    }

    IEnumerator Move(float duration)
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
    }
}