using Code.Gameplay.Features.Battle.View.UI;
using Entitas;

namespace Code.Gameplay.Features.Battle
{
    [Game] public class EnemyHexagon : IComponent { }
    [Game] public class PlayerHexagon : IComponent { }
    [Game] public class BattleIndicator : IComponent { public int FromHexId; public int ToHexId; public float WinIndicator; public int BattleId; }
    [Game] public class BattleIndicatorControllerComponent : IComponent { public BattleIndicatorController Controller;}
    [Game] public class CurrentBattleCooldown : IComponent { public float Value ;}
    [Game] public class BattleCooldown : IComponent { public float Value ;}
    [Game] public class BattleArrow : IComponent { public int BattlefieldId; }
    [Game] public class Battlefield : IComponent { public int AttackerHexagonId; public int DefenderHexagonId; }
}