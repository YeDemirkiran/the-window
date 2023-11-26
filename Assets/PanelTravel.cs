using UnityEngine;
using static UnityEngine.UI.Image;

public enum Axis { x, y, z }

public class PanelTravel : MonoBehaviour
{
    [SerializeField] Axis axis = Axis.x;
    [SerializeField] float speed, raycastDistance;
    [SerializeField] LayerMask raycastLayermask;

    bool instantiatedPanel = false, destroyed = false;
    int instantiatedSide = 0;

    public GameObject motherObject { get; set; }

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

                    GameObject newPanel = Instantiate(gameObject);

                    newPanel.transform.position = hit.point;

                    newPanel.transform.position -= transform.right * transform.lossyScale.z / 2f;
                    newPanel.transform.position -= transform.forward * transform.lossyScale.x / 2f;

                    newPanel.transform.Rotate(Vector3.up * -90f);

                    newPanel.GetComponent<PanelTravel>().motherObject = gameObject;

                    Debug.Log("3");
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

                    Debug.Log("4");

                    GameObject newPanel = Instantiate(gameObject);

                    newPanel.transform.position = hit.point;
                    newPanel.transform.position += transform.right * transform.lossyScale.z / 2f;
                    newPanel.transform.position -= transform.forward * transform.lossyScale.x / 2f;

                    newPanel.transform.Rotate(Vector3.up * 90f);

                    newPanel.GetComponent<PanelTravel>().motherObject = gameObject;
                }
            }
            else if (!destroyed)
            {
                if (AxisRaycast(transform.position - (axisVec * transform.lossyScale.x / 2f), raycastDistance, axis, raycastLayermask) == 1 && instantiatedSide == -1) // Positive side
                {
                    // Destroy panel
                    Debug.Log("1");
                    Destroy(gameObject);
                    destroyed = true;
                }
                else if (AxisRaycast(transform.position - (axisVec * transform.lossyScale.x / 2f), raycastDistance, axis, raycastLayermask) == -1 && instantiatedSide == 1)  // Negative side
                {
                    Destroy(gameObject);
                    destroyed = true;
                }
            }
        }
        else
        {
            Debug.Log("We have a mother object, waiting for it to get destroyed");
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
}