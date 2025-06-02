using System.Collections.Generic;
using Code.Gameplay.Common.Services;
using Entitas;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationChooserHideReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly IUIFactory _uiFactory;
        private readonly GameContext _game;

        public MigrationChooserHideReactiveSystem(IContext<GameEntity> context, IUIFactory uiFactory) : base(context)
        {
            _uiFactory = uiFactory;
            _game = Contexts.sharedInstance.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.ManToggleEnabling.AddedOrRemoved());

        protected override bool Filter(GameEntity entity) =>
            true;

        protected override void Execute(List<GameEntity> entities)
        {
            IGroup<GameEntity> group = _game.GetGroup(GameMatcher.AnyOf(GameMatcher.ManToggleEnabling));

            if (group.count == 0)
                _uiFactory.SliderMigrationChooserDeactivate();
        }
    }
}