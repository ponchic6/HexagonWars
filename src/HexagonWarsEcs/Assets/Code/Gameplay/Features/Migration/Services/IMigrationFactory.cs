using System.Collections.Generic;
using Code.Gameplay.Common;
using Code.Infrastructure.View;

namespace Code.Gameplay.Features.Migration.Services
{
    public interface IMigrationFactory
    {
        public void SetInitialHex(EntityBehaviour value, int selectedPeople, ManMigrationType warriors);
        public GameEntity SetFinishHexAndCreateMigration(EntityBehaviour value);
        public EntityBehaviour GetAwailableNeighbourHex(EntityBehaviour defendersHex);
        public List<GameEntity> CreateMigrationViewTrail(EntityBehaviour entityBehaviour, ManMigrationType manMigrationType);
    }
}