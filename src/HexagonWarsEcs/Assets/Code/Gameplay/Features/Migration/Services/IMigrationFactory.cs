using Code.Gameplay.Common;
using Code.Infrastructure.View;

namespace Code.Gameplay.Features.Migration.Services
{
    public interface IMigrationFactory
    {
        void SetInitialHex(EntityBehaviour value, int selectedPeople, ManMigrationType warriors);
        void SetFinishHexAndCreateMigration(EntityBehaviour value);
    }
}