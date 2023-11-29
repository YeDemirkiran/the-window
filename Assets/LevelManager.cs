using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    public Panel currentPanel {  get; private set; }
    public int currentPanelID { get; private set; }

    [Header("Escape Animation")]
    [SerializeField] float startDelay = 3f, podArriveDelay = 2f;
    [SerializeField] AudioClip alarmSound, escapeMusic;
    [SerializeField] GameObject podPrefab;

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

    public void NewLevel()
    {
        Debug.Log("Creating a new level");

        StopAllCoroutines();
        StartCoroutine(NewLevelIE());
    }

    IEnumerator NewLevelIE()
    {
        yield return new WaitForSeconds(startDelay);

        AudioSource musicSource = MusicManager.instance.audioSource;
        musicSource.Stop();
        musicSource.clip = escapeMusic;
        musicSource.Play();

        AudioManager.instance.audioSource.PlayOneShot(alarmSound);

        yield return new WaitForSeconds(podArriveDelay);

        GameObject pod = Instantiate(podPrefab);

        Transform player = PlayerController.instance.transform;

        pod.transform.position = player.position + player.forward + player.up * 5f;

        Debug.Break();

        while (true)
        {
            yield return null;
        }
    }
}