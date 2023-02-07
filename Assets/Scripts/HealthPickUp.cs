using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] AudioClip pickUpSound = default;
    [SerializeField] [Range(0, 1)] float pickUpSoundVolume = 0.7f;

    Player player;
    HealthDisplay health;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        health = FindObjectOfType<HealthDisplay>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(pickUpSound, Camera.main.transform.position, pickUpSoundVolume);
        player = collision.gameObject.GetComponent<Player>();
        if (!player) { return; }
        health.AddHealth();
        Destroy(gameObject);
    }
}
