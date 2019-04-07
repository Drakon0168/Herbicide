using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CommanderDeath();

public class Commander : Unit
{
    public CommanderDeath commanderDeath;

    protected override void Start()
    {
        base.Start();
    }

    public override void Die()
    {
        if(commanderDeath != null)
        {
            commanderDeath();
        }

        base.Die();
    }
}
