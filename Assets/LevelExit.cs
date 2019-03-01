using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] float playerFadeSpeed = 0.1f;
    [SerializeField] float playerFadeInterval = 0.1f;

    private void StopPlayerInput(ref Collider2D otherCollider)
    {
        otherCollider.GetComponent<Player>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        StopPlayerInput(ref otherCollider);
        var playerRenderer = otherCollider.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOutPlayer(playerRenderer));
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator FadeOutPlayer(SpriteRenderer playerRenderer)
    {
        if (!playerRenderer) yield return null;

        while(playerRenderer.color.a > 0f)
        {
            Color newColor = playerRenderer.color;
            newColor.a -= playerFadeSpeed;
            playerRenderer.color = newColor;
            yield return new WaitForSecondsRealtime(playerFadeInterval);
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
