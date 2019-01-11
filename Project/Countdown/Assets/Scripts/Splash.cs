using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    public GameObject text;
	// Use this for initialization
	void Start ()
    {
        Invoke("Disclaimer", 2);
	}

    public void Disclaimer()
    {
        text.SetActive(true);

        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(8);
        text.SetActive(false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainMenu");
    }

}
