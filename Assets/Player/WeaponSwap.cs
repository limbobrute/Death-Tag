using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public int selectedWeapon = 0;
    private int allowedWeapons = 0;

    // Start is called before the first frame update
    void Start()
    {
        selectWeapon();    
    }

    // Update is called once per frame
    void Update()
    {
        int pre = selectedWeapon;
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
            { selectedWeapon = 0; }
            else
            { selectedWeapon++; }
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            { selectedWeapon = transform.childCount - 1; }
            else
            { selectedWeapon--; }
        }

        if(pre != selectedWeapon)
        { selectWeapon(); }
    }

    public void Pickup()
    {
        selectedWeapon = transform.childCount - 1;
        selectWeapon();
    }


    void selectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i == selectedWeapon)
            { weapon.gameObject.SetActive(true); }
            else 
            { weapon.gameObject.SetActive(false); }
            i++;

        }
    }
}
