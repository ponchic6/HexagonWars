using Entitas;

namespace Code.Gameplay.Features.Production
{
    [Game] public class LivingArea : IComponent { }
    [Game] public class FoodFarm : IComponent { public int Workers; }
}