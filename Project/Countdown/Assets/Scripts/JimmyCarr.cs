using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JimmyCarr : MonoBehaviour
{
    public SceneController sceneController;
    //scene names
    public string lettersSingle;
    public string numbersSingle;
    public string end;

    public int numberofRounds;  //max number of rounds
    public int roundNumber; //current round

    /// <summary>
    /// "numbers" for numbers only, "letters" for letters only, "normal" for alternating rounds
    /// </summary>
    public string roundTypes;

    public Dropdown typeDropdown;
    public InputField roundNumberField;

    public void ModeSelect()
    {
        if(typeDropdown.value == 0)
        {
            roundTypes = "normal";
        }
        else if(typeDropdown.value == 1)
        {
            roundTypes = "letters";
        }
        if(typeDropdown.value == 2)
        {
            roundTypes = "numbers";
        }
    }

    public void SetRoundNumber()
    {
        numberofRounds = int.Parse(roundNumberField.text);
    }

    public void StartStandardMode()
    {
        roundTypes = "normal";
        numberofRounds = 4;
        sceneController.LoadScene(lettersSingle);
        roundNumber = 1;
    }

    public void StartCustomMode()
    {
        SetRoundNumber();
        roundNumber = 1;
        ModeSelect();

        if(roundTypes == "normal" || roundTypes == "letters")
        {
            sceneController.LoadScene(lettersSingle);
        }
        else if(roundTypes == "numbers")
        {
            sceneController.LoadScene(numbersSingle);
        }
    }

    public void NextRound()
    {
        if(roundNumber == numberofRounds)
        {
            sceneController.LoadScene("Final");
            roundNumber = 0;
        }
        else if(roundNumber < numberofRounds)
        {
            if(roundTypes == "normal")
            {
                if (SceneManager.GetActiveScene().name == lettersSingle)
                {
                    roundNumber++;
                    sceneController.LoadScene(numbersSingle);
                }
                else if(SceneManager.GetActiveScene().name == numbersSingle)
                {
                    roundNumber++;
                    sceneController.LoadScene(lettersSingle);
                }
            }
            else if(roundTypes == "letters")
            {
                roundNumber++;
                sceneController.LoadScene(lettersSingle);
            }
            else if(roundTypes == "numbers")
            {
                roundNumber++;
                sceneController.LoadScene(numbersSingle);
            }
        }
    }
}
