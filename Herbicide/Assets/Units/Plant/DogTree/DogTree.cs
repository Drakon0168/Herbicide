using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogTree : Unit
{
    protected override void Start()
    {
        base.Start();
        damageResistance.Add(DamageType.Impact, 1);
        damageResistance.Add(DamageType.Acid, 1);
        damageResistance.Add(DamageType.Fire, 0);
    }
}
