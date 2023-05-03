
using UnityEngine;
public class GunSystem : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;

    //public float fireRate = 15f;
    //public float nextTimeToFire = 0f; // fire rate
    public float timeBetweenShooting, timeBetweenEeachShot; // how fast can i press button
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public float reloadTime;
    int bulletsLeft, bulletsShot;
    

    bool shooting, readyToShoot, reloading; // bools

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public int metka = 0;
    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    void Update()
    {
       
        MyInput();
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            //nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            muzzleFlash.Play();
        }
    }
    void Shoot()
    {
        readyToShoot = false;

        //float x = Random.Range(-spread, spread);
        //float y = Random.Range(-spread, spread);
        
        
        //calc spread
        //Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);


        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            GameObject impactGo =  Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f); // seconds
        }
        bulletsLeft--;

        Invoke("ResetShot", timeBetweenShooting);
        //if (bulletsLeft > 0)
         //   Invoke("Shoot", timeBetweenEeachShot);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
