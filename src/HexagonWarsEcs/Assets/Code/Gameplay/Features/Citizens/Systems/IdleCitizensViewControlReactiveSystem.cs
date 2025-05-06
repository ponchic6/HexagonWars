using System.Collections.Generic;
using Code.Gameplay.Features.Citizens.Services;
using Entitas;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class IdleCitizensViewControlReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly ICitizensModelFactory _citizensModelFactory;

        public IdleCitizensViewControlReactiveSystem(IContext<GameEntity> context, ICitizensModelFactory citizensModelFactory) : base(context)
        {
            _citizensModelFactory = citizensModelFactory;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.CitizensAmount);

        protected override bool Filter(GameEntity entity) => 
            true;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                if (entity.citizensAmount.Value != 0)
                    _citizensModelFactory.TryCreateCitizen(entity.id.Value);
                else
                    _citizensModelFactory.TryRemoveCitizen(entity.id.Value);
            }
        }
    }
}