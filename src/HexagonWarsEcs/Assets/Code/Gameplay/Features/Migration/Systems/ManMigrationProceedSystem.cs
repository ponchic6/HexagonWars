using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class ManMigrationProceedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public ManMigrationProceedSystem()
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.AllOf(
                GameMatcher.MigrationComplexityWay,
                GameMatcher.WayIdPoints,
                GameMatcher.ManMigrationAmount));
        }
        
        public void Execute()
        {
            foreach (GameEntity routEntity in _entities)
            {
                if (routEntity.migrationComplexityWay.Value.Count <= 0)
                {
                    routEntity.isDestructed = true;
                    continue;
                }
                
                if (routEntity.migrationComplexityWay.Value[0] > 0)
                {
                    routEntity.migrationComplexityWay.Value[0] -= Time.deltaTime;
                    routEntity.ReplaceMigrationComplexityWay(routEntity.migrationComplexityWay.Value);
                    continue;
                }

                GameEntity currentHex = _game.GetEntityWithId(routEntity.wayIdPoints.Value[0]);
                GameEntity nextHex = _game.GetEntityWithId(routEntity.wayIdPoints.Value[1]);

                if (currentHex.manAmount.Value <= 0)
                {
                    routEntity.isDestructed = true;
                    continue;
                }

                if (nextHex.isEnemyHexagon)
                    continue;
                
                if (currentHex.manAmount.Value < routEntity.manMigrationAmount.Value)
                {
                    nextHex.ReplaceManAmount(nextHex.manAmount.Value + currentHex.manAmount.Value);
                    currentHex.ReplaceManAmount(0);
                }
                else
                {
                    currentHex.ReplaceManAmount(currentHex.manAmount.Value - routEntity.manMigrationAmount.Value);
                    nextHex.ReplaceManAmount(nextHex.manAmount.Value + routEntity.manMigrationAmount.Value);
                }
                
                routEntity.migrationComplexityWay.Value.RemoveAt(0);
                routEntity.ReplaceMigrationComplexityWay(routEntity.migrationComplexityWay.Value);
                routEntity.wayIdPoints.Value.RemoveAt(0);
                routEntity.ReplaceWayIdPoints(routEntity.wayIdPoints.Value);
            }
        }

    }
}