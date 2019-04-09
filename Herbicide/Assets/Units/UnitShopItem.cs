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
    private GameObject unitPrefab;
    private Team team;
    private UnitShop shop;

    public GameObject UnitPrefab
    {
        get { return unitPrefab; }
        set { unitPrefab = value; }
    }

    public void SetupItem(GameObject unitPrefab, Team team, UnitShop shop)
    {
        this.unitPrefab = unitPrefab;
        this.team = team;
        this.shop = shop;
    }

    /// <summary>
    /// Displays all of the units in the list for purchase
    /// </summary>
    /// <param name="units"></param>
    public void DisplayShop()
    {
        Unit unit = unitPrefab.GetComponent<Unit>();
        unitImage.sprite = unit.UnitImage;
        unitName.text = unit.UnitName;
        unitDescription.text = unit.UnitDescription;
        unitCost.text = "Cost: " + unit.Cost;
    }

    /// <summary>
    /// Attempts to buy the unit
    /// </summary>
    public void BuyUnit()
    {
        if(team.Currency >= unitPrefab.GetComponent<Unit>().Cost)
        {
            Unit newUnit = Instantiate(unitPrefab, team.transform).GetComponent<Unit>();
            newUnit.Position = Board.SelectedPosition;
            newUnit.currentMoveRange = 0;
            newUnit.Team = team.teamType;
            Board.Tiles[Board.SelectedPosition].OccupyingUnit = newUnit;
            team.Currency -= newUnit.Cost;
            team.AddUnit(newUnit);

            if(shop.unitBought != null)
            {
                shop.unitBought();
            }
        }
        
        Board.DeselectTiles();
        Board.SelectionState = SelectionState.Unit;
        shop.HideShop();
    }
}
