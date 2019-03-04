using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour {

    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] float reloadSceneDelay = 2f;

    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void UpdateLivesText()
    {
        livesText.text = playerLives.ToString();
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    private void Start()
    {
        UpdateLivesText();
        UpdateScoreText();
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        UpdateScoreText();
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene("Main Menu");
        Destroy(gameObject);
    }

    private IEnumerator ReloadCurrentScene()
    {
        yield return new WaitForSecondsRealtime(reloadSceneDelay);
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void TakeLife()
    {
        playerLives--;
        UpdateLivesText();
        StartCoroutine(ReloadCurrentScene());
    }

}
