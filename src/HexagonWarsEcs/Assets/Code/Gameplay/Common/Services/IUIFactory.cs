using Code.Gameplay.Features.Building.View;
using Code.Gameplay.Features.Logistics.View.UI;
using Code.Gameplay.Features.Migration.View.UI;
using Code.Gameplay.Features.Production.View.UI;
using Code.Infrastructure.View;

namespace Code.Gameplay.Common.Services
{
    public interface IUIFactory
    {
        public void ShowInfoPanel(EntityBehaviour entityBehaviour);
        public void HideInfoPanel(EntityBehaviour entityBehaviour);
        public void SliderMigrationChooserActivate(EntityBehaviour entityBehaviour, ManMigrationType manMigrationType);
        public void SliderMigrationChooserDeactivate();
        public BuildingInfoPanel BuildingInfoPanel { get; }
        public ProductionInfoPanel ProductionInfoPanel { get; }
        public SupplyRoutsInfoPanel SupplyRoutsInfoPanel { get; }
        public MigrationAmountChooser MigrationAmountChooser { get; }
    }
}