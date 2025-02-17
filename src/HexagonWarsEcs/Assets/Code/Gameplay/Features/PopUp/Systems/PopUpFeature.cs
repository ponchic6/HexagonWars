using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.PopUp.Systems
{
    public class PopUpFeature : Feature
    {
        public PopUpFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<ResourcesAmountPopUpCooldownSystem>());
            Add(systemFactory.Create<ResourcesAmountPopUpSystem>());
        }
    }
}