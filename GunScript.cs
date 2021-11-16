using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public float impact = 30f;
    public float fireRate = 15f;
    public AudioSource audioSource;

    private float nextTimeToFire = 0f;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1")&&Time.time>=nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            shoot();
        }
    }
    public void shoot()
    {
        audioSource.Play();
        muzzleFlash.Play();
        RaycastHit hit;
        
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy!=null)
            {
                enemy.TakeDamage(damage);

            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impact);

            }
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);


        }
    }
}
