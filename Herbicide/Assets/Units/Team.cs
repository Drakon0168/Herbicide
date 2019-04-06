using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamType
{
    Plants,
    Robots
}

public class Team : MonoBehaviour
{
    private List<Unit> units;
    [SerializeField]
    private GameObject commanderPrefab;
    private Commander commander;
    [SerializeField]
    private int currency;
    [SerializeField]
    private int currencyPerTurn;
    private Vector2 startPosition;
    public TeamType teamType;

    /// <summary>
    /// The current amount of currency available
    /// </summary>
    public int Currency
    {
        get { return currency; }
    }

    /// <summary>
    /// The amount of currency gained per turn
    /// </summary>
    public int CurrencyPerTurn
    {
        get { return currencyPerTurn; }
    }

    /// <summary>
    /// The list of units on the team
    /// </summary>
    public List<Unit> Units
    {
        get { return units; }
    }
    
    void Start()
    {
        units = new List<Unit>();
    }

    /// <summary>
    /// Sets the start position for the team and spawns a commander at that location
    /// </summary>
    /// <param name="position"></param>
    public void SetStart(Vector2 position)
    {
        startPosition = position;

        commander = Instantiate(commanderPrefab, new Vector3(startPosition.x + 0.5f, startPosition.y + 0.5f, -0.5f), Quaternion.Euler(0, 0, 0), transform).GetComponent<Commander>();
        commander.Position = startPosition;
        commander.Team = teamType;
        Board.Tiles[startPosition].OccupyingUnit = commander;
    }
}
