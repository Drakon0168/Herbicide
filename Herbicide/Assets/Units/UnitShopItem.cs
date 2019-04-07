using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitShopItem : MonoBehaviour
{
    [SerializeField]
    private Image unitImage;
    [SerializeField]
    private Text unitName;
    [SerializeField]
    private Text unitDescription;
    [SerializeField]
    private Text unitCost;

    /// <summary>
    /// Displays all of the units in the list for purchase
    /// </summary>
    /// <param name="units"></param>
    public void DisplayShop(Unit unit)
    {
        unitImage.sprite = unit.UnitImage;
        unitName.text = unit.UnitName;
        unitDescription.text = unit.UnitDescription;
        unitCost.text = "Cost: " + unit.Cost;
    }
}
