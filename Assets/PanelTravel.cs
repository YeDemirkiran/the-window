using UnityEngine;

public enum Axis { x, y, z }

public class PanelTravel : MonoBehaviour
{
    [SerializeField] Axis axis = Axis.x;
    [SerializeField] float speed, raycastDistance;
    [SerializeField] LayerMask raycastLayermask;

    bool instantiatedPanel = false, destroyed = false;
    int instantiatedSide = 0;

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckCollisions();
    }

    void CheckCollisions()
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
            if (AxisRaycast(raycastDistance, axis, raycastLayermask) == 1) // Positive side
            {
                // Instantiate panel

                instantiatedPanel = true;
                instantiatedSide = 1;

                GameObject newPanel = Instantiate(gameObject);
                newPanel.transform.position = transform.position;
                newPanel.transform.Rotate(Vector3.up * -90f);

                Debug.Log("3");
            }
            else if (AxisRaycast(raycastDistance, axis, raycastLayermask) == -1)  // Negative side
            {
                // Instantiate panel

                instantiatedPanel = true;
                instantiatedSide = -1;

                Debug.Log("4");

                GameObject newPanel = Instantiate(gameObject);
                newPanel.transform.position = transform.position;
                newPanel.transform.Rotate(Vector3.up * 90f);
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