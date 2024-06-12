using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

/*
 * Created on: 03/17/2022
 * Created by: William HP.
 * Last Edited on 03/19/2022
 */

public class FesteringGhoulAI : MonoBehaviour
{
    private NavMeshAgent Ghoul;
    private GameObject Player;
    private Transform target;
    private Transform escape;
    private Animator animator;
    private GameObject Enemey;
    private Collider Mcollider;
    private GameManger GameManger;
    private GameObject[] RunAway;
    private Transform lap;
    private ScoreKeeper ScoreKeeper;
    private TextMeshProUGUI GameOver;
    private GameManger GameMagner;
    private float n = 0f;
    private int count = 0;
    public int points = 4;
    public bool ScoreUpdated = false;
    public bool Hunt = false;
    public bool Marked = false;
    public bool JustMarked = false;
    public float HP = 0f;
    public float HPMax = 0f;
    public float run = 0f;
    public float walk = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Ghoul = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player");
        target = Player.transform;
        animator = GetComponent<Animator>();
        Enemey = transform.gameObject;
        Mcollider = GetComponent<MeshCollider>();
        GameManger = GameObject.Find("InputManger").GetComponent<GameManger>();
        RunAway = GameObject.FindGameObjectsWithTag("RunAway");
        lap = RunAway[0].transform;
        ScoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
        GameOver = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Ghoul.speed = 0f;
            animator.SetBool("Dead", true);
            Dead();
            Mcollider.enabled = false;
        }
        else if (HP > 0 && GameManger.RunAway == false)
        {
            Ghoul.SetDestination(target.position);
            //transform.LookAt(target);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("spawn"))
            { Ghoul.speed = 0; }

            else if (Vector3.Distance(target.position, transform.position) >= 15)
            {
                Ghoul.speed = run;
                animator.SetFloat("Blend", 0.0f);
            }

            else if (Vector3.Distance(target.position, transform.position) <= 15 && Vector3.Distance(target.position, transform.position) > 2.5)
            {
                Ghoul.speed = walk;
                animator.SetFloat("Blend", 0.5f);
            }

            else if (Vector3.Distance(target.position, transform.position) <= 2.5)
            {
                Ghoul.speed = 0;
                animator.SetFloat("Blend", 1f);
                animator.SetBool("Attack", true);
                Marking();
                StartCoroutine(Attack());
            }
        }
        else if (HP > 0 && GameManger.RunAway == true && Marked == false)
        {
            if (escape == null)
            {
                int r = Random.Range(0, RunAway.Length);
                escape = RunAway[r].transform;
                Ghoul.speed = run;
                animator.SetFloat("Blend", 0.0f);
            }
            else
            {
                Ghoul.SetDestination(escape.position);
                if (Vector3.Distance(escape.position, transform.position) <= 5f)
                { escape = null; }
            }
        }
        else if (HP > 0 && Marked == true && Hunt == false)
        {
            Ghoul.SetDestination(lap.position);
            Ghoul.speed = run;
            if (Vector3.Distance(lap.position, transform.position) <= 5f && count < RunAway.Length - 1)
            { lap = RunAway[count].transform; count += 1; }
            else if (count == RunAway.Length - 1 && Vector3.Distance(lap.position, transform.position) <= 5f)
            { Hunt = true; }
        }
        else if (HP > 0 && Marked == true && Hunt == true)
        {
            Ghoul.SetDestination(target.position);
            if (Vector3.Distance(target.position, transform.position) >= 15)
            {
                Ghoul.speed = run;
                animator.SetFloat("Blend", 0.0f);
            }

            else if (Vector3.Distance(target.position, transform.position) <= 15 && Vector3.Distance(target.position, transform.position) > 2.5)
            {
                Ghoul.speed = walk;
                animator.SetFloat("Blend", 0.5f);
            }

            else if (Vector3.Distance(target.position, transform.position) <= 2.5)
            {
                Ghoul.speed = 0;
                animator.SetFloat("Blend", 1f);
                animator.SetBool("Attack", true);
                StartCoroutine(Attack());
            }
        }
    }

    private void ChangeMark()
    { Marked = !Marked; }

    private void Marking()
    { JustMarked = !JustMarked; }

    IEnumerator Attack()
    {
        float n = 0;
        n += Time.deltaTime;
        if (n <= 1.20f)
        { yield return null; }

        /*if (Marked == false && Vector3.Distance(target.position, transform.position) <= 2.5)
        {
            Marking();
            GameManger.RunAway = true;
            ChangeMark();
            transform.localScale += new Vector3(0.25f, 0.25f, 0.25f);
            n = 0f;
        }
        else if (Marked == true && JustMarked == false && Vector3.Distance(target.position, transform.position) <= 2.5)
        { 
            Player.GetComponent<PlayerController>().enabled = false;
            GameOver.SetText("Game Over.");
            GameManger.enabled = false;
            Debug.Log("You're dead.");
        }*/
        if (Marked == true && JustMarked == false && Vector3.Distance(target.position, transform.position) <= 2.5)
        {
            Player.GetComponent<PlayerController>().enabled = false;
            GameOver.SetText("Game Over.");
            GameManger.enabled = false;
            Debug.Log("You're dead.");
        }
        else if (Marked == false && Vector3.Distance(target.position, transform.position) <= 2.5)
        {
            Marking();
            GameManger.RunAway = true;
            ChangeMark();
            transform.localScale += new Vector3(0.25f, 0.25f, 0.25f);
            n = 0f;
        }
        animator.SetBool("Attack", false);
        Marking();
    }

    private void Dead()
    {
        if (ScoreUpdated == false)
        { ScoreKeeper.ScoreUpdate(points); ScoreUpdated = true; }
        if (Marked == true)
        {
            ChangeMark();
            transform.localScale -= new Vector3(0.25f, 0.25f, 0.25f);
            GameManger.RunAway = false;
        }
        if (n < 10f)
        { n += Time.deltaTime; }
        else if (n >= 10f)
        {
            count = 0;
            this.HP = this.HPMax;
            animator.SetBool("Dead", false);
            Enemey.SetActive(false);
        }
    }
}
