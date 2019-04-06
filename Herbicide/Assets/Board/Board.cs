using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void EndTurn();
public delegate void ResetGame();

public enum SelectionState
{
    Unit,
    Action
}

public class Board : MonoBehaviour
{
    [SerializeField]
    private Vector2 boardSize;
    [SerializeField]
    private Vector2 plantStart;
    [SerializeField]
    private Vector2 robotStart;
    private static Dictionary<Vector2, Tile> tiles;
    [SerializeField]
    private GameObject TilePrefab;
    [SerializeField]
    private List<Team> teams;
    private int turnStage;
    private int turn;
    public static EndTurn endTurn;
    public static ResetGame resetGame;
    private SelectionState selectionState;
    private Unit selectedUnit;

    [Space]
    [Header("UI Elements")]
    [SerializeField]
    private Text TurnDisplay;
    [SerializeField]
    private Text currencyDisplay;

    /// <summary>
    /// A list of all of the tiles on the screen
    /// </summary>
    public static Dictionary<Vector2, Tile> Tiles
    {
        get { return tiles; }
    }

    /// <summary>
    /// All of the teams in the map
    /// </summary>
    public List<Team> Teams
    {
        get { return teams; }
    }

    /// <summary>
    /// The current turn
    /// </summary>
    public int Turn
    {
        get { return turn; }
    }

    private void Start()
    {
        tiles = new Dictionary<Vector2, Tile>();

        resetGame += ResetBoard;
        resetGame();

        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EndOfTurn();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector2(Mathf.FloorToInt(mousePosition.x ), Mathf.FloorToInt(mousePosition.y));

            SelectTile(mousePosition);
        }
    }

    /// <summary>
    /// Selects the tile at the given position
    /// </summary>
    public  void SelectTile(Vector2 position)
    {
        if (tiles.ContainsKey(position))
        {
            switch(selectionState)
            {
                case SelectionState.Unit:
                    DeselectTiles();
                    tiles[position].Highlight = HighlightState.Selected;

                    if (tiles[position].OccupyingUnit != null && tiles[position].OccupyingUnit.Team == teams[turnStage].teamType)
                    {
                        selectedUnit = tiles[position].OccupyingUnit;
                        selectedUnit.Select();
                        selectionState = SelectionState.Action;
                    }
                    break;
                case SelectionState.Action:
                    switch (tiles[position].Highlight)
                    {
                        case HighlightState.Selected:
                        case HighlightState.UnSelected:
                            selectedUnit = null;
                            DeselectTiles();
                            selectionState = SelectionState.Unit;
                            break;
                        case HighlightState.Move:
                            Vector2 distance = new Vector2(Mathf.Abs(position.x - selectedUnit.Position.x), Mathf.Abs(position.y - selectedUnit.Position.y));
                            if (distance.x < distance.y)
                            {
                                selectedUnit.currentMoveRange -= distance.x * 2 + (distance.y - distance.x);
                            }
                            else
                            {
                                selectedUnit.currentMoveRange -= distance.y * 2 + (distance.x - distance.y);
                            }

                            tiles[selectedUnit.Position].OccupyingUnit = null;
                            selectedUnit.Position = position;
                            tiles[selectedUnit.Position].OccupyingUnit = selectedUnit;
                            DeselectTiles();
                            selectedUnit.Select();
                            tiles[selectedUnit.Position].Highlight = HighlightState.Selected;
                            break;
                    }
                    break;
            };
        }
    }

    /// <summary>
    /// Selects the tile at the given position, using different selection types based on team
    /// </summary>
    public static void SelectTile(Vector2 position, TeamType team)
    {
        if (tiles.ContainsKey(position))
        {
            if (tiles[position].OccupyingUnit != null)
            {
                if(tiles[position].OccupyingUnit.Team == team)
                {
                    tiles[position].Highlight = HighlightState.Assist;
                }
                else
                {
                    tiles[position].Highlight = HighlightState.Attack;
                }
            }
            else
            {
                tiles[position].Highlight = HighlightState.Move;
            }
        }
    }

    /// <summary>
    /// Deselects all selected tiles
    /// </summary>
    public void DeselectTiles()
    {
        foreach(KeyValuePair<Vector2, Tile> pair in tiles)
        {
            pair.Value.Highlight = HighlightState.UnSelected;
        }
    }

    /// <summary>
    /// Ends the current turn
    /// </summary>
    public void EndOfTurn()
    {
        DeselectTiles();
        selectionState = SelectionState.Unit;

        turnStage++;

        if(turnStage >= teams.Count)
        {
            if (endTurn != null)
            {
                endTurn();
            }

            turnStage = 0;
            turn++;
        }

        UpdateUI();
    }

    /// <summary>
    /// Updates the Turn and Currency UI to show the correct information
    /// </summary>
    public void UpdateUI()
    {
        TurnDisplay.text = "Turn: " + turn + "\n";

        if (teams[turnStage].teamType == TeamType.Plants)
        {
            TurnDisplay.text += "Plants";
        }
        else
        {
            TurnDisplay.text += "Robots";
        }

        currencyDisplay.text = "Current Currency: " + teams[turnStage].Currency + "\nCurrency Per Turn: " + teams[turnStage].CurrencyPerTurn;
    }
    
    /// <summary>
    /// Resets the board to it's default state
    /// </summary>
    public void ResetBoard()
    {
        //Set up the tile ditionary
        for (int x = 0; x < boardSize.x; x++)
        {
            for (int y = 0; y < boardSize.y; y++)
            {
                Tile newTile = Instantiate(TilePrefab, new Vector2(x + 0.5f, y + 0.5f), Quaternion.Euler(0, 0, 0), transform).GetComponent<Tile>();
                newTile.Terrain = TileState.Dirt;
                tiles.Add(new Vector2(x, y), newTile);
            }
        }

        //Set up the player starts
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (tiles.ContainsKey(plantStart + new Vector2(x, y)))
                {
                    tiles[plantStart + new Vector2(x, y)].Terrain = TileState.Grass;
                }

                if (tiles.ContainsKey(robotStart + new Vector2(x, y)))
                {
                    tiles[robotStart + new Vector2(x, y)].Terrain = TileState.Metal;
                }
            }
        }

        teams[0].SetStart(plantStart);
        teams[1].SetStart(robotStart);

        selectionState = SelectionState.Unit;
    }
}
