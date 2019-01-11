using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Generator : MonoBehaviour
{
    public Text result;
    public bool calculating = false;
    private long estimate = 5429503678976;
    private long estimateStart = 0;
    public Image estimateBar;

    public List<string> letters = new List<string>();
    [SerializeField]
    public int spot1
    {
        get { return _spot1; }
        set
        {
            _spot1 = value; if (_spot9 + 1 > letters.Count)
            {
                calculating = false;
            }
        }
    }
    [SerializeField]
    public int spot2
    {
        get { return _spot2; }
        set
        {
            _spot2 = value; if (_spot2 + 1 > letters.Count)
            {
                spot1++;
                spot2 = 0;
            }
        }
    }
    [SerializeField]
    public int spot3
    {
        get { return _spot3; }
        set
        {
            _spot3 = value; if (_spot3 + 1 > letters.Count)
            {
                spot2++;
                spot3 = 0;
            }
        }
    }
    [SerializeField]
    public int spot4
    {
        get { return _spot4; }
        set
        {
            _spot4 = value; if (_spot4 + 1 > letters.Count)
            {
                spot3++;
                spot4 = 0;
            }
        }
    }
    [SerializeField]
    public int spot5
    {
        get { return _spot5; }
        set
        {
            _spot5 = value; if (_spot5 + 1 > letters.Count)
            {
                spot4++;
                spot5 = 0;
            }
        }
    }
    [SerializeField]
    public int spot6
    {
        get { return _spot6; }
        set
        {
            _spot6 = value; if (_spot6 + 1 > letters.Count)
            {
                spot5++;
                spot6 = 0;
            }
        }
    }
    [SerializeField]
    public int spot7
    {
        get { return _spot7; }
        set
        {
            _spot7 = value; if (_spot7 + 1 > letters.Count)
            {
                spot6++;
                spot7 = 0;
            }
        }
    }
    [SerializeField]
    public int spot8
    {
        get { return _spot8; }
        set
        {
            _spot8 = value; if (_spot8 + 1 > letters.Count)
            {
                spot7++;
                spot8 = 0;
            }
        }
    }
    [SerializeField]
    public int spot9
    {
        get { return _spot9; }
        set
        {
            _spot9 = value; if (_spot9 + 1 > letters.Count)
            {
                spot8++;
                spot9 = 0;
            }
        }
    }

    public StreamWriter writer;
    public string stage;
    [SerializeField]
    private int _spot1;
    [SerializeField]
    private int _spot2;
    [SerializeField]
    private int _spot3;
    [SerializeField]
    private int _spot4;
    [SerializeField]
    private int _spot5;
    [SerializeField]
    private int _spot6;
    [SerializeField]
    private int _spot7;
    [SerializeField]
    private int _spot8;
    [SerializeField]
    private int _spot9;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(GenerateResults());
	}
	
    IEnumerator GenerateResults()
    {
        estimateStart = 0;
        calculating = true;

        string path = "Assets/Combination.txt";

        writer = new StreamWriter(path, true);

        while (calculating == true)
        { 

            stage = letters[spot1] + letters[spot2] + letters[spot3] + letters[spot4] + letters[spot5] + letters[spot6] + letters[spot7] + letters[spot8] + letters[spot9];
            result.text = stage;
            writer.WriteLine(stage);
            yield return null;

            spot9++;
            estimateStart++;


            estimateBar.fillAmount = ((estimateStart * 1.0f) / estimate);

            if(calculating == false)
            {
                writer.Close();
                break;
            }
        }
        yield return null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        writer.Close();
    }
}
