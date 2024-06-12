using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DummGhoulAI : MonoBehaviour
{
    private NavMeshAgent Ghoul;
    private GameObject Player;
    private Transform target;
    private Animator animator;
    public float run = 0f;
    public float walk = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Ghoul = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player");
        target = Player.transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
        }
    }
}
