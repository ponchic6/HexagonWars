using Code.Gameplay.Features.Building.DataStructure;
using Code.Infrastructure.StaticData;
using Entitas;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class DeathBuildersByHungerSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IGroup<GameEntity> _entities;

        public DeathBuildersByHungerSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.BuildingProgress, GameMatcher.CurrentHungerDeathCooldown));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.currentHungerDeathCooldown.Value == 0)
                {
                    foreach (BuildProgressContainer buildProgressContainer in entity.buildingProgress.Value)
                    {
                        buildProgressContainer.buildersAmount = (int)(buildProgressContainer.buildersAmount * _commonStaticData.CoefficientBuildersDeathByHungerInAct);
                    }
                }
            }
        }
    }
}