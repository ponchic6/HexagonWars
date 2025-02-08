using Code.Infrastructure.View;

namespace Code.Gameplay.Features.Migration.Services
{
    public interface IMigrationFactory
    {
        void SetInitialHex(EntityBehaviour value, int selectedPeople);
        void SetFinishHexAndCreateMigration(EntityBehaviour value);
    }
}