using Code.Gameplay.Common;
using Code.Infrastructure.View;

namespace Code.Gameplay.Features.Migration.Services
{
    public interface IMigrationFactory
    {
        void SetInitialHex(EntityBehaviour value, int selectedPeople, ManMigrationType warriors);
        GameEntity SetFinishHexAndCreateMigration(EntityBehaviour value);
        EntityBehaviour GetAwailableNeighbourHex(EntityBehaviour defendersHex);
    }
}