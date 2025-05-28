using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class WarriorsMigrationProceedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public WarriorsMigrationProceedSystem()
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.AllOf(
                GameMatcher.MigrationComplexityWay,
                GameMatcher.WayIdPoints,
                GameMatcher.WarriorsMigrationAmount));
        }
        
        public void Execute()
        {
            foreach (GameEntity routEntity in _entities)
            {
                if (routEntity.migrationComplexityWay.Value.Count <= 0)
                {
                    routEntity.isDestructed = true;
                    return;
                }
                
                if (routEntity.migrationComplexityWay.Value[0] > 0)
                {
                    routEntity.migrationComplexityWay.Value[0] -= Time.deltaTime;
                    routEntity.ReplaceMigrationComplexityWay(routEntity.migrationComplexityWay.Value);
                    continue;
                }
                
                GameEntity currentHex = _game.GetEntityWithId(routEntity.wayIdPoints.Value[0]);
                GameEntity nextHex = _game.GetEntityWithId(routEntity.wayIdPoints.Value[1]);
                
                if (nextHex.isEnemyHexagon)
                    continue;
                    
                if (currentHex.warriorsAmount.Value < routEntity.warriorsMigrationAmount.Value)
                {
                    routEntity.isDestructed = true;
                    return;
                }
                    
                currentHex.ReplaceWarriorsAmount(currentHex.warriorsAmount.Value - routEntity.warriorsMigrationAmount.Value);
                nextHex.ReplaceWarriorsAmount(nextHex.warriorsAmount.Value + routEntity.warriorsMigrationAmount.Value);
                    
                routEntity.migrationComplexityWay.Value.RemoveAt(0);
                routEntity.ReplaceMigrationComplexityWay(routEntity.migrationComplexityWay.Value);
                routEntity.wayIdPoints.Value.RemoveAt(0);
                routEntity.ReplaceWayIdPoints(routEntity.wayIdPoints.Value);
            }
        }

    }
}