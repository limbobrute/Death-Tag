using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider))]
public class WeaponPickup : MonoBehaviour
{
    public GameObject WeaponPrefab;
    public GameObject WeaponHolder;
    public new Vector3 PlacePoint;

    private void OnTriggerEnter(Collider other)
    {
        var temp = other.gameObject;
        if(temp.name =="Player2.0")
        {
            var newWeapon = Instantiate(WeaponPrefab);
            var gun = newWeapon.GetComponent<Gun>();
            newWeapon.transform.localPosition = PlacePoint;
            newWeapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            newWeapon.transform.parent = WeaponHolder.transform;
            //gun.animator = WeaponHolder.GetComponent<Animator>();
            gun.fpsCam = GameObject.Find("Main Camera").GetComponent<Camera>();
            gun.text = GameObject.Find("Ammo").GetComponent<TextMeshProUGUI>();
            gun.AmmoLeft = GameObject.Find("AmmoHeld").GetComponent<TextMeshProUGUI>();
            WeaponHolder.GetComponent<WeaponSwap>().Pickup();
            this.gameObject.SetActive(false);
        }
    }
}
