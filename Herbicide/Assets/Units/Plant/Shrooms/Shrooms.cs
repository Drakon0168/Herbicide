using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrooms : Unit
{
    protected override void Start()
    {
        base.Start();
        damageResistance.Add(DamageType.Acid, 1);
        damageResistance.Add(DamageType.Impact, 0);
    }
}
