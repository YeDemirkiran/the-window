using System.Collections;
using UnityEngine;

public class SpaceShit : MonoBehaviour
{
    [SerializeField] float arriveTime = 2f, arriveOffset = 5f;
    [SerializeField] AnimationCurve curve;

    [Header("Projectile")]
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] GameObject projectilePrefab;

    bool canShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        Activate();
    }

    public void Activate()
    {
        StopAllCoroutines();
        StartCoroutine(ActivateIE());
    }

    IEnumerator ActivateIE()
    {
        StartCoroutine(MoveToPlayer(arriveTime, arriveOffset));

        yield return new WaitUntil(() => canShoot);

        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = projectileSpawnPoint.position;
        projectile.transform.eulerAngles = projectileSpawnPoint.eulerAngles;
        projectile.transform.parent = transform;

        projectile.GetComponent<CaptureProjectile>().Prepare(3f);
    }

    IEnumerator MoveToPlayer(float duration, float offset)
    {
        Vector3 startingPosition = transform.position;
        Quaternion startingRotation = transform.rotation;

        Transform player = PlayerController.instance.cam.transform;
        Vector3 targetPosition;
        Quaternion targetRotation;

        float lerp = 0f;

        transform.parent = player;

        while (lerp < 1f)
        {
            lerp += Time.deltaTime / duration;

            targetPosition = player.position + (player.forward * offset);
            targetRotation = Quaternion.LookRotation((player.position - transform.position).normalized);

            transform.position = Vector3.Slerp(transform.position, targetPosition, curve.Evaluate(lerp));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, curve.Evaluate(lerp));

            yield return null;
        }

        if (lerp >= 0.75f)
        {
            canShoot = true;
        }
        
        //transform.position = player.position + (player.forward * offset);
        //transform.rotation = Quaternion.LookRotation((player.position - transform.position).normalized); 
    }
}