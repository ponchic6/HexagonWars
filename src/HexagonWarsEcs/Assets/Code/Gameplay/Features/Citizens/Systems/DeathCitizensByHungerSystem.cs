using Code.Infrastructure.StaticData;
using Entitas;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class DeathCitizensByHungerSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IGroup<GameEntity> _entities;

        public DeathCitizensByHungerSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.CitizensAmount, GameMatcher.CurrentHungerDeathCooldown));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.currentHungerDeathCooldown.Value == 0) 
                    entity.ReplaceCitizensAmount((int)(entity.citizensAmount.Value * _commonStaticData.CoefficientCitizensDeathByHungerInAct));
            }
        }
    }
}