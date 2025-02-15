using System.Linq;
using Code.Gameplay.Features.Logistics.DataStructure;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Logistics.Systems
{
    public class SupplyProceedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private GameContext _game;

        public SupplyProceedSystem()
        {
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
                        _game.GetEntityWithId(entity.wayIdPoints.Value.Last()).foodAmount.Value += 4;
                        _game.GetEntityWithId(entity.wayIdPoints.Value[0]).foodAmount.Value -= 4;
                        courierProgress.currentProgress = 0;
                    }
                }
            }
        }
    }
}