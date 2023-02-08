using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Ship")]
    [SerializeField] float movementSpeed = 15f;
    [SerializeField] float padding = 0.6f;
    [SerializeField] int health = 5;

    [Header("Shooting")]
    [SerializeField] GameObject laserPrefab = default;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = .2f;
    [SerializeField] AudioClip shootingSFX = default;
    [SerializeField] [Range(0, 1)] float shootingSoundVolume = 0.7f;

    [Header("Death Explosion")]
    [SerializeField] GameObject deathFX = default;
    [SerializeField] GameObject hithFX = default;
    [SerializeField] float durationOfExplosion = 1f;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    HealthDisplay healthDisplay;

    //private LevelLoader levelLoader;

    void Start()
    {
        SetUpMoveBoundaries();
        healthDisplay = FindObjectOfType<HealthDisplay>();
    //    levelLoader = FindObjectOfType<LevelLoader>();
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    void Update()
    {
        Move();
        Shoot();
    }

    private void Shoot()
    {
        string fireButton;
        if(LevelLoader.keyboardControlOn)
            fireButton = "Fire1"; //Keyboard control
        else
            fireButton = "Fire2"; //Mouse/Touch control

        if(Input.GetButtonDown(fireButton))
        {
            firingCoroutine = StartCoroutine(FireContinuosly());
            
        }
        else if (Input.GetButtonUp(fireButton))
        {
            StopCoroutine(firingCoroutine);
        }
    }
    IEnumerator FireContinuosly()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootingSFX, Camera.main.transform.position, shootingSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(laserPrefab);
    }

    private void Move()
    {
        if(LevelLoader.keyboardControlOn) //Keyboard control
        {
            float deltaX = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
            float deltaY = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

            float newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
            float newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
            transform.position = new Vector2(newXPos, newYPos);
        }
        else //Mouse/Touch control
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y + 100f, 1f));
            worldPos = new Vector2(Mathf.Clamp(worldPos.x, xMin, xMax), Mathf.Clamp(worldPos.y, yMin, yMax));
            transform.position = worldPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        InstantiateVFX(hithFX);
        healthDisplay.SubtractHealth();
        damageDealer.DestroyProjectileOnHit();
        health -= damageDealer.GetDamage();
        if (health <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        healthDisplay.LoseAllLives();
        FindObjectOfType<LevelLoader>().LoadGameOver();
        Destroy(gameObject);
        InstantiateVFX(deathFX);
    }

    private void InstantiateVFX(GameObject vfx)
    {
        GameObject explosion = Instantiate(vfx, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
    }

    public int GetHealth()
    {
        return health;
    }

    public void AddHealth()
    {
        health++;
    }
}
