using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : Manager
{
    [SerializeField]
    private Image prefab;
    [SerializeField]
    private Image player;
    [SerializeField]
    private Transform minimapParent;
    [SerializeField]
    private float minimapScale = 0.1f;
    [SerializeField]
    private float minimapWidth = 100.0f;
    [SerializeField]
    private float scanSize = 50.0f;

    private List<Image> blips;
    private Pool<Image> pool;

    private SpawnManager spawnManager;
    private PlayerManager playerManager;

    private void Awake()
    {
        pool = new Pool<Image>(CreateNewImage);
        blips = new List<Image>();
    }

    private Image CreateNewImage()
    {
        Image instance = GameObject.Instantiate<Image>(prefab);
        instance.transform.SetParent(minimapParent, false);
        return instance;
    }

    public override bool Link()
    {
        Main main = Main.Instance;
        ManagerStore managerStore = main.ManagerStore;
        spawnManager = managerStore.Get<SpawnManager>();
        playerManager = managerStore.Get<PlayerManager>();

        return true;
    }

    private Image GetBlip()
    {
        return pool.Get();
    }

    private void UpdateMinimap()
    {
        // Align the player
        Transform pTransform = playerManager.Player;
        Vector3 rot = pTransform.rotation.eulerAngles;
        player.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -(rot.y + 180.0f)));

        // Return all items
        foreach (Image image in blips)
        {
            pool.Store(image);
        }

        List<Spawnable> active = spawnManager.Active;

        foreach (Spawnable spawnable in active)
        {
            if (spawnable.SpawnType == SpawnType.Enemy) // Ensure the spawnable is an enemy
            {
                // Get the world origin and the difference
                Vector3 worldOrigin = Vector3.zero;
                Vector3 pos = spawnable.transform.position;
                Vector3 diff = worldOrigin - pos;

                float x = Mathf.Abs(diff.x);
                float z = Mathf.Abs(diff.z);

                // Within scan radius?
                //if(x <= scanSize && z <= scanSize)
                //{
                    float fX = diff.x / scanSize;
                    float fZ = diff.z / scanSize;

                    float mX = fX * minimapWidth;
                    float mZ = fZ * minimapWidth;

                    // Get the blip
                    Image image = GetBlip();
                    blips.Add(image);

                    float half = scanSize * 0.5f;
                    image.transform.localPosition = new Vector3(mX + half, mZ - half, 0);
                //}

                //// Get the blip
                //Image image = GetBlip();
                //blips.Add(image);

                //// Scale down the position to minimap space
                //diff *= minimapScale;



                //image.transform.localPosition = new Vector3(diff.x, diff.z, 0);
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMinimap();
    }
}