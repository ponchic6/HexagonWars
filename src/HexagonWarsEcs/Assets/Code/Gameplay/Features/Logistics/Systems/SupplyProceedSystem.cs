using System.Linq;
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
                GameMatcher.CurrentSupplyComplexityWay,
                GameMatcher.WayIdPoints,
                GameMatcher.MaxSupplyComplexityWay,
                GameMatcher.CouriersAmount));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.couriersAmount.Value == 0)
                    continue;
                
                if (entity.currentSupplyComplexityWay.Value < entity.maxSupplyComplexityWay.Value)
                {
                    entity.ReplaceCurrentSupplyComplexityWay(entity.currentSupplyComplexityWay.Value + Time.deltaTime);
                }
                else
                {
                    _game.GetEntityWithId(entity.wayIdPoints.Value.Last()).food.Value += entity.couriersAmount.Value;
                    _game.GetEntityWithId(entity.wayIdPoints.Value[0]).food.Value -= entity.couriersAmount.Value;
                    entity.ReplaceCurrentSupplyComplexityWay(0);
                    return;
                }
            }
        }
    }
}