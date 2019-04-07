using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitDisplay : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text unitName;
    [SerializeField]
    private Text unitDescription;
    [SerializeField]
    private Text unitStats;
    private Unit displayedUnit;

    public void refreshDisplay()
    {
        DisplayUnit(displayedUnit);
    }

    public void DisplayUnit(Unit unit)
    {
        displayedUnit = unit;
        image.sprite = unit.UnitImage;
        unitName.text = unit.UnitName;
        unitDescription.text = unit.UnitDescription;
        string stats = "Health: " + unit.Health + "\nDamage: " + unit.Damage + "\nDamage Type\n";

        switch (unit.DamageType)
        {
            case DamageType.Impact:
                stats += "Impact";
                break;
            case DamageType.Fire:
                stats += "Fire";
                break;
            case DamageType.Acid:
                stats += "Acid";
                break;
        }

        stats += "\nResistances:\nImpact: ";

        if (unit.Resistances.ContainsKey(DamageType.Impact))
        {
            switch (unit.Resistances[DamageType.Impact])
            {
                case 0:
                    stats += "Weak";
                    break;
                case 1:
                    stats += "Strong";
                    break;
            }
        }
        else
        {
            stats += "None";
        }

        stats += "\nFire: ";

        if (unit.Resistances.ContainsKey(DamageType.Fire))
        {
            switch (unit.Resistances[DamageType.Fire])
            {
                case 0:
                    stats += "Weak";
                    break;
                case 1:
                    stats += "Strong";
                    break;
            }
        }
        else
        {
            stats += "None";
        }

        stats += "\nAcid: ";

        if (unit.Resistances.ContainsKey(DamageType.Acid))
        {
            switch (unit.Resistances[DamageType.Acid])
            {
                case 0:
                    stats += "Weak";
                    break;
                case 1:
                    stats += "Strong";
                    break;
            }
        }
        else
        {
            stats += "None";
        }

        unitStats.text = stats;
    }
}
