using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDetection : MonoBehaviour
{
    private float Range;
    public bool IsHit;
    private UnitWeapon HouseWeapon;

    private void Start()
    {
        HouseWeapon = GetComponentInParent<UnitWeapon>();
        Range = HouseWeapon.DetectionRange;
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Range))
        {
            if (hit.transform.tag == "Enemy")
            {
                HouseWeapon.GetComponent<UnitWeapon>().GetTarget(hit.transform.gameObject);
                IsHit = true;
            }
            else
            {
                IsHit = false;
            }
        }
    }
}
