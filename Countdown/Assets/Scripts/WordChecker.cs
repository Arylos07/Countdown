using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordChecker : MonoBehaviour
{
    public GameObject img; //black image for scene transitions;
    public GameObject letterTile;
    public Transform Letters;
    public Transform Answer;

    public string guess;

    public GameManager gameManager;
    public string anagram;
    public Text conundrum;
    public WordsStruct[] data;
    WWW www;
    public bool wordFound;
    private int score;
    public List<string> letters = new List<string>();
    Dictionary<string, string> headers = new Dictionary<string, string>()
    {
        {"X-RapidAPI-Key", "e16399e637msh06223974b4a1f55p15d3a9jsnf3b7e3070b10"}
    };

    public void AddLetter(string letter)
    {
        GameObject letterTileStage = Instantiate(letterTile, Letters) as GameObject;
        LetterTile tile = letterTileStage.GetComponent<LetterTile>();

        tile.letter = letter;

        anagram += letter;
    }

    public void AddLetterAnswer(string letter)
    {
        GameObject letterTileStage = Instantiate(letterTile, Answer) as GameObject;
        LetterTile tile = letterTileStage.GetComponent<LetterTile>();

        tile.letter = letter;
    }

    public void GetWordList()
    {
        StartCoroutine(CheckWord());
    }

    IEnumerator CheckWord()
    {
        yield return new WaitForSeconds(1);

        www = new WWW("https://danielthepope-countdown-v1.p.rapidapi.com/solve/"+anagram+"?variance=9", null, headers);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Processjson(www.text);
        }
        else
        {
            print(www.error.ToString());
            Invoke("GetWordList", 3);
        }
    }

    private void Processjson(string jsonString)
    {
        data = JsonMapper.ToObject<WordsStruct[]>(jsonString);
    }

    public void SearchWord()
    {
        StartCoroutine(FindWord(guess));
    }

    public void CheckScore(string word)
    {
        StartCoroutine(FindWord(word.ToUpper(), false));
    }

    public IEnumerator FindWord(string word)
    {
        score = 0;
        int i = 0;

        while (i < data.Length)
        {
            if(word == data[i].word)
            {
                wordFound = true;
                break;
            }
            else if(word != data[i].word)
            {
                i++;
            }
        }

        yield return null;

        if(wordFound == true)
        {
            score = word.Length;

            if(word.Length == 9)
            {
                score = 18;
            }

            for (int lett = 0; lett < word.Length; lett++)
            {
                AddLetterAnswer(word[lett] + "");
            }

            ScoreManager.AddScore(score);
            conundrum.text = (word + " is in! Points awarded: " + score);
        }
        else if(wordFound == false)
        {
            //check dictionary?
            conundrum.text = (word + " was not found: no points will be awarded");
        }
        //show possibilities
        yield return null;
        yield return new WaitForSeconds(4);

        conundrum.text = "Longest word possible: " + data[0].word + ", for " + data[0].length + " points";

        yield return new WaitForSeconds(4);

        img.SetActive(true);
        // loop over 1 second
        for (float alpha = 0; alpha <= 1; alpha += Time.unscaledDeltaTime)
        {
            // set color with i as alpha
            img.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        gameManager.scoreManager.GetComponent<JimmyCarr>().NextRound();

        yield return null;
    }
    public IEnumerator FindWord(string word, bool awardScore)
    {
        guess = word.ToUpper();
        score = 0;
        int i = 0;

        while (i < data.Length)
        {
            if (word == data[i].word)
            {
                wordFound = true;
                break;
            }
            else if (word != data[i].word)
            {
                i++;
            }
        }

        if (wordFound == true)
        {
            score = word.Length;

            if (word.Length == 9)
            {
                score = 18;
            }

            conundrum.text = (word + " was found! value: " + score + ". Highest Possible: " + data[0].length);
        }
        else if (wordFound == false)
        {
            //check dictionary?
            conundrum.text = (word + " was not found: no points will be awarded");
        }

        yield return null;
    }
}
public class Words
{
    [SerializeField]
    public WordsStruct[] words;
}

public class WordsStruct
{
    public string word;
    public int length;
    public bool conundrum;
}
