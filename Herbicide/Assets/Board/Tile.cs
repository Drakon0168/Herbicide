using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The possible terrain states of a tile
/// </summary>
public enum TileState
{
    Dirt,
    Grass,
    Metal
}

/// <summary>
/// The possible highlight states of a tile
/// </summary>
public enum HighlightState
{
    UnSelected,
    Selected,
    Move,
    Attack,
    Assist
}

public class Tile : MonoBehaviour
{
    private TileState terrain;
    private HighlightState highlightState;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<Sprite> sprites;
    private Unit occupyingUnit;

    /// <summary>
    /// The terrain type of the tile
    /// </summary>
    public TileState Terrain
    {
        get { return terrain; }
        set {
            terrain = value;

            if(spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            spriteRenderer.sprite = sprites[(int)terrain];
        }
    }

    public Unit OccupyingUnit
    {
        get { return occupyingUnit; }
        set { occupyingUnit = value; }
    }

    /// <summary>
    /// The current highlight state of the tile
    /// </summary>
    public HighlightState Highlight
    {
        get { return highlightState; }
        set
        {
            highlightState = value;

            switch (highlightState)
            {
                case HighlightState.UnSelected:
                    spriteRenderer.color = Color.white;
                    break;
                case HighlightState.Selected:
                    spriteRenderer.color = Color.green;
                    break;
                case HighlightState.Move:
                    spriteRenderer.color = Color.yellow;
                    break;
                case HighlightState.Attack:
                    spriteRenderer.color = Color.red;
                    break;
                case HighlightState.Assist:
                    spriteRenderer.color = Color.blue;
                    break;
            }
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
