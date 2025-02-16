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
                GameMatcher.MaxSupplyComplexityWay,
                GameMatcher.CouriersProgressList ));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.couriersProgressList.Value.Count == 0)
                    continue;

                foreach (CurrentCourierProgress courierProgress in entity.couriersProgressList.Value)
                {
                    if (courierProgress.currentProgress < entity.maxSupplyComplexityWay.Value)
                    {
                        courierProgress.currentProgress += Time.deltaTime;
                    }
                    else
                    {
                        GameEntity finishHex = _game.GetEntityWithId(entity.wayIdPoints.Value.Last());
                        GameEntity startHex = _game.GetEntityWithId(entity.wayIdPoints.Value[0]);

                        if (startHex.foodAmount.Value >= _commonStaticData.CourierFoodCapacity)
                        {
                            finishHex.foodAmount.Value += _commonStaticData.CourierFoodCapacity;
                            startHex.foodAmount.Value -= _commonStaticData.CourierFoodCapacity;
                        }
                        
                        courierProgress.currentProgress = 0;
                    }
                }
            }
        }
    }
}