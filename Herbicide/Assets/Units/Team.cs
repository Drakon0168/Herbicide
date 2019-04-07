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
    public List<GameObject> unitPrefabs;
    private Commander commander;
    private int currency;
    private int currencyPerTurn;
    private Vector2 startPosition;
    public TeamType teamType;

    /// <summary>
    /// The current amount of currency available
    /// </summary>
    public int Currency
    {
        get { return currency; }
        set { currency = value; }
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

        Board.endTurn += OnTurnEnd;
        Board.resetGame += ResetTeam;

        ResetTeam();
    }

    /// <summary>
    /// Called when the commander dies, ends the game
    /// </summary>
    public void CommanderDeath()
    {
        foreach(Unit unit in units)
        {
            unit.Die();
        }

        //TODO: add a delay for the end game animations to play
        Board.resetGame();
    }

    /// <summary>
    /// Called when the turn ends
    /// </summary>
    public void OnTurnEnd()
    {
        currency += currencyPerTurn;
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
        commander.commanderDeath += CommanderDeath;
        Board.Tiles[startPosition].OccupyingUnit = commander;
    }

    /// <summary>
    /// Called when the game ends, resets the team to its default state
    /// </summary>
    public void ResetTeam()
    {
        foreach (Unit unit in units)
        {
            Destroy(unit.gameObject);
        }

        if (commander != null)
        {
            Destroy(commander.gameObject);
        }

        units.Clear();

        currency = 1;
        currencyPerTurn = 2;
    }
}
