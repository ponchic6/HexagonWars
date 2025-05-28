using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationFeature : Feature
    {
        public MigrationFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<CitizensMigrationProceedSystem>());
            Add(systemFactory.Create<WarriorsMigrationProceedSystem>());
            Add(systemFactory.Create<CitizensRunningAnimationOnPlaceSystem>());
            Add(systemFactory.Create<WarriorsRunningAnimationOnPlaceSystem>());
            Add(systemFactory.Create<MigrationTrailClearSystem>());
            
            Add(systemFactory.Create<MigrationChooserUiReactiveSystem>());
            Add(systemFactory.Create<MigrationButtonsVisibilityReactiveSystem>());
            Add(systemFactory.Create<MigrationEnemyButtonsVisibilityReactiveSystem>());
            Add(systemFactory.Create<MigrationTogglesDisablingReactiveSystem>());
            Add(systemFactory.Create<MigrationTogglesCountPeopleReactiveSystem>());
            Add(systemFactory.Create<MigrationTrailCreatingReactiveSystem>());
            Add(systemFactory.Create<MigrationTogglesForChooserReactiveSystem>());
            Add(systemFactory.Create<MigrationChooserHideReactiveSystem>());
        }
    }
}