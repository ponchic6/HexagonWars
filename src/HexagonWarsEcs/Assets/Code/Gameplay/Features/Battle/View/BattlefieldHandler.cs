using Code.Gameplay.Common.Services;
using Code.Gameplay.Common.View;
using Code.Gameplay.Features.Battle.Services;
using Code.Infrastructure.View;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.Gameplay.Features.Battle.View
{
    public class BattlefieldHandler : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private PointerHandler _pointerHandler;
        private IBattleFieldFactory _battleFieldFactory;
        private IUIFactory _uiFactory;

        [Inject]
        public void Construct(IBattleFieldFactory battleFieldFactory, IUIFactory uiFactory)
        {
            _battleFieldFactory = battleFieldFactory;
            _uiFactory = uiFactory;
        }

        private void Awake() =>
            _pointerHandler.OnPointerDownEvent += OnPointerDown;

        private void OnDisable() =>
            _pointerHandler.OnPointerDownEvent -= OnPointerDown;
        
        private void OnPointerDown(PointerEventData eventData)
        {
            if (!_entityBehaviour.Entity.isEnemyHexagon)
                return;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                _battleFieldFactory.TrySetDefendersAndCreateBattlefield(_entityBehaviour); 
                _uiFactory.SliderMigrationChooserDeactivate();
            }
        }
    }
}