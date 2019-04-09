using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void UnitBought();

public class UnitShop : MonoBehaviour
{
    [SerializeField]
    private GameObject shopItemPrefab;
    [SerializeField]
    private GameObject closeShopButton;
    private Dictionary<Team, List<UnitShopItem>> shopItems;
    public UnitBought unitBought;

    // Start is called before the first frame update
    public void SetupShop(List<Team> teams)
    {
        if(shopItems == null)
        {
            shopItems = new Dictionary<Team, List<UnitShopItem>>();
        }

        foreach (Team team in teams)
        {
            shopItems.Add(team, new List<UnitShopItem>());
            int topY = 125 + (team.unitPrefabs.Count * -250) / 2;

            for (int i = 0; i < team.unitPrefabs.Count; i++)
            {
                UnitShopItem newItem = Instantiate(shopItemPrefab, transform).GetComponent<UnitShopItem>();
                newItem.SetupItem(team.unitPrefabs[i], team, this);
                newItem.DisplayShop();
                shopItems[team].Add(newItem);

                newItem.GetComponent<RectTransform>().position += new Vector3(0, topY + i * 250, 0);

                newItem.gameObject.SetActive(false);
            }
        }
    }
    
    /// <summary>
    /// Shows all of the items in the given teams shop and hides all others
    /// </summary>
    /// <param name="team">The team to show the items for</param>
    public void DisplayShop(Team team)
    {
        closeShopButton.SetActive(true);

        foreach(KeyValuePair<Team, List<UnitShopItem>> pair in shopItems)
        {
            if(pair.Key == team)
            {
                foreach(UnitShopItem item in pair.Value)
                {
                    item.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (UnitShopItem item in pair.Value)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Hides all store items
    /// </summary>
    public void HideShop()
    {
        closeShopButton.SetActive(false);

        foreach (KeyValuePair<Team, List<UnitShopItem>> pair in shopItems)
        {
            foreach (UnitShopItem item in pair.Value)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
