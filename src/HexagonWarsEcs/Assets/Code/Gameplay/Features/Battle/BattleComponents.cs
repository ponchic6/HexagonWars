using System.Collections.Generic;
using Code.Gameplay.Features.Battle.DataStructures;
using Entitas;

namespace Code.Gameplay.Features.Battle
{
    [Game] public class EnemyHexagon : IComponent { }
    [Game] public class PlayerHexagon : IComponent { }
    [Game] public class WarriorsAmount : IComponent { public int Value ;}
    [Game] public class WarriorsMigrationAmount : IComponent { public int Value ;}
    [Game] public class CurrentBattleCooldown : IComponent { public float Value ;}
    [Game] public class BattleCooldown : IComponent { public float Value ;}
    [Game] public class BattleArrow : IComponent { public int BattlefieldId; }
    [Game] public class Battlefield : IComponent { public List<WarriorsContainer> AttackerHexagonContainers; public WarriorsContainer DefenderHexagonContainer;}
    
}