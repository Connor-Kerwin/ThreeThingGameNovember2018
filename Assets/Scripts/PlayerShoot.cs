using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float firerate = 0.2f;
    public float time = 0f;

    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

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
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
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
    }
}
