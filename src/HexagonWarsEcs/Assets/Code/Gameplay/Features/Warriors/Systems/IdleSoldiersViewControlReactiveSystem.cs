using System.Collections.Generic;
using Code.Gameplay.Features.Warriors.Services;
using Entitas;

namespace Code.Gameplay.Features.Warriors.Systems
{
    public class IdleSoldiersViewControlReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly ISoldiersModelFactory _soldiersModelFactory;

        public IdleSoldiersViewControlReactiveSystem(IContext<GameEntity> context, ISoldiersModelFactory soldiersModelFactory) : base(context)
        {
            _soldiersModelFactory = soldiersModelFactory;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.WarriorsAmount);

        protected override bool Filter(GameEntity entity) => 
            true;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                if (entity.warriorsAmount.Value != 0)
                    _soldiersModelFactory.TryCreateSoldier(entity.id.Value);
                else
                    _soldiersModelFactory.TryRemoveSoldier(entity.id.Value);
            }
        }
    }
} 