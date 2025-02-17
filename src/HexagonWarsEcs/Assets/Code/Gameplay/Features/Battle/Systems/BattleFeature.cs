using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattleFeature : Feature
    {
        public BattleFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<BattleArrowsPositionSystem>());
            Add(systemFactory.Create<WarriorsMigrationProceedSystem>());
            Add(systemFactory.Create<BattleCooldownSystem>());
            Add(systemFactory.Create<BattleLoopSystem>());
            Add(systemFactory.Create<BattleStopSystem>());
            Add(systemFactory.Create<BattleResultSystem>());
            Add(systemFactory.Create<WarriorLossesPopUpSystem>());
            Add(systemFactory.Create<BattleArrowsCleanupSystem>());
            Add(systemFactory.Create<PaintingInFractionColorSystem>());
        }
    }
}