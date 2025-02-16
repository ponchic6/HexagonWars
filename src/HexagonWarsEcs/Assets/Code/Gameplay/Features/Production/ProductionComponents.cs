using Entitas;

namespace Code.Gameplay.Features.Production
{
    [Game] public class LivingArea : IComponent { }
    [Game] public class FoodFarm : IComponent { public int Workers; }
    [Game] public class Barracks : IComponent { public int WarriorsOrdered; public float CurrentCooldown; public float Cooldown; }
}