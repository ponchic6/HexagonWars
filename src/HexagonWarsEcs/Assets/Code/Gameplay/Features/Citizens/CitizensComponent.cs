using Entitas;

namespace Code.Gameplay.Features.Citizens
{
    [Game] public class CitizensAmount : IComponent { public int Value; }
    [Game] public class CurrentHungerDeathCooldown : IComponent { public float Value; }
    [Game] public class MaxHungerDeathCooldown : IComponent { public float Value; }
}