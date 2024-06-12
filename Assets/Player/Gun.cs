using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * Created on: 03/17/2022
 * Created by: William HP.
 * Last Edited on 05/23/2023
 */

public class Gun : MonoBehaviour
{
    #region PROPERTIES
    int bulletLeft, bulletShot;
    bool shooting, readyToShoot, reloading;

    [Header("Fire rate")]
    [Tooltip("Time between acknowledge mouse clicks in seconds")] public float timeBetweenShot;
    [Tooltip("Time between bullets fired in seconds")] public float timebetweenShooting;
    [Tooltip("How many bullets are fired per mouse click")] public int bulletPerTap;
    [Tooltip("Total instances of potienal damage for every round shot off")][Range(1,20)]public int multishot = 1;
    public float reloadTime;
    public bool allowButtonHold;

    [Header("Bullet Properties")]
    public float range = 100f;
    public float impactForce = 25f;
    public float damage = 1f;
    public float spread;
    public int magzineSize;

    [Header("Other Properties")]
    public AudioSource play;
    public AudioClip fire;
    public AudioClip reload;
    public Animator animator;
    public Camera fpsCam;
    public ParticleSystem MuzzleFlash;
    public GameObject impactEffect;
    public GameObject EnemyImpactEffect;
    public TextMeshProUGUI text;
    public TextMeshProUGUI AmmoLeft;
    public int AmmoHeld = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        bulletLeft = magzineSize;
        readyToShoot = true;
    }

    private void OnEnable()
    {
        animator.SetBool("IsReload", false);
        reloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        text.SetText(bulletLeft + " / " + magzineSize);
        AmmoLeft.SetText("" + AmmoHeld);
    }

    private void MyInput()
    {
        if (allowButtonHold)
        { shooting = Input.GetKey(KeyCode.Mouse0); }
        else
        { shooting = Input.GetKeyDown(KeyCode.Mouse0); }

        if (Input.GetKeyDown(KeyCode.R) && bulletLeft < magzineSize && !reloading)
        { Reload(); }

        if (readyToShoot && shooting && !reloading && bulletLeft > 0)
        {
            bulletShot = bulletPerTap;
            Shoot();
        }
    }

    private void Shoot()//Fire! FIRE!!
    {
        readyToShoot = false;//We can't!!
        MuzzleFlash.Play();//Visual proof that the gun was fired
        play.PlayOneShot(fire);

        for (int i = 0; i < bulletPerTap; i++)
        {
            for (int j = 0; j < multishot; j++)
            {
                //Spread
                float x = Random.Range(-spread, spread);
                float y = Random.Range(-spread, spread);

                //Calculate Direction with Spread
                Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);//Which direction is the bullet traveling?
                                                                                    //MuzzleFlash.Play();//Visual proof that the gun was fired

                RaycastHit hit;
                Physics.Raycast(fpsCam.transform.position, direction, out hit, range);//Did we hit something?
                if (hit.rigidbody != null)//Oh boy we got something!!
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);//YEET!!!
                    GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);//Visual proof that we hit something
                    Destroy(impact, 15f);
                }
                else if (hit.transform != null && hit.transform.tag == "Enemy")//Yes! We got one! Now how dead is it?
                {
                    GameObject temp = hit.transform.gameObject;
                    GameObject hitpoint = Instantiate(EnemyImpactEffect, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                    Destroy(hitpoint, 3.5f);
                    //Let's check what we hit and how dead it is
                    /*if (temp.name == "Ghoul(Clone)")
                    { temp.GetComponent<GhoulAI>().HP -= damage; }
                    else if (temp.name == "ghoul_boss(Clone)")
                    { temp.GetComponent<GhoulAI>().HP -= damage; }
                    else if (temp.name == "ghoul_festering(Clone)")
                    { temp.GetComponent<GhoulAI>().HP -= damage; }
                    else if (temp.name == "ghoul_grotesque(Clone)")
                    { temp.GetComponent<GhoulAI>().HP -= damage; }
                    else if (temp.name == "ghoul_scavenger(Clone)")
                    { temp.GetComponent<GhoulAI>().HP -= damage; }*/
                    if(temp.GetComponent<GhoulCollideTracker>() != null)
                    { temp.GetComponent<GhoulCollideTracker>().thisGhoul.HP -= damage; }

                }

                GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                Destroy(impactGo, 15f);
            }
        }
        //Ah man, we don't have infinite ammo?
        bulletLeft--;
        bulletShot--;
        Invoke("ResetShot", timebetweenShooting);

        if (bulletShot > 0 && bulletLeft > 0)
        { Invoke("Shoot", timeBetweenShot); }
    }

    private void ResetShot()//I'm out! Reloading!!
    {readyToShoot = true;}

    private void Reload()
    {
        reloading = true;
        animator.SetBool("IsReload", true);
        //Invoke("ReloadFinished", reloadTime);
        StartCoroutine(ReloadFinished());

        Invoke("ResetShot", timebetweenShooting);

        if (bulletShot > 0 && bulletLeft > 0)
        { Invoke("Shoot", timeBetweenShot); }
    }

    /*private void ReloadFinished()//Good to go! Let's do this!
    {
        bulletLeft = magzineSize;
        sound[1].Play();
        reloading = false;
        animator.SetBool("Reloading", false);
    }*/

    IEnumerator ReloadFinished()
    {
        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("IsReload", false);
        yield return new WaitForSeconds(.5f);
        play.PlayOneShot(reload);
        var bulletShot = magzineSize - bulletLeft;
        var ReloadAmount = AmmoHeld - bulletShot;
        Debug.Log("Amount to reload is " + ReloadAmount);
        Debug.Log("Ammo held before reloading is " + AmmoHeld);
        if (AmmoHeld > ReloadAmount && ReloadAmount > 0)
        {
            Debug.Log("More ammo to go through");
            bulletLeft = magzineSize;
            AmmoHeld -= bulletShot;
        }
        else
        {
            Debug.Log("Loading up the last of the ammo");
            bulletLeft += AmmoHeld;
            AmmoHeld = 0;
        }
        reloading = false;

    }
}
