using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class AmmoPickup : MonoBehaviour
{
    [SerializeField]public enum Ammo { Pistol, Rilfe, Shotgun};
    public Ammo Weapon;
    public int RefillAmount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "PlayerObj")
        {
            switch(Weapon)
            {
                case Ammo.Pistol:
                    //var gun = GameObject.FindGameObjectWithTag("Pistol").GetComponent<Gun>();
                    var temp = Resources.FindObjectsOfTypeAll<Gun>();
                    foreach(Gun gun in temp)
                    {
                        if(gun.gameObject.CompareTag("Pistol"))
                        { gun.AmmoHeld += RefillAmount; }
                    }
                    break;

                case Ammo.Rilfe:
                    //var gun1 = GameObject.FindGameObjectWithTag("Rilfe").GetComponent<Gun>();
                    var temp1 = Resources.FindObjectsOfTypeAll<Gun>();
                    foreach (Gun gun in temp1)
                    {
                        if (gun.gameObject.CompareTag("Rilfe"))
                        { gun.AmmoHeld += RefillAmount; }
                    }
                    break;

                case Ammo.Shotgun:
                    //var gun2 = GameObject.FindGameObjectWithTag("Shotgun").GetComponent<Gun>();
                    var temp2 = Resources.FindObjectsOfTypeAll<Gun>();
                    foreach (Gun gun in temp2)
                    {
                        if (gun.gameObject.CompareTag("Shotgun"))
                        { gun.AmmoHeld += RefillAmount; }
                    }
                    break;

                default:
                    Debug.Log("Player doesn't have this weapon yet.");
                    break;

            }
            gameObject.SetActive(false);
        }
    }
}
