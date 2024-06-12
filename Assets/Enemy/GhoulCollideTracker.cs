using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulCollideTracker : MonoBehaviour
{
    public GhoulAI thisGhoul;
    private void Start()
    {
        gameObject.tag = "Enemy";
    }
}
