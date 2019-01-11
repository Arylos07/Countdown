using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using B83.ExpressionParser;
using UnityEngine.SceneManagement;

public class MathSolver : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject img;
    public Text result;
    //Set up global variables
    public static System.Random randy = new System.Random();
    ExpressionParser parser = new ExpressionParser();
    public int[] numbers = new int[6];
    public int target = 0;
    public static int bestSoFar = 0;
    private int attemps = 0;
    private string endResult;

    /// <summary>
    /// Used to parse string expressions for use in checking player-entered equations. To be used for the future.
    /// </summary>
    /// <param name="input"></param>
    public void ParseInput(string input)
    {
        char[] x = "*".ToCharArray();
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i].ToString() == "x" || input[i].ToString() == "X")
            {
                input = input.Replace(input[i], x[0]);
            }
        }

        Expression exp = parser.EvaluateExpression(input);

        print("exp: " + input + " = " + exp.Value);
        print(Mathf.Abs(target - (float)exp.Value));

        if (Mathf.Abs(target - (float)exp.Value) == 0)
        {
            ScoreManager.AddScore(10);
            print("Target reached - Points: 10");
        } 
        else if(Mathf.Abs(target - (float)exp.Value) <= 10 && Mathf.Abs(target - (float)exp.Value) > 0)
        {
            ScoreManager.AddScore(7);
            print("off by " + Mathf.Abs(target - (float)exp.Value) + ". 7 points awarded");

        }
        else if(Mathf.Abs(target - (float)exp.Value) > 10)
        {
            print("off by " + Mathf.Abs(target - (float)exp.Value) + ". No points awarded");
        }
    }

    public void SubmitAnswer(int answer)
    {
        if (Mathf.Abs(target - answer) == 0)
        {
            ScoreManager.AddScore(10);
            result.text = ("Target reached - Points: 10");
        }
        else if (Mathf.Abs(target - answer) <= 30 && Mathf.Abs(target - answer) > 0)
        {
            ScoreManager.AddScore(7);
            result.text = ("off by " + Mathf.Abs(target - answer) + ". 7 points awarded");

        }
        else if (Mathf.Abs(target - answer) <= 50 && Mathf.Abs(target - answer) > 30)
        {
            ScoreManager.AddScore(5);
            result.text = ("off by " + Mathf.Abs(target - answer) + ". 5 points awarded");
        }
        else if (Mathf.Abs(target - answer) > 50)
        {
            result.text = ("off by " + Mathf.Abs(target - answer) + ". No points awarded");
        }

        StartCoroutine(NextRound());
    }

    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(4);

        result.text = endResult;

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
    }

    public IEnumerator CalculateSolutions()
    {
        //create a loop to solve the puzzle
        while (true)
        {

            //mix up the numbers
            int num = randy.Next(6);
            int temp = numbers[num];
            numbers[num] = numbers[0];
            numbers[0] = temp;

            //create a running total
            int thisTotal = numbers[0];


            //text showing the solution (start with the first number)
            string solution = numbers[0].ToString();

            //we may not want to use all the numbers - choose how many...
            int numbersToUse = randy.Next(1, 7);

            //loop for each number we're going to use
            for (int i = 1; i < numbersToUse; i++)
            {

                //choose an operation + - x /
                int operation = randy.Next(4);
                if (operation == 0)
                {
                    thisTotal += numbers[i];
                    solution += " + " + numbers[i].ToString();
                }
                if (operation == 1)
                {
                    thisTotal -= numbers[i];
                    solution += " - " + numbers[i].ToString();
                }
                if (operation == 2)
                {
                    thisTotal *= numbers[i];
                    solution += " x " + numbers[i].ToString();
                }
                if (operation == 3)
                {
                    if (thisTotal % numbers[i] != 0) continue;  //fraction?
                    thisTotal /= numbers[i];
                    solution += " / " + numbers[i].ToString();
                }

            }

            //update output text
            solution += " = " + thisTotal.ToString();

            //is this our best solution so far?
            if (Mathf.Abs(target - thisTotal) < Mathf.Abs(target - bestSoFar))
            {
                bestSoFar = thisTotal;
            }

            //is this target?
            if (thisTotal == target)
            {
                endResult = "Solution found: " + solution;
                break;
            }

            attemps++;
            //how long so far
            if (attemps >= 100000)
            {
                endResult = ("Closest solution: " + solution + ", " + (Mathf.Abs(target - bestSoFar)) + " away.");
                break;
            }
        }

        gameManager.answerInputfield.interactable = true;
        //print result
        yield return null;
    }

}

