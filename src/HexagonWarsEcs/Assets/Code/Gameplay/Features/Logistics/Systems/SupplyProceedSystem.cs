using System.Linq;
using Code.Gameplay.Features.Logistics.DataStructure;
using Code.Infrastructure.StaticData;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Logistics.Systems
{
    public class SupplyProceedSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public SupplyProceedSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            _game = Contexts.sharedInstance.game;
            
            _entities = _game.GetGroup(GameMatcher.AllOf(
                GameMatcher.WayIdPoints,
                GameMatcher.SupplyComplexityWay,
                GameMatcher.CouriersProgressList));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.couriersProgressList.Value.Count == 0)
                    continue;

                foreach (CurrentCourierProgress courierProgress in entity.couriersProgressList.Value)
                {
                    if (courierProgress.currentProgress < entity.supplyComplexityWay.Value)
                        courierProgress.currentProgress += Time.deltaTime;
                    else
                        DeliverResource(entity, courierProgress);
                }
            }
        }

        private void DeliverResource(GameEntity entity, CurrentCourierProgress courierProgress)
        {
            GameEntity finishHex = _game.GetEntityWithId(entity.wayIdPoints.Value.Last());
            GameEntity startHex = _game.GetEntityWithId(entity.wayIdPoints.Value[0]);

            float capacity;
            
            switch (courierProgress.logisticResources)
            {
                case LogisticResources.Food:
                    capacity = _commonStaticData.CourierCapacity.First(x => x.logisticResources == LogisticResources.Food).capacity;
                    
                    if (startHex.foodAmount.Value >= capacity)
                    {
                        finishHex.foodAmount.Value += capacity;
                        startHex.foodAmount.Value -= capacity;
                    }
                    break;
                
                case LogisticResources.Ammo:
                    capacity = _commonStaticData.CourierCapacity.First(x => x.logisticResources == LogisticResources.Ammo).capacity;
                    
                    if (startHex.ammoAmount.Value >= capacity)
                    {
                        finishHex.ammoAmount.Value += capacity;
                        startHex.ammoAmount.Value -= capacity;
                    }
                    break;
            }
            
            courierProgress.currentProgress = 0;
        }
    }
}