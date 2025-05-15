using Code.Infrastructure.View;

namespace Code.Gameplay.Features.Battle.Services
{
    public interface IBattleFieldFactory
    {
        public void SetAttackers(EntityBehaviour entityBehaviour, int selectedWarriors);
        public void TrySetDefendersAndCreateBattlefield(EntityBehaviour entityBehaviour);
        public void CreateBattlefieldFromWarriorsMigration(GameEntity entity);
    }
}