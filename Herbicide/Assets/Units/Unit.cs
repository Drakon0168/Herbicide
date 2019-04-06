using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Impact,
    Fire,
    Acid
}

public abstract class Unit : MonoBehaviour
{
    [SerializeField]
    protected int maxHealth;
    protected int health;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected DamageType damageType;
    [SerializeField]
    protected float moveRange;
    [HideInInspector]
    public float currentMoveRange;
    [HideInInspector]
    protected Vector2 position;
    protected Dictionary<DamageType, int> damageResistance;
    protected TeamType team;
    [SerializeField]
    protected string unitName;
    [SerializeField]
    protected string displayInfo;
    [SerializeField]
    protected Sprite profileImage;

    /// <summary>
    /// The team that the unit belongs to
    /// </summary>
    public TeamType Team
    {
        get { return team; }
        set { team = value; }
    }

    /// <summary>
    /// The position of the unit
    /// </summary>
    public Vector2 Position
    {
        get { return position; }
        set
        {
            position = value;
            transform.position = new Vector3(position.x + 0.5f, position.y + 0.5f, -0.5f);
        }
    }

    /// <summary>
    /// The name of the unit to be displayed when it is selected
    /// </summary>
    public string UnitName
    {
        get { return unitName; }
    }

    /// <summary>
    /// The info to be displayed when the unit is selected
    /// </summary>
    public string UnitDescription
    {
        get { return displayInfo; }
    }

    /// <summary>
    /// The type of damage that the unit deals
    /// </summary>
    public DamageType DamageType
    {
        get { return damageType; }
    }

    /// <summary>
    /// The unit's current health
    /// </summary>
    public int Health
    {
        get { return health; }
    }

    /// <summary>
    /// The amount of damage the unit deals
    /// </summary>
    public int Damage
    {
        get { return damage; }
    }

    /// <summary>
    /// The units damage resistances, 0 for weak against, 1 for strong against.
    /// </summary>
    public Dictionary<DamageType, int> Resistances
    {
        get { return damageResistance; }
    }

    protected virtual void Start()
    {
        health = maxHealth;
        currentMoveRange = moveRange;
        Board.endTurn += OnTurnEnd;
        damageResistance = new Dictionary<DamageType, int>();
    }

    /// <summary>
    /// Called when the turn ends
    /// </summary>
    public void OnTurnEnd()
    {
        currentMoveRange = moveRange;
    }

    /// <summary>
    /// Called when the unit is selected
    /// </summary>
    public virtual void Select()
    {
        List<Vector2> movePositions = GetMovablePositions();
        
        foreach (Vector2 vector in movePositions)
        {
            if (vector != position)
            {
                Board.SelectTile(vector, team);
            }
        }
    }

    /// <summary>
    /// Returns a list of all of the positions this unit can move to
    /// </summary>
    /// <returns></returns>
    public List<Vector2> GetMovablePositions()
    {
        //TODO: Make unit get positions more efficient and account for walls and occupied spaces
        List<List<Vector2>> selectedPositions = new List<List<Vector2>>();
        selectedPositions.Add(new List<Vector2>());
        selectedPositions.Add(new List<Vector2>());
        
        selectedPositions[0].Add(position);

        for (int i = 0; i < currentMoveRange; i++)
        {
            foreach (Vector2 vector in selectedPositions[0])
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (!(x == 0 && y == 0) && ((x == 0 && y != 0) || (y == 0 && x != 0)))
                        {
                            if (!selectedPositions[0].Contains(vector + new Vector2(x, y)) && !selectedPositions[1].Contains(vector + new Vector2(x, y)))
                            {
                                selectedPositions[1].Add(vector + new Vector2(x, y));
                            }
                        }
                    }
                }
            }

            selectedPositions[0].AddRange(selectedPositions[1]);
            selectedPositions[1].Clear();
        }

        return selectedPositions[0];
    }

    /// <summary>
    /// Called when the units health drops below 0
    /// </summary>
    public virtual void Die()
    {
        //Play death animation
    }

    /// <summary>
    /// Decrements the unit's health based on the damage dealt and the units resistances
    /// </summary>
    /// <param name="damage"></param>
    public void takeDamage(int damage, DamageType damageType)
    {
        if (damageResistance.ContainsKey(damageType))
        {
            switch (damageResistance[damageType])
            {
                case 0:
                    damage *= 2;
                    break;
                case 1:
                    damage = (int)(damage * 0.5f);
                    break;
            }
        }
        
        health -= damage;

        if(health <= 0)
        {
            health = 0;
        }
    }
}
