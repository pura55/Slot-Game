using UnityEngine;

public class SlotRules
{
    //å©ÇÈ3ÉâÉCÉì
    public static readonly int[][] Lines =
    {
        new int[] { 1, 1, 1 },                          // íÜíi
        new int[] { 0, 1, 2 },                          // Ç»Ç»Çﬂâ∫Ç™ÇË
        new int[] { 2, 1, 0 }                           // Ç»Ç»Çﬂè„Ç™ÇË
    };

    public static readonly Symbol HealSymbol = Symbol.Cherry;  // HEAL(ç∂ÉäÅ[Éã)ÇÃê}ïø

    public static bool TryGetTarget(Role role, out Symbol target)
    {
        switch (role)
        {
            case Role.COIN:
                target = Symbol.Grape;
                return true;
            case Role.BIG_COIN:
                target = Symbol.Clown;
                return true;
            case Role.LIGHT_ATTACK:
                target = Symbol.Bell;
                return true;
            case Role.REPLAY:
                target = Symbol.Animal;
                return true;
        }

        target = Symbol.Bar;
        return false;
    }

    public static bool IsWinningTriple(Symbol a, Symbol b, Symbol c)
    {
        if (a == b && b == c)
        {
            return true;
        }

        return a == Symbol.Seven && b == Symbol.Seven && c == Symbol.Bar;
    }

    public static bool MatchBonus(Role bonus, Symbol a, Symbol b, Symbol c)
    {
        switch (bonus)
        {
            case Role.BIG_BONUS:
                return a == Symbol.Seven && b == Symbol.Seven && c == Symbol.Seven;
            case Role.BONUS:
                return a == Symbol.Seven && b == Symbol.Seven && c == Symbol.Bar;
        }
        return false;
    }

    public static bool TryGetBonusPattern(Role bonus, out Symbol[] pattern)
    {
        switch (bonus)
        {
            case Role.BIG_BONUS:
                pattern = new Symbol[] { Symbol.Seven, Symbol.Seven, Symbol.Seven };
                return true;
            case Role.BONUS:
                pattern = new Symbol[] { Symbol.Seven, Symbol.Seven, Symbol.Bar };
                return true;
        }

        pattern = null;
        return false;
    }

    public static bool IsBarBarBar(Symbol a, Symbol b, Symbol c)
    {
        return a == Symbol.Bar && b == Symbol.Bar && c == Symbol.Bar;
    }

    public static PlayerCommandType ToCommand(Role role)
    {
        switch (role)
        {
            case Role.LIGHT_ATTACK: return PlayerCommandType.LIGHT_ATTACK;
            case Role.HEAVY_ATTACK: return PlayerCommandType.HEAVY_ATTACK;
            case Role.REPLAY: return PlayerCommandType.ADDITIONAL_TURN;
            case Role.HEAL: return PlayerCommandType.HEAL;
            case Role.COIN:
            case Role.BIG_COIN: return PlayerCommandType.COIN;
            case Role.BONUS:
            case Role.BIG_BONUS: return PlayerCommandType.BONUS;
        }
        return PlayerCommandType.NONE;
    }

    public static int Mod(int a, int m)
    {
        return ((a % m) + m) % m;
    }
}
