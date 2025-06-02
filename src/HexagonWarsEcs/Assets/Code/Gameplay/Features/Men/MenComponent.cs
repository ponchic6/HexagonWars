using Entitas;

namespace Code.Gameplay.Features.Men
{
    [Game] public class ManAmount : IComponent { public int Value; }
    [Game] public class ManAnimation : IComponent { public ManAnimationType Value; }
    [Game] public class CurrentHungerDeathCooldown : IComponent { public float Value; }
    [Game] public class MaxHungerDeathCooldown : IComponent { public float Value; }
}