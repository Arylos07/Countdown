using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static int score;
    public GameManager gameManager;
	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}
	
    public static void AddScore(int value)
    {
        score += value;
    }

	// Update is called once per frame
	void Update ()
    {
		if(gameManager == null && SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (SceneManager.GetActiveScene().name != "Splash")
                gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
        else if (gameManager != null)
        {
            gameManager.score = score;
        }
	}
}
