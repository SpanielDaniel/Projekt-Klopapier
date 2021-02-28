using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeapon : MonoBehaviour
{
    [SerializeField] private Transform[] RayDetectors;
    private Vector3 FirePoint;
    [SerializeField] private GameObject BulletPrefab;
    private float AttackPoints;
    private float AttackSpeed;
    private float CountDownShoot;
    private GameObject Target;
    public float DetectionRange ;
    [Range(10f, 160f)] public float DetectionAngle;

    public void Init(Unit _unit)
    {
        AttackPoints = _unit.GetAttack();
        AttackSpeed = _unit.GetAttackSpeed();
        DetectionRange = _unit.GetRange();
        Debug.Log(_unit.GetRange());
    }

    private void Start()
    {
        FirePoint = new Vector3(transform.position.x, transform.position.y, (transform.position.z + 0.5f));
    }

    private void Update()
    {
        SetDetectors();
        if (EnemyInRange())
        {
            if (CountDownShoot <= 0)
            {
                Shoot();
                CountDownShoot = AttackSpeed;
            }

            CountDownShoot -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Shoot at Target
    /// </summary>
    private void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(BulletPrefab, FirePoint, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(Target);
            bullet.SetDMGValue(AttackPoints);
        }
    }

    public void GetTarget(GameObject _target)
    {
        Target = _target;
    }

    private void SetDetectors()
    {
        float angleSteps = (DetectionAngle / RayDetectors.Length) / 2;
        for (int i = 0; i < RayDetectors.Length; i++)
        {
            RayDetectors[i].localRotation = Quaternion.AngleAxis(angleSteps * (i + 1), Vector3.up);
            RayDetectors[i+1].localRotation = Quaternion.AngleAxis(-angleSteps * (i + 1), Vector3.up);
            i++;
        }
    }

    private bool EnemyInRange()
    {
        for (int i = 0; i < RayDetectors.Length; i++)
        {
            if (RayDetectors[i].GetComponent<RayDetection>().IsHit)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.forward * DetectionRange + transform.position);
        for (int i = 0; i < RayDetectors.Length; i++)
        {
            Gizmos.DrawLine(transform.position, RayDetectors[i].forward * DetectionRange + RayDetectors[i].position);
        }
    }
}
