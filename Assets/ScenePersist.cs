using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour {

    int startIndex = -1;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        startIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        int currrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (startIndex != currrentSceneIndex)
        {
            Destroy(gameObject);
        }
    }


}
