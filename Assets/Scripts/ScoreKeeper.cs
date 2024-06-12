using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    private int score = 0;
    public TextMeshProUGUI output;

    public void ScoreUpdate(int s)
    {
        score += s;
        output.text = "Score: " + score.ToString();
    }
}
