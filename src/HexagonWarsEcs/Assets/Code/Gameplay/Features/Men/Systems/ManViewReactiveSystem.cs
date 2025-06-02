using System.Collections.Generic;
using Code.Gameplay.Features.Citizens.Services;
using Entitas;

namespace Code.Gameplay.Features.Men.Systems
{
    public class ManViewReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly IManModelFactory _manModelFactory;

        public ManViewReactiveSystem(IContext<GameEntity> context, IManModelFactory manModelFactory) : base(context)
        {
            _manModelFactory = manModelFactory;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.ManAmount);

        protected override bool Filter(GameEntity entity) => 
            true;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                if (entity.manAmount.Value != 0)
                    _manModelFactory.TryCreateCitizen(entity.id.Value);
                else
                    _manModelFactory.TryRemoveCitizen(entity.id.Value);
            }
        }
    }
}