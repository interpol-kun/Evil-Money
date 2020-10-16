using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/PlayerInfo")]
public class PlayerInformation : ScriptableObject{

    [SerializeField]
    private int overalMoney;

    public Tier tier;

    [SerializeField]
    private List<int> levelMoney;

    [SerializeField]
    private List<int> paperIndexes;

    [System.Flags]
    public enum Tier { None = 0, First = 1, Second = 2, Third = 3, DoorTier = 4 }

    public static Tier SetFlag(Tier a, Tier b)
    {
		return a | b;
    }

    public static Tier UnsetFlag(Tier a, Tier b)
    {
        return a & (~b);
    }

    public static bool HasFlag(Tier a, Tier b)
    {
        return (a & b) == b;
    }

    public static Tier ToogleFlag(Tier a, Tier b)
    {
        return a ^ b; //using XOR
    }

    public int OveralMoney
    {
        get
        {
            return overalMoney;
        }

        set
        {
            LevelMoney.Add(value - overalMoney);
            overalMoney = value;
        }
    }

    public List<int> LevelMoney
    {
        get
        {
            return levelMoney;
        }

        set
        {
            levelMoney = value;
        }
    }

    public List<int> PaperIndexes
    {
        get
        {
            return paperIndexes;
        }

        set
        {
            paperIndexes = value;
        }
    }

    private void ClearPlayerInfo()
    {
        LevelMoney = new List<int>();
        OveralMoney = 0;
        tier = new PlayerInformation.Tier();
        PaperIndexes = new List<int>();
    }

    public void Clear()
    {
        ClearPlayerInfo();
    }
}
