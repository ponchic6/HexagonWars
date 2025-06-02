using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Men.Systems
{
    public class ManFeature : Feature
    {
        public ManFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<DeathByHungerSystem>());
            Add(systemFactory.Create<HungerSystem>());
            //Add(systemFactory.Create<ManAnimationIdleOnPlaceSystem>());
            Add(systemFactory.Create<ManAnimationIdleOnPlaceReactiveSystem>());
            //Add(systemFactory.Create<ManAnimationRunningOnPlaceSystem>());
            Add(systemFactory.Create<ManAnimationRunningOnPlaceReactiveSystem>());
            //Add(systemFactory.Create<ManAnimationStartShootingSystem>());
            Add(systemFactory.Create<ManAnimationStartShootingReactiveSystem>());
            //Add(systemFactory.Create<ManAnimationStopShootingSystem>());
            Add(systemFactory.Create<ManAnimationStopShootingReactiveSystem>());
            Add(systemFactory.Create<RunningManViewControlSystem>());

            Add(systemFactory.Create<ManViewReactiveSystem>());
        }
    }
}