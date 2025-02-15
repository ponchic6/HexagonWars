using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class CitizensMigrationProceedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private List<GameEntity> _buffer = new(64);
        private GameContext _game;

        public CitizensMigrationProceedSystem()
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.AllOf(
                GameMatcher.ComplexityWay,
                GameMatcher.WayIdPoints,
                GameMatcher.CitizensMigrationAmount));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities.GetEntities(_buffer))
            {
                if (entity.complexityWay.Value.Count > 0)
                {
                    entity.complexityWay.Value[0] -= Time.deltaTime;
                    entity.ReplaceComplexityWay(entity.complexityWay.Value);
                }
                else
                {
                    entity.isDestructed = true;
                    return;
                }
                
                if (entity.complexityWay.Value[0] <= 0)
                {
                    if (_game.GetEntityWithId(entity.wayIdPoints.Value[0]).citizensAmount.Value < entity.citizensMigrationAmount.Value)
                    {
                        entity.isDestructed = true;
                        return;
                    }
                    _game.GetEntityWithId(entity.wayIdPoints.Value[0]).citizensAmount.Value -= entity.citizensMigrationAmount.Value;
                    _game.GetEntityWithId(entity.wayIdPoints.Value[1]).citizensAmount.Value += entity.citizensMigrationAmount.Value;
                    entity.complexityWay.Value.RemoveAt(0);
                    entity.ReplaceComplexityWay(entity.complexityWay.Value);
                    entity.wayIdPoints.Value.RemoveAt(0);
                    entity.ReplaceWayIdPoints(entity.wayIdPoints.Value);
                }
            }
        }
    }
}