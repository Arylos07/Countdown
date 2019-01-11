using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject img;
    public GameObject scoreManager;
    public int score;
    public Text scoreText;
    public float timer = 0;
    public bool timerRunning = false;
    public Text timerText;
    public WordChecker wordChecker;
    public MathSolver mathSolver;
    public Text targetText;
    public GameObject submitButton;

    public GameObject[] lettersButtons;
    public InputField answerInputfield;

    public InputField bigNumbersField;
    public InputField smallNumbersField;

    public List<string> vowels = new List<string>();
    public List<string> consonants = new List<string>();

    public List<int> bigNumbers = new List<int>();
    public List<int> smallNumbers = new List<int>();

    public Transform numbersField;
    public GameObject numberTile;

    public void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager");
        StartCoroutine(FadeIn());
    }

    public void ReturnToMenu()
    {
        StartCoroutine(Menu());
    }

    IEnumerator Menu()
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

        scoreManager.GetComponent<SceneController>().LoadScene("MainMenu");
        
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

    public void Consonant()
    {
        wordChecker.AddLetter(consonants[Random.Range(0, consonants.Count)]);
        if(wordChecker.anagram.Length == 9)
        {
            foreach(GameObject button in lettersButtons)
            {
                button.GetComponent<Button>().interactable = false;
            }

            Invoke("SetTimer", 3.5f);
            timer = 3.5f;

            wordChecker.GetWordList();
        }
    }

    public void AdjustNumbers()
    {
        if (bigNumbersField.text != string.Empty)
        {
            if (int.Parse(bigNumbersField.text) > 4)
            {
                bigNumbersField.text = 4.ToString();
            }
            else
            {
                smallNumbersField.text = (6 - int.Parse(bigNumbersField.text)).ToString();
            }
            submitButton.GetComponent<Button>().interactable = true;
        }
        else if(bigNumbersField.text == string.Empty)
        {
            smallNumbersField.text = string.Empty;
            submitButton.GetComponent<Button>().interactable = false;
        }
    }

    public void GenerateNumbers()
    {
        bigNumbersField.interactable = false;
        smallNumbersField.interactable = false;
        submitButton.SetActive(false);

        StartCoroutine(Numbers(int.Parse(bigNumbersField.text), int.Parse(smallNumbersField.text)));
    }

    public void SetTimer()
    {
        timer = 30.3f;
        timerRunning = true;
        answerInputfield.gameObject.SetActive(true);

        if (SceneManager.GetActiveScene().name == "Numbers-Single")
            mathSolver.enabled = true;
    }

    public void SubmitMaths()
    {
        if (answerInputfield.text != string.Empty)
        {
            mathSolver.SubmitAnswer(int.Parse(answerInputfield.text));
            Finish();
        }
        else if(answerInputfield.text == string.Empty)
        {
            mathSolver.SubmitAnswer(0);
            Finish();
        }
    }

    public void Finish()
    {
        timer = 0.1f;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Final")
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                timerText.text = timer.ToString("F0");
            }

            if (timer <= 0 && timerRunning == true)
            {
                timerRunning = false;
                answerInputfield.interactable = false;
                timerText.text = string.Empty;
                if (SceneManager.GetActiveScene().name == "Letters-Single")
                    wordChecker.SearchWord();

                //parse input field
            }
        }

        scoreText.text = score.ToString();
    }

    public void SubmitWord()
    {
        wordChecker.CheckScore(answerInputfield.text);
    }

    public void Vowel()
    {
        wordChecker.AddLetter(vowels[Random.Range(0, vowels.Count)]);
        if (wordChecker.anagram.Length == 9)
        {
            foreach (GameObject button in lettersButtons)
            {
                button.GetComponent<Button>().interactable = false;
            }

            Invoke("SetTimer", 3);
            timer = 4;

            wordChecker.GetWordList();
        }
    }

    IEnumerator Numbers(int big, int small)
    {
        int i = 0;

        if(big > 4)
        {
            big = 4;
        }

        while (i < small)
        {
            int s = smallNumbers[Random.Range(0, smallNumbers.Count)];
            mathSolver.numbers.SetValue(s, i);
            i++;

            GameObject letterTileStage = Instantiate(numberTile, numbersField) as GameObject;
            LetterTile tile = letterTileStage.GetComponent<LetterTile>();

            tile.letter = s.ToString();

            yield return new WaitForSeconds(2);
        }

        i = 0;

        while (i < big)
        {
            int b = bigNumbers[Random.Range(0, bigNumbers.Count - small)];
            bigNumbers.Remove(b);   //since only 4 big numbers can be used and can't be repeated, remove the number generated from the list.
            mathSolver.numbers.SetValue(b, i);
            i++;

            GameObject letterTileStage = Instantiate(numberTile, numbersField) as GameObject;
            LetterTile tile = letterTileStage.GetComponent<LetterTile>();

            tile.letter = b.ToString();

            yield return new WaitForSeconds(2);
        }
        i = 0;

        float randomTimer = 0;
        int randomTarget = 0;

        while (randomTimer < 3)
        {
            randomTarget = Random.Range(0, 999);
            targetText.text = randomTarget.ToString();
            randomTimer += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }

        mathSolver.target = randomTarget;

        mathSolver.StartCoroutine(mathSolver.CalculateSolutions());

        Invoke("SetTimer", 3.5f);
        timer = 3.5f;

        yield return null;
    }
}
