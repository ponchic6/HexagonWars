using Code.Gameplay.Common.Services;
using Code.Gameplay.Common.View;
using Code.Gameplay.Features.Migration.Services;
using Code.Infrastructure.View;
using Entitas;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Code.Gameplay.Features.Migration.View
{
    public class MigrationEndHexHandler : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private PointerHandler _pointerHandler;
        [SerializeField] private Toggle _citizenToggle;
        [SerializeField] private Toggle _warriorsToggle;
        private IMigrationFactory _migrationFactory;
        private GameContext _game;

        [Inject]
        public void Construct(IMigrationFactory migrationFactory, IUIFactory uiFactory)
        {
            _migrationFactory = migrationFactory;
            _game = Contexts.sharedInstance.game;
        }
        
        private void Awake()
        {
            _pointerHandler.OnPointerDownEvent += OnPointerDown;
        }

        private void OnDisable() =>
            _pointerHandler.OnPointerDownEvent -= OnPointerDown;

        private void OnPointerDown(PointerEventData eventData)
        {
            if (_entityBehaviour.Entity.isEnemyHexagon)
                return;

            if (eventData.button != PointerEventData.InputButton.Right)
                return;

            IGroup<GameEntity> toggles = _game.GetGroup(GameMatcher.AnyOf(GameMatcher.CitizenToggleEnabling, GameMatcher.SoldiersToggleEnabling));
            
            foreach (GameEntity entity in toggles.GetEntities())
            {
                entity.isCitizenToggleEnabling = false;
                entity.isSoldiersToggleEnabling = false;
            }

            _migrationFactory.SetFinishHexAndCreateMigration(_entityBehaviour);
        }
    }
}