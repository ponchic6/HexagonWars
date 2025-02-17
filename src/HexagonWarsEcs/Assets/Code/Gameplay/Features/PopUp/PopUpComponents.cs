using System.Collections.Generic;
using Code.Gameplay.Features.PopUp.DataStructures;
using Entitas;

namespace Code.Gameplay.Features.PopUp
{
    [Game] public class AmountPopUpCooldown : IComponent { public float Cooldown; public float RemainingTime; }
    [Game] public class AmountPopUpEvents : IComponent { public List<AmountPopUpContainer> Value; }
}