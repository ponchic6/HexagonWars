using Code.Gameplay.Features.Building.DataStructure;
using Entitas;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class DeathBuildersByHungerSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public DeathBuildersByHungerSystem()
        {
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.BuildingProgress, GameMatcher.CurrentHungerDeathCooldown));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.foodAmount.Value == 0 && entity.currentHungerDeathCooldown.Value >= entity.maxHungerDeathCooldown.Value)
                {
                    foreach (BuildProgressContainer buildProgressContainer in entity.buildingProgress.Value)
                    {
                        buildProgressContainer.buildersAmount = (int)(buildProgressContainer.buildersAmount * 0.9f);
                    }
                }
            }
        }
    }
}