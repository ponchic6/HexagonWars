using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationFeature : Feature
    {
        public MigrationFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<MigrationArrowsPositionSystem>());
            Add(systemFactory.Create<MigrationProceedSystem>());
        }
    }
}