using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public bool Dead = false;
    public TextMeshProUGUI GameOverText;
    public void Update()
    {

        if(Dead == true)
        { GameOverText.SetText("GAME OVER"); }
    }

}
