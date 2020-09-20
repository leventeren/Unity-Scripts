using UnityEngine;
using System.Collections;
using Ballistics;

public class LivingBox : LivingEntity {

    /// <summary>
    /// called when Health is 0
    /// </summary>
    public override void OnDeath()
    {
        //Debug.Log("Dead");
    }

    /// <summary>
    /// called when health value changed
    /// </summary>
    /// <param name="amount"></param>
    public override void OnHealthChanged(float amount)
    {
        Debug.Log(transform.name + " took " + (-amount).ToString() + " damage");
    }
}
