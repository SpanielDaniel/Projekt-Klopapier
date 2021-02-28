// File     : Bullet.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using Buildings;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // -------------------------------------------------------------------------------------------------------------

    #region Init

    // private -----------------------------------------------------------------------------------------------------

    private GameObject Target;
    private int Damage;
    private Unit PlayerUnit;
    private float speed = 3f;

    #endregion

    // -------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Set Target
    /// </summary>
    /// <param name="_target">Target Variable</param>
    public void Seek(GameObject _target)
    {
        Target = _target;
    }

    /// <summary>
    /// Set Damage for Bullet
    /// </summary>
    /// <param name="_dmg">Damage Variable</param>
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

        // If Bullet hit something Damage it
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

            Building building = Target.GetComponent<Building>();
            if (building != null)
            {
                building.TakeDamage(Damage);
            }
            Destroy(gameObject);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame,Space.World);
        transform.LookAt(Target.transform);

    }
}
