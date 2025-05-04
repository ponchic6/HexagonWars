using Code.Gameplay.Features.Building.DataStructure;
using UnityEngine;

namespace Code.Gameplay.Features.Production.View.UI
{
    public abstract class ProductionHandler : MonoBehaviour
    {
        public abstract void Setup(GameEntity entity);
        public abstract void UpdateProductionUi();
    }
}