using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class CitizensMigrationProceedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public CitizensMigrationProceedSystem()
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.AllOf(
                GameMatcher.MigrationComplexityWay,
                GameMatcher.WayIdPoints,
                GameMatcher.CitizensMigrationAmount));
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
                    
                if (currentHex.citizensAmount.Value < routEntity.citizensMigrationAmount.Value)
                {
                    routEntity.isDestructed = true;
                    return;
                }
                    
                currentHex.ReplaceCitizensAmount(currentHex.citizensAmount.Value - routEntity.citizensMigrationAmount.Value);
                nextHex.ReplaceCitizensAmount(nextHex.citizensAmount.Value + routEntity.citizensMigrationAmount.Value);
                    
                routEntity.migrationComplexityWay.Value.RemoveAt(0);
                routEntity.ReplaceMigrationComplexityWay(routEntity.migrationComplexityWay.Value);
                routEntity.wayIdPoints.Value.RemoveAt(0);
                routEntity.ReplaceWayIdPoints(routEntity.wayIdPoints.Value);
            }
        }
    }
}