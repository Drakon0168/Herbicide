using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelliChopper : Unit
{
    protected override void Start()
    {
        base.Start();
        damageResistance.Add(DamageType.Fire, 1);
        damageResistance.Add(DamageType.Acid, 0);
    }
}
