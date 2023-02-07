using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Ship Stats")]
    [SerializeField] int health = 100;
    [SerializeField] int scorePoints = 15;
    
    [Header("Shooting")]
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    float shotCounter;

    [Header("Visuals and VFX")]
    [SerializeField] GameObject enemyExplosionFX = default;
    [SerializeField] GameObject hitFX = default;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] GameObject enemyLaserPrefab = default;

    [Header("SFX")]
    [SerializeField] AudioClip[] shootingSounds = default;
    [SerializeField] [Range(0,1)] float shootingSoundVolume = 0.7f;

    [Header("PickUp")]
    [SerializeField] GameObject healthPickup = default;
    [SerializeField] int minNumberToCalculateSpawnChance = 1;
    [SerializeField] int maxNumberToCalculateSpawnChance = 30;
    [SerializeField] float pickUpFallSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Shoot();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Shoot()
    {
        GameObject enemyLaser = Instantiate(enemyLaserPrefab, transform.position, Quaternion.identity) as GameObject;
        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioClip clip = shootingSounds[Random.Range(0, shootingSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, shootingSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        InstantiateVFX(hitFX);
        damageDealer.DestroyProjectileOnHit();
        health -= damageDealer.GetDamage();
        if (health <= 0)
        {
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        FindObjectOfType<GameSession>().AddToScore(scorePoints);
        Destroy(gameObject);
        InstantiateVFX(enemyExplosionFX);
        InstantiateHealthPickup();
    }

    public void InstantiateHealthPickup()
    {
        int spawnChance = Random.Range(minNumberToCalculateSpawnChance, maxNumberToCalculateSpawnChance);
        if(spawnChance == 11)
        {
            GameObject pickup = Instantiate(healthPickup, transform.position, transform.rotation);
            pickup.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -pickUpFallSpeed);
        }
    }

    private void InstantiateVFX(GameObject vfx)
    {
        GameObject explosion = Instantiate(vfx, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
    }
}
