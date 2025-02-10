using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Migration
{
    [Game] public class ComplexityWay : IComponent { public List<float> Value; }
    [Game] public class MigrationAmount : IComponent { public int Value; }
    [Game] public class WayIdPoints : IComponent { public List<int> Value; }
    [Game] public class MigrationArrow : IComponent { }
    [Game] public class StartMigrationPoint : IComponent { }
    [Game] public class FinishMigrationPoint : IComponent { }
}