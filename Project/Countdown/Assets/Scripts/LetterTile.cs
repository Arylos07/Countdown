using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterTile : MonoBehaviour
{
    public string letter;
    public Text letterText;

    public void Start()
    {
        letterText.text = letter.ToUpper();
    }
}
