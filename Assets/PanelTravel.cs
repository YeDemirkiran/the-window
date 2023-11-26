using System.Collections;
using UnityEngine;

public enum Axis { x, y, z }

public class PanelTravel : MonoBehaviour
{
    [SerializeField] Axis axis = Axis.x;
    [SerializeField] float speed, raycastDistance;
    [SerializeField] LayerMask raycastLayermask;
    [SerializeField] Transform meshObject;

    [Header("Fade")]
    [SerializeField] float fadeDuration;
    [SerializeField] float fadeEmission;

    bool instantiatedPanel = false, destroyed = false;
    int instantiatedSide = 0;

    public GameObject motherObject { get; set; }
    GameObject createdObject;
    new Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckCollisions();
    }

    void CheckCollisions()
    {
        if (motherObject == null)
        {
            Vector3 axisVec = Vector3.zero;

            switch (axis)
            {
                case Axis.x:
                    axisVec = Vector3.right;
                    break;
                case Axis.y:
                    axisVec = Vector3.up;
                    break;
                case Axis.z:
                    axisVec = Vector3.forward;
                    break;
            }

            if (!instantiatedPanel)
            {
                RaycastHit hit;

                if (AxisRaycast(raycastDistance, axis, raycastLayermask, out hit) == 1) // Positive side
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Light Panel"))
                    {
                        speed = -speed;
                        return;
                    }

                    // Instantiate panel
                    instantiatedPanel = true;
                    instantiatedSide = 1;

                    PlayerController player = null;

                    foreach (Transform child in transform)
                    {
                        if (child.TryGetComponent(out player))
                        {
                            player.transform.parent = null;
                            break;
                        }
                    }

                    GameObject newPanel = Instantiate(gameObject);
                    newPanel.name = gameObject.name;

                    newPanel.transform.position = hit.point;

                    newPanel.transform.position -= transform.right * meshObject.lossyScale.z / 2f;
                    newPanel.transform.position -= transform.forward * meshObject.lossyScale.x / 2f;

                    newPanel.transform.Rotate(Vector3.up * -90f);

                    newPanel.GetComponent<PanelTravel>().motherObject = gameObject;

                    createdObject = newPanel;

                    if (player != null)
                    {
                        player.transform.parent = createdObject.transform;
                        player.currentMovingObject = createdObject.transform;
                    }

                    //Debug.Log("3");
                }
                else if (AxisRaycast(raycastDistance, axis, raycastLayermask, out hit) == -1)  // Negative side
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Light Panel"))
                    {
                        speed = -speed;
                        return;
                    }

                    // Instantiate panel
                    instantiatedPanel = true;
                    instantiatedSide = -1;

                    //Debug.Log("4");
                    PlayerController player = null;

                    foreach (Transform child in transform)
                    {
                        if (child.TryGetComponent(out player))
                        {
                            player.transform.parent = null;
                            break;
                        }
                    }

                    GameObject newPanel = Instantiate(gameObject);
                    newPanel.name = gameObject.name;

                    newPanel.transform.position = hit.point;
                    newPanel.transform.position += transform.right * meshObject.lossyScale.z / 2f;
                    newPanel.transform.position -= transform.forward * meshObject.lossyScale.x / 2f;

                    newPanel.transform.Rotate(Vector3.up * 90f);

                    newPanel.GetComponent<PanelTravel>().motherObject = gameObject;

                    createdObject = newPanel;

                    if (player != null)
                    {
                        player.transform.parent = createdObject.transform;
                        player.currentMovingObject = createdObject.transform;
                    }        
                }
            }
            else if (!destroyed)
            {
                if (AxisRaycast(transform.position - (axisVec * meshObject.lossyScale.x / 2f), raycastDistance, axis, raycastLayermask) == 1 && instantiatedSide == -1) // Positive side
                {
                    // Destroy panel
                    //Debug.Log("1");

                    Destroy(gameObject);
                    destroyed = true;
                }
                else if (AxisRaycast(transform.position - (axisVec * meshObject.lossyScale.x / 2f), raycastDistance, axis, raycastLayermask) == -1 && instantiatedSide == 1)  // Negative side
                {
                    Destroy(gameObject);
                    destroyed = true;
                }
            }
        }
        else
        {
            //Debug.Log("We have a mother object, waiting for it to get destroyed");
        }     
    }



    int AxisRaycast(Vector3 origin, float maxDistance, Axis axis, LayerMask layerMask)
    {
        switch (axis)
        {
            case Axis.x:
                if (Physics.Raycast(origin, transform.right, maxDistance, layerMask))
                {
                    return 1;
                }
                else if (Physics.Raycast(origin, -transform.right, maxDistance, layerMask))
                {
                    return -1;
                }

                break;

            case Axis.y:
                if (Physics.Raycast(origin, transform.up, maxDistance, layerMask))
                {
                    return 1;
                }
                else if (Physics.Raycast(origin, -transform.up, maxDistance, layerMask))
                {
                    return -1;
                }

                break;

            case Axis.z:
                if (Physics.Raycast(origin, transform.forward, maxDistance, layerMask))
                {
                    return 1;
                }
                else if (Physics.Raycast(origin, -transform.forward, maxDistance, layerMask))
                {
                    return -1;
                }

                break;
        }

        return 0;
    }

    int AxisRaycast(float maxDistance, Axis axis, LayerMask layerMask)
    {
        switch (axis)
        {
            case Axis.x:
                if (Physics.Raycast(transform.position, transform.right, maxDistance, layerMask))
                {
                    return 1;
                } 
                else if (Physics.Raycast(transform.position, -transform.right, maxDistance, layerMask))
                {
                    return -1;
                }

                break;

            case Axis.y:
                if (Physics.Raycast(transform.position, transform.up, maxDistance, layerMask))
                {
                    return 1;
                }
                else if (Physics.Raycast(transform.position, -transform.up, maxDistance, layerMask))
                {
                    return -1;
                }

                break;

            case Axis.z:
                if (Physics.Raycast(transform.position, transform.forward, maxDistance, layerMask))
                {
                    return 1;
                }
                else if (Physics.Raycast(transform.position, -transform.forward, maxDistance, layerMask))
                {
                    return -1;
                }

                break;
        }

        return 0;
    }

    int AxisRaycast(float maxDistance, Axis axis, LayerMask layerMask, out RaycastHit hit)
    {
        switch (axis)
        {
            case Axis.x:
                if (Physics.Raycast(transform.position, transform.right, out RaycastHit hitinfo1, maxDistance, layerMask))
                {
                    hit = hitinfo1;
                    return 1;
                }
                else if (Physics.Raycast(transform.position, -transform.right, out RaycastHit hitinfo2, maxDistance, layerMask))
                {
                    hit = hitinfo2;
                    return -1;
                }

                break;

            case Axis.y:
                if (Physics.Raycast(transform.position, transform.up, out RaycastHit hitinfo3, maxDistance, layerMask))
                {
                    hit = hitinfo3;
                    return 1;
                }
                else if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitinfo4, maxDistance, layerMask))
                {
                    hit = hitinfo4;
                    return -1;
                }

                break;

            case Axis.z:
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitinfo5, maxDistance, layerMask))
                {
                    hit = hitinfo5;
                    return 1;
                }
                else if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hitinfo6, maxDistance, layerMask))
                {
                    hit = hitinfo6;
                    return -1;
                }

                break;
        }

        hit = default;
        return 0;
    }

    void Move()
    {
        switch (axis)
        {
            case Axis.x:
                transform.position += transform.right * speed * Time.deltaTime;
                break;

            case Axis.y:
                transform.position += transform.up * speed * Time.deltaTime;
                break;

            case Axis.z:
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
        }
    }

    void FadePanel()
    {
        StopAllCoroutines();
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        float lerp = 0f;
        Material mat = renderer.material;

        mat.EnableKeyword("_EmissionColor");

        Color startingEmission = mat.GetColor("_EmissionColor");
        Color targetEmission = startingEmission;
        targetEmission.a = fadeEmission;

        while (lerp < 1f)
        {
            lerp += Time.deltaTime / fadeDuration;

            mat.SetColor("_EmissionColor", Color.Lerp(startingEmission, targetEmission, lerp));

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("anan");

        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("baban");
            FadePanel();
        }
    }
}