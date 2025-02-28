using Code.Gameplay.Features.Citizens.Services;
using Entitas;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class CitizensViewControlSystem : IExecuteSystem
    {
        private readonly ICitizensModelFactory _citizensModelFactory;
        private readonly IGroup<GameEntity> _entities;

        public CitizensViewControlSystem(ICitizensModelFactory citizensModelFactory)
        {
            _citizensModelFactory = citizensModelFactory;
            
            GameContext game = Contexts.sharedInstance.game;
            _entities = game.GetGroup(GameMatcher.CitizensAmount);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.citizensAmount.Value != 0)
                    _citizensModelFactory.TryCreateIdleCitizen(entity.id.Value);
                else
                    _citizensModelFactory.TryRemoveIdleCitizen(entity.id.Value);
            }
        }
    }
}