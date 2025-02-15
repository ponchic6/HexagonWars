using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class WarriorsMigrationProceedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;
        private List<GameEntity> _buffer = new(64);

        public WarriorsMigrationProceedSystem()
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.AllOf(
                GameMatcher.ComplexityWay,
                GameMatcher.WayIdPoints,
                GameMatcher.WarriorsMigrationAmount));
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
                    if (_game.GetEntityWithId(entity.wayIdPoints.Value[0]).warriorsAmount.Value < entity.warriorsMigrationAmount.Value)
                    {
                        entity.isDestructed = true;
                        return;
                    }
                    
                    _game.GetEntityWithId(entity.wayIdPoints.Value[0]).warriorsAmount.Value -= entity.warriorsMigrationAmount.Value;
                    _game.GetEntityWithId(entity.wayIdPoints.Value[1]).warriorsAmount.Value += entity.warriorsMigrationAmount.Value;
                    entity.complexityWay.Value.RemoveAt(0);
                    entity.ReplaceComplexityWay(entity.complexityWay.Value);
                    entity.wayIdPoints.Value.RemoveAt(0);
                    entity.ReplaceWayIdPoints(entity.wayIdPoints.Value);
                }
            }
        }

    }
}