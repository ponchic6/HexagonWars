using System.Collections.Generic;
using Code.Gameplay.Common.Services;
using Entitas;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationChooserUiReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly IUIFactory _uiFactory;

        public MigrationChooserUiReactiveSystem(IContext<GameEntity> context, IUIFactory uiFactory) : base(context)
        {
            _uiFactory = uiFactory;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.AnyOf(
                GameMatcher.ManAmount));

        protected override bool Filter(GameEntity entity) =>
            _uiFactory.MigrationAmountChooser.gameObject.activeSelf &&
            _uiFactory.MigrationAmountChooser.EntityBehaviour.Entity.id.Value == entity.id.Value;

        protected override void Execute(List<GameEntity> entities) =>
            _uiFactory.MigrationAmountChooser.UpdateUi();
    }
}