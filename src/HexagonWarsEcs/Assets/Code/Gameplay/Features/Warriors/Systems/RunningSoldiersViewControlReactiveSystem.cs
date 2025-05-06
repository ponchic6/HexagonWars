using System.Collections.Generic;
using Code.Gameplay.Features.Warriors.Services;
using Entitas;

namespace Code.Gameplay.Features.Warriors.Systems
{
    public class RunningSoldiersViewControlReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly ISoldiersModelFactory _soldiersModelFactory;
        private readonly GameContext _game;

        public RunningSoldiersViewControlReactiveSystem(IContext<GameEntity> context, ISoldiersModelFactory soldiersModelFactory) : base(context)
        {
            _soldiersModelFactory = soldiersModelFactory;
            _game = Contexts.sharedInstance.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.MigrationComplexityWay);

        protected override bool Filter(GameEntity entity) =>
            entity.hasWarriorsMigrationAmount;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                if (entity.migrationComplexityWay.Value.Count == 0)
                    continue;
                
                if (entity.migrationComplexityWay.Value[0] > 0)
                    continue;
                
                GameEntity currentHex = _game.GetEntityWithId(entity.wayIdPoints.Value[0]);
                GameEntity nextHex = _game.GetEntityWithId(entity.wayIdPoints.Value[1]);
                    
                _soldiersModelFactory.CreateAndMoveSoldierModel(currentHex, nextHex);
            }
        }
    }
} 