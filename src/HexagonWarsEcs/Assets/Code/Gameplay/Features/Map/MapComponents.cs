using System.Collections.Generic;
using Code.Infrastructure.View;
using Entitas;

namespace Code.Gameplay.Features.Map
{
    [Game] public class MapParent : IComponent { public List<EntityBehaviour> Value; }
    [Game] public class ChildHexagon : IComponent { }
}