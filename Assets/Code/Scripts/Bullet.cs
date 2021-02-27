using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject Target;
    private int Damage;
    private Unit PlayerUnit;
    private float speed = 3f;

    public void Seek(GameObject _target)
    {
        Target = _target;
    }

    public void SetDMGValue(float _dmg)
    {
        Damage = (int)_dmg;
    }

    void Update()
    {

        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = Target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            if (Target.CompareTag("Enemy"))
            {
                Target.GetComponent<Enemy>().TakeDamage(Damage);
            }
            if (Target.CompareTag("Unit"))
            {
                Target.GetComponent<Unit>().TakeDamage(Damage);
            }
            if (Target.CompareTag("Building"))
            {
                Target.GetComponent<Buildings.Building>().TakeDamage(Damage);
            }
            Destroy(gameObject);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame,Space.World);
        transform.LookAt(Target.transform);

    }
}
