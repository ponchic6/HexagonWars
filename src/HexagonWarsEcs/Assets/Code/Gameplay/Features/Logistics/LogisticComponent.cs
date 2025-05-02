using System.Collections.Generic;
using Code.Gameplay.Features.Logistics.DataStructure;
using Code.Gameplay.Features.Logistics.View;
using Entitas;

namespace Code.Gameplay.Features.Logistics
{
    [Game] public class FoodAmount : IComponent { public float Value; }
    [Game] public class AmmoAmount : IComponent { public float Value; }
    [Game] public class CouriersProgressList : IComponent { public List<CurrentCourierProgress> Value; }
    [Game] public class SupplyComplexityWay : IComponent { public float Value; }
    [Game] public class SupplyRoute : IComponent { }
    [Game] public class AvailabilityForSupplyRout : IComponent { }
    [Game] public class HighlightedSupplyRout : IComponent { }
    [Game] public class SupplyHighlighter : IComponent { public SupplyRoutHighlighter Value; }
}