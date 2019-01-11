using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject img;
    public JimmyCarr jimmyCarr;
    public GameObject credits;

	// Use this for initialization
	void Start ()
    {
        jimmyCarr = GameObject.Find("[ScoreManager]").GetComponent<JimmyCarr>();
        StartCoroutine(FadeIn());
	}

    IEnumerator FadeIn()
    {
        // loop over 1 second backwards
        for (float alpha = 1; alpha >= 0; alpha -= Time.unscaledDeltaTime)
        {
            // set color with i as alpha
            img.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        img.SetActive(false);
    }

    public void PlayStandard()
    {
        StartCoroutine(Play());
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ToggleCredits()
    {
        credits.SetActive(!credits.activeSelf);
    }

    public void ProjectSource()
    {
        Application.OpenURL("null");    //replace with project URL later
    }

    IEnumerator Play()
    {
        img.SetActive(true);
        // loop over 1 second
        for (float alpha = 0; alpha <= 1; alpha += Time.unscaledDeltaTime)
        {
            // set color with i as alpha
            img.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        jimmyCarr.StartStandardMode();
    }
}
