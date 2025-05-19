using System.Collections.Generic;
using Code.Gameplay.Features.Migration.View;
using Entitas;

namespace Code.Gameplay.Features.Migration
{
    [Game] public class MigrationComplexityWay : IComponent { public List<float> Value; }
    [Game] public class CitizensMigrationAmount : IComponent { public int Value; }
    [Game] public class WayIdPoints : IComponent { public List<int> Value; }
    [Game] public class MigrationArrow : IComponent { public int MigrationId; }
    [Game] public class MigrationHandlerComponent : IComponent { public MigrationStartHexHandler Value; }
}