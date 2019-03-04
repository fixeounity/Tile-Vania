using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

    [SerializeField] AudioClip coinPickupSFX;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        PlayPickupSFX();
        Destroy(gameObject);
    }

    private void PlayPickupSFX()
    {
        AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
    }
}
