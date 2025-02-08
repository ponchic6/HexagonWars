using System.Collections.Generic;
using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Gameplay.Features.Map.View
{
    public class NeighboringHexagons : MonoBehaviour
    {
        [SerializeField] private List<EntityBehaviour> _neighboringHexagons;

        public List<EntityBehaviour> NeighboringHexagonsList => _neighboringHexagons;
    }
}