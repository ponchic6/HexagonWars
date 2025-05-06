using System.Collections.Generic;
using Code.Gameplay.Features.Citizens.Services;
using Entitas;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class RunningCitizensViewControlReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly ICitizensModelFactory _citizensModelFactory;
        private readonly GameContext _game;

        public RunningCitizensViewControlReactiveSystem(IContext<GameEntity> context, ICitizensModelFactory citizensModelFactory) : base(context)
        {
            _citizensModelFactory = citizensModelFactory;
            _game = Contexts.sharedInstance.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.MigrationComplexityWay);

        protected override bool Filter(GameEntity entity) =>
            entity.hasCitizensMigrationAmount;

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
                    
                _citizensModelFactory.CreateAndMoveCitizenModel(currentHex, nextHex);
            }
        }
    }
}