using System.Collections.Generic;
using Code.Gameplay.Common.Services;
using Code.Infrastructure.View;
using Entitas;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationTogglesForChooserReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly IUIFactory _uiFactory;

        public MigrationTogglesForChooserReactiveSystem(IContext<GameEntity> context, IUIFactory uiFactory) : base(context)
        {
            _uiFactory = uiFactory;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(
                GameMatcher.ManToggleEnabling.AddedOrRemoved()
            );

        protected override bool Filter(GameEntity entity) =>
            true;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                EntityBehaviour entityBehaviour = entity.view.Value;
                
                if (entity.isManToggleEnabling)
                    _uiFactory.SliderMigrationChooserActivate(entityBehaviour);
                else 
                    entity.migrationHandler.Value.AllTogglesOffWithoutNotify();
            }
        }
    }
}
