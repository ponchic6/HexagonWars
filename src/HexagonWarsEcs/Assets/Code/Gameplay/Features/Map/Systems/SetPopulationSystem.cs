using System;
using System.Collections.Generic;
using Entitas;

namespace Code.Gameplay.Features.Map.Systems
{
    public class SetPopulationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;
        private List<GameEntity> _buffer = new(128);

        public SetPopulationSystem()
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.ChildHexagon);

        }
        
        public void Execute()
        {
            var random = new Random();
            
            foreach (GameEntity entity in _entities.GetEntities(_buffer))
            {
                if (!entity.hasCitizensAmount)
                {
                    entity.AddCitizensAmount(random.Next(1, 101));
                }
            }
        }
    }
}