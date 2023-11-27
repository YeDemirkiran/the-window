using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis { x, y, z }

public class Panel : MonoBehaviour
{
    public static List<Panel> panels = new List<Panel>();

    public static Panel FindByID(int id)
    {
        foreach (Panel panel in panels)
        {
            if (panel.id == id)
            {
                return panel;
            }
        }

        return null;
    }

    public static int FindByIDint(int id)
    {
        int i = 0;

        foreach (Panel panel in panels)
        {
            if (panel.id == id)
            {
                return i;
            }

            i++;
        }

        return -1;
    }

    public int id { get; private set; }

    [SerializeField] Axis axis = Axis.x;
    [SerializeField] float speed, raycastDistance;
    [SerializeField] LayerMask raycastLayermask;
    [SerializeField] Transform meshObject;

    [Header("Fade")]
    [SerializeField] float fadeDuration;
    [ColorUsage(false, true)][SerializeField] Color emissionColor;

    bool instantiatedPanel = false, destroyed = false;
    int instantiatedSide = 0;

    public bool dontAddInList { get; set; }
    public bool deactivated { get; set; }


    public GameObject motherObject { get; set; }
    GameObject createdObject;
    new Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();

        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
        }   
    }

    private void Start()
    {
        if (dontAddInList)
        {
            return;
        }

        panels.Add(this);
        panels.Sort((x, y) => x.transform.position.y.CompareTo(y.transform.position.y));

        int i = 0;
        foreach (Panel panel in panels)
        {
            if (panel == this)
            {
                id = i + 1;
                break;
            }

            i++;
        }
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

                int raycast = AxisRaycast(raycastDistance, axis, raycastLayermask, out hit);

                if (raycast != 0)
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Light Panel"))
                    {
                        speed = -speed;
                        return;
                    }

                    // Instantiate panel
                    instantiatedPanel = true;
                    instantiatedSide = raycast;

                    PlayerController player = PlayerController.instance;
                    bool playerWasChild = false;

                    if (player.transform.parent == transform)
                    {
                        player.transform.parent = null;
                        playerWasChild = true;
                    }                    

                    GameObject newPanel = Instantiate(gameObject);
                    newPanel.name = gameObject.name;
                    newPanel.transform.position = hit.point;

                    newPanel.transform.position += -raycast * transform.right * meshObject.lossyScale.z / 2f;
                    newPanel.transform.position -= transform.forward * meshObject.lossyScale.x / 2f;
                    newPanel.transform.Rotate(Vector3.up * -raycast * 90f);

                    Panel p = newPanel.GetComponent<Panel>();

                    p.dontAddInList = true;
                    p.id = id;
                    p.deactivated = deactivated;
                    p.motherObject = gameObject;

                    createdObject = newPanel;

                    if (PlayerController.instance.currentMovingObject == transform)
                    {
                        Debug.Log("111");

                        if (playerWasChild)
                        {
                            Debug.Log("333");

                            player.transform.parent = createdObject.transform.root;
                        }

                        player.currentMovingObject = createdObject.transform;
                    }
                    else
                    {
                        Debug.Log("222");
                    }
                }
            }
            else if (!destroyed)
            {
                int raycast = AxisRaycast(transform.position - (axisVec * meshObject.lossyScale.x / 2f), raycastDistance, axis, raycastLayermask);

                if ((raycast == 1 && instantiatedSide == -1) || (raycast == -1 && instantiatedSide == 1))
                {
                    Destroy(gameObject);
                    destroyed = true;
                }
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

    public void Deactivate()
    {
        if (!deactivated)
        {
            Debug.Log(panels.Count);

            deactivated = true;
            FadePanel();

            panels.RemoveAt(FindByIDint(id));
            

            Debug.Log(panels.Count);
        }        
    }

    void FadePanel()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        float lerp = 0f;
        Material mat = renderer.material;

        mat.EnableKeyword("_EmissionColor");

        Color startingEmission = mat.GetColor("_EmissionColor");
        Color targetEmission = emissionColor;

        while (lerp < 1f)
        {
            lerp += Time.deltaTime / fadeDuration;

            mat.SetColor("_EmissionColor", Color.Lerp(startingEmission, targetEmission, lerp));

            yield return null;
        }
    }
}