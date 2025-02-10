using Entitas;

namespace Code.Gameplay.Features.Logistics
{
    [Game] public class Food : IComponent { public float Value; }
    [Game] public class CouriersAmount : IComponent { public int Value; }
    [Game] public class MaxSupplyComplexityWay : IComponent { public float Value; }
    [Game] public class CurrentSupplyComplexityWay : IComponent { public float Value; }
    [Game] public class SupplyRoute : IComponent { }
}