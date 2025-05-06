using Code.Gameplay.Features.Citizens.Systems;
using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationFeature : Feature
    {
        public MigrationFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<CitizensMigrationProceedSystem>());
            Add(systemFactory.Create<WarriorsMigrationProceedSystem>());
            Add(systemFactory.Create<MigrationChooserUiReactiveSystem>());
            Add(systemFactory.Create<MigrationHexButtonsReactiveSystem>());
            Add(systemFactory.Create<MigrationEnemyHexButtonsReactiveSystem>());
            Add(systemFactory.Create<IdleCitizensViewControlReactiveSystem>());
            Add(systemFactory.Create<RunningCitizensViewControlReactiveSystem>());
        }
    }
}