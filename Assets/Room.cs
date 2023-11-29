using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public static Room Instance { get; private set; }
    public LightPanel currentLight {  get; private set; }

    [Header("Room Initialization")]
    [SerializeField] GameObject lightPanelPrefab;

    [Header("Panel Initialization")]
    [SerializeField] GameObject climbPanelPrefab;
    [SerializeField] int minCount, maxCount;
    [SerializeField] float minSpeed, maxSpeed;

    List<Transform> blocks;

    void Awake()
    {
        Instance = this;

        Transform walls = transform.Find("Walls");

        blocks = walls.GetComponentsInChildren<Transform>().ToList();
        blocks.RemoveAt(0);

        CreateRoom();
    }

    public void CreateRoom()
    {
        CreateLightPanel();
        CreateClimbPanels();
    }

    void CreateLightPanel()
    {
        Transform block = ChooseRandomBlock();

        GameObject panel = Instantiate(lightPanelPrefab);
        panel.transform.parent = block.parent;
        panel.transform.localPosition = block.localPosition;
        panel.transform.localEulerAngles = block.localEulerAngles;
        Destroy(block.gameObject);

        currentLight = panel.GetComponent<LightPanel>();
    }

    void CreateClimbPanels()
    {
        int random = Random.Range(minCount, maxCount);

        for (int i = 0; i < random; i++)
        {
            Transform block = ChooseRandomBlock();
            GameObject panel = Instantiate(climbPanelPrefab);
            Panel _panel = panel.GetComponent<Panel>();

            panel.transform.eulerAngles = block.eulerAngles;
            panel.transform.position = block.position + (block.forward * block.lossyScale.z / 2f) + (panel.transform.forward * _panel.meshObject.lossyScale.z / 2f);

            _panel.id = i + 1;
            _panel.axis = (Axis)Random.Range(0, 2);
            _panel.speed = Random.Range(minSpeed, maxSpeed);
            _panel.speed *= Random.Range(0, 2) == 1 ? 1 : -1;

            Panel.panels.Add(_panel);
        }
    }

    Transform ChooseRandomBlock()
    {
        int random = Random.Range(0, blocks.Count - 6);

        int i = 0;

        foreach (Transform item in blocks)
        {
            if (item.name == "Block")
            {
                if (i == random)
                {
                    return item;
                }

                i++;
            }    
        }

        Debug.Log("Block couldn't be found! I: " + i + ", Random: " + random);

        return null;
    }
}