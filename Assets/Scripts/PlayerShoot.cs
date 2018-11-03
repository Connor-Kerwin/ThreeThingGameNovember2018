using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private Tracer tracerPrefab;
    [SerializeField]
    private float weaponVelocity = 1.0f;
    [SerializeField]
    private Transform weaponShootPoint;

    private TracerPool tracerPool;

    public float firerate = 0.2f;
    public float time = 0f;

    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    private void Awake()
    {
        tracerPool = new TracerPool(tracerPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            time += Time.deltaTime;
            if (time >= firerate)
            {
                Shoot();
                time = 0;
            }

        }
    }

    void Shoot()
    {
        Vector3 startPos = weaponShootPoint.position;
        Vector3 endPos;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            endPos = hit.point;

            Debug.Log(hit.transform.name);
            Transform parent = hit.transform.root;
            if (parent != null)
            {
                GameObject target = parent.gameObject;
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
        else // Nothing was hit, assume end point is far away
        {
            endPos = transform.position + transform.forward * 100.0f;
        }

        tracerPool.SpawnTracer(startPos, endPos, weaponVelocity);

    }
}
