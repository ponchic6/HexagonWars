using Entitas;

namespace Code.Gameplay.Features.Production
{
    [Game] public class City : IComponent { public int CitizenOrdered; public float CurrentCooldown; public float Cooldown; }
    [Game] public class FoodFarm : IComponent { public int Workers; }
    [Game] public class Mine : IComponent { public int Miners; public OreType OreType; };
    [Game] public class Barracks : IComponent { public int WarriorsOrdered; public float CurrentCooldown; public float Cooldown; }
}