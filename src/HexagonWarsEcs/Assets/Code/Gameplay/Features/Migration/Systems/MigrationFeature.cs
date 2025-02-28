using Code.Gameplay.Features.Citizens.Systems;
using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationFeature : Feature
    {
        public MigrationFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<CitizensMigrationProceedSystem>());
            Add(systemFactory.Create<CitizensViewControlSystem>());
        }
    }
}