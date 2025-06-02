using Code.Gameplay.Features.Building.DataStructure;
using Code.Infrastructure.StaticData;
using Entitas;

namespace Code.Gameplay.Features.Men.Systems
{
    public class DeathByHungerSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IGroup<GameEntity> _entities;
        private GameContext _game;

        public DeathByHungerSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            _game = Contexts.sharedInstance.game;
            
            _entities = _game.GetGroup(GameMatcher.CurrentHungerDeathCooldown);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.currentHungerDeathCooldown.Value != 0) 
                    continue;
                
                if (entity.hasManAmount) 
                    entity.ReplaceManAmount((int)(entity.manAmount.Value * _commonStaticData.CoefficientCitizensDeathByHungerInAct));
                
                if (entity.hasBuildingProgress)
                {
                    foreach (BuildProgressContainer buildProgressContainer in entity.buildingProgress.Value)
                        buildProgressContainer.buildersAmount = (int)(buildProgressContainer.buildersAmount * _commonStaticData.CoefficientBuildersDeathByHungerInAct);
                }
            }
        }
    }
}