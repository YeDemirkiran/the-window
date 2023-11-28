using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Panel currentPanel {  get; private set; }
    public int currentPanelID { get; private set; }

    public void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => Panel.panels.Count >= 1);
        currentPanelID = Panel.panels[0].id;

        Panel.FindByID(currentPanelID).StartPulse();
    }

    public void IncreaseID(int amount = 1)
    {
        currentPanelID += amount;

        Panel p = Panel.FindByID(currentPanelID);

        if (p != null)
        {
            p.StartPulse();
        }
    }
}