using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour, IStateMachineListener<GameState>
{
    [SerializeField]
    private Tracer tracerPrefab;
    [SerializeField]
    private float weaponVelocity = 1.0f;
    [SerializeField]
    private Transform weaponShootPoint;
    [SerializeField]
    private float recoilPerShot = 0.25f;
    [SerializeField]
    private float recoilRecovery = 0.1f;
    [SerializeField]
    private float maxRecoil = 0.5f;
    [SerializeField]
    private Transform weapon;
    [SerializeField]
    private float MuzzleFlashTime = 0.05f;

    [SerializeField]
    private Transform muzzleFlashObject;

    private float recoil;
    private TracerPool tracerPool;
    private bool alive;

    public float firerate = 0.2f;
    public float time = 0f;

    public float damage = 10f;
    public float range = 100f;
    public ParticleSystem muzzleFlash;

    public Camera fpsCam;

    private void Awake()
    {
        tracerPool = new TracerPool(tracerPrefab);
        alive = false;
    }

    void Start()
    {
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        StateManager stateManager = managerStore.Get<StateManager>();
        stateManager.AddListener(this);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (alive) // Only behave when alive
        {
            if (Input.GetButton("Fire1"))
            {
                if (time >= firerate)
                {
                    Shoot();
                    muzzleFlashObject.gameObject.SetActive(true);
                    StartCoroutine(Flash());
                    time = 0;
                }
            }

            // Recover from recoil
            RecoilRecovery();

            // Apply physical recoil
            ApplyRecoil();
        }
    }

    private IEnumerator Flash()
    {
        yield return new WaitForSeconds(MuzzleFlashTime);
        muzzleFlashObject.gameObject.SetActive(false);
    }

    void Recoil()
    {
        recoil = Mathf.Clamp(recoil + recoilPerShot, 0.0f, maxRecoil);
    }

    void RecoilRecovery()
    {
        recoil = Mathf.Clamp(recoil - (recoilRecovery * Time.deltaTime), 0.0f, 1000.0f);
    }

    void ApplyRecoil()
    {
        Vector3 localRot = weapon.transform.localRotation.eulerAngles;
        localRot.x = -recoil;

        weapon.transform.localRotation = Quaternion.Euler(localRot);
    }

    void Shoot()
    {
        Recoil();

        Vector3 startPos = weaponShootPoint.position;
        Vector3 endPos;
        //muzzleFlash.Play();
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

    public void OnStateChanged(GameState previous, GameState current)
    {
        switch (current)
        {
            case GameState.Menu:
            case GameState.DeathScreen:

                alive = false;

                break;
            case GameState.Playing:

                alive = true;
                break;
        }
    }
}
