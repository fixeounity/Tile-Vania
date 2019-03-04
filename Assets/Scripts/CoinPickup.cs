using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

    [SerializeField] AudioClip coinPickupSFX;

    [SerializeField] int pickupPoints = 1;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Avoid colliding with the feet-collider of the player
        if (!(otherCollider is CapsuleCollider2D)) return;

        FindObjectOfType<GameSession>().AddToScore(pickupPoints);
        PlayPickupSFX();
        Destroy(gameObject);
    }

    private void PlayPickupSFX()
    {
        AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
    }
}
