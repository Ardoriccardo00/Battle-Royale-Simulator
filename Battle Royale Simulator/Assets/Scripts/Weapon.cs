using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] float range = 100f;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] Transform shootingPoint;
    [SerializeField] Bullet bullet;
    bool canShoot = true;
    int ammo;

    private void OnEnable()
    {
        canShoot = true;
    }

    void Start()
    {
        ammo = Random.Range(5, 20);
    }

    void Update()
    {
        if(ammo == 0) Destroy(gameObject);
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        if(ammo > 0)
        {
            PlayMuzzleFlash();
            Instantiate(bullet, shootingPoint.position, bullet.transform.rotation);
            ammo--;
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }
}
