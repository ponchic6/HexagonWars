using System.Collections.Generic;
using Code.Gameplay.Features.Building.DataStructure;
using Entitas;

namespace Code.Gameplay.Features.Building
{
    [Game] public class BuildingProgress : IComponent { public List<BuildProgressContainer> Value; }
}