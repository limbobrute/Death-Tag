using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

/*
 * Created on: 03/17/2022
 * Created by: William HP.
 * Last Edited on 05/23/2023
 */

public class GhoulAI : MonoBehaviour
{
    #region PROPERTIES
    private NavMeshAgent Ghoul;
    private GameObject Player;
    private Transform target;
    private Transform escape;
    private Animator animator;
    private GameObject Enemey;
    private GameManger GameManger;
    private GameObject[] RunAway;
    private Transform lap;
    private GameOver GameOver;
    private float n = 0f;
    private int count = 0;
    public int points = 1;
    public bool ScoreUpdated = false;
    public bool Hunt = false;
    public bool Marked = false;
    public bool JustMarked = false;
    public float HP = 0f;
    public float HPMax = 0f;
    public float run = 0f;
    public float walk = 0f;
    public float AttackDelay = 0f;
    private AudioSource play;
    public AudioClip FootSteps;
    public AudioClip RandSound;
    public GameObject[] AmmoPickup;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Ghoul = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player2.0");
        target = Player.transform;
        animator = GetComponent<Animator>();
        Enemey = transform.gameObject;
        //Mcollider = GetComponent<MeshCollider>();
        GameManger = GameObject.Find("Mangers").GetComponent<GameManger>();
        RunAway = GameObject.FindGameObjectsWithTag("RunAway");
        lap = RunAway[0].transform;
        GameOver = GameObject.Find("GameOver").GetComponent<GameOver>();
        play = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!play.isPlaying)
        {
            int rand = Random.Range(0, 99);
            if(rand <= 4)
            { play.PlayOneShot(RandSound); }
            else
            { play.PlayOneShot(FootSteps); }
        }
        #region UNMARKED
        if (HP <= 0 && animator.GetBool("Dead") == false)//This ghoul is dead, now it's acting like it before anything else can occur
        {
            play.Stop();
            if (ScoreUpdated == false)
            { GameManger.Points += points; ScoreUpdated = true; }
            if (Marked == true)
            {
                ChangeMark();
                GameManger.RunAway = false;
            }
            Ghoul.speed = 0f;
            animator.SetBool("Dead", true);
            //Mcollider.enabled = false;
            StartCoroutine(Dead());
        }
        else if (HP > 0 && GameManger.RunAway == false)//Time to hunt the player and be the first to smack them
        {
            Ghoul.SetDestination(target.position);//Make sure that we always know were the player is
            //transform.LookAt(target);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("spawn"))//Are we spawnning in?
            { Ghoul.speed = 0; }

            else if (Vector3.Distance(target.position, transform.position) >= 15)//It's a hike to the player, best pick up the pace
            {
                Ghoul.speed = run;
                animator.SetFloat("Blend", 0.0f);
            }

            else if (Vector3.Distance(target.position, transform.position) <= 15 && Vector3.Distance(target.position, transform.position) > 2.5)//Player is close enough, best make sure we don't over take them
            {
                Ghoul.speed = walk;
                animator.SetFloat("Blend", 0.5f);
            }

            else if (Vector3.Distance(target.position, transform.position) <= 2.5 && animator.GetBool("Attack") == false)//Now's our chance! Let's get that player!!
            {
                Ghoul.speed = 0;//I need to stop moving for this
                animator.SetFloat("Blend", 1f);
                animator.SetBool("Attack", true);//The smack
                StartCoroutine(Attack());
            }
        }
        else if (HP > 0 && GameManger.RunAway == true && Marked == false)//Darn, some other ghoul got the player, now we gotta run away from them
        {
            if (escape == null)//I need to find a place to run away to
            {
                int r = Random.Range(0, RunAway.Length);
                escape = RunAway[r].transform;
                Ghoul.speed = run;
                animator.SetFloat("Blend", 0.0f);
            }
            else//Don't worry, I've got a good place to run to
            {
                Ghoul.SetDestination(escape.position);
                if (Vector3.Distance(escape.position, transform.position) <= 5f)//I've gotten to the place, just realized this is a bad place to flee to
                { escape = null; }
            }
        }
        #endregion

        #region MARKED
        else if (HP > 0 && Marked == true && Hunt == false)//Ah HA! I got the player. Now I need to flee to regain my powers
        {
            animator.SetFloat("Blend", 0.5f);
            Ghoul.SetDestination(lap.position);
            Ghoul.speed = run;
            if (Vector3.Distance(lap.position, transform.position) <= 5f && count < RunAway.Length - 1)//Nope, still haven't regained my power
            { lap = RunAway[count].transform; count += 1; }
            else if(count == RunAway.Length - 1  && Vector3.Distance(lap.position, transform.position) <= 5f)//I've regained my powers, now to hunt down that nasty player
            { Hunt = true; }
        }
        else if(HP > 0 && Marked == true && Hunt == true)
        {
            Ghoul.SetDestination(target.position);//I know were you are player

            if (Vector3.Distance(target.position, transform.position) >= 15)//Man, that player is far. Best run
            {
                Ghoul.speed = run;
                animator.SetFloat("Blend", 0.0f);
            }

            else if (Vector3.Distance(target.position, transform.position) <= 15 && Vector3.Distance(target.position, transform.position) > 2.5)//There they are. I've got them now
            {
                Ghoul.speed = walk;
                animator.SetFloat("Blend", 0.5f);
            }

            else if (Vector3.Distance(target.position, transform.position) <= 2.5 && animator.GetBool("Attack") == false)//Your mine!
            {
                Ghoul.speed = 0;
                animator.SetFloat("Blend", 1f);
                animator.SetBool("Attack", true);
                StartCoroutine(Attack());
            }
        }
        #endregion

    }

    #region OTHERMETHODS
    private void ChangeMark()
    { 
        Marked = !Marked;
        if(Marked)
        { transform.localScale += new Vector3(0.5f, 0.5f, 0.5f); }
        else if(!Marked)
        { transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f); }
    }

    private void Marking()
    { JustMarked = !JustMarked; }

    private void AmmoDrop()
    {
        int rand = Random.Range(0, 99);
        if (rand <= 33)
        {
            int AmmoDrop = Random.Range(0, 2);
            Instantiate(AmmoPickup[AmmoDrop], new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, 0, 0));
        }
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(5);
        this.HP = this.HPMax;
        //Mcollider.enabled = true;
        animator.SetBool("Dead", false);
        Enemey.SetActive(false);
        ScoreUpdated = false;
        AmmoDrop();
    }

    IEnumerator Attack()
    {
        n += Time.deltaTime;
        if (n <= AttackDelay)
        { yield return null; }

        if (Marked == false && Vector3.Distance(target.position, transform.position) <= 2.5)
        {
            Marking();
            GameManger.RunAway = true;
            ChangeMark();
            Debug.Log("You've been marked");
            n = 0f;
        }
        else if (Marked == true && JustMarked == false && Vector3.Distance(target.position, transform.position) <= 2.5)
        {
            Player.GetComponent<Move>().enabled = false;
            Player.GetComponentInChildren<CameraMove>().enabled = false;
            GameObject.Find("WeaponHolder").SetActive(false);
            GameOver.Dead = true;
            //GameManger.enabled = false;
            Debug.Log("You're dead.");
        }
        animator.SetBool("Attack", false);
        Marking();
    }
    #endregion
}
