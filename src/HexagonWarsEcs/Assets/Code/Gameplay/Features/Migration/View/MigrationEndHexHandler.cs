using Code.Gameplay.Common.Services;
using Code.Gameplay.Common.View;
using Code.Gameplay.Features.Map.View;
using Code.Gameplay.Features.Migration.Services;
using Code.Infrastructure.View;
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
        private IUIFactory _uiFactory;
        private IMigrationFactory _migrationFactory;
        private CommonMigrationToggleGroup _migrationToggleGroup;

        [Inject]
        public void Construct(IMigrationFactory migrationFactory, IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _migrationFactory = migrationFactory;
        }
        
        private void Awake()
        {
            _migrationToggleGroup = GetComponentInParent<CommonMigrationToggleGroup>();

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

            _migrationFactory.SetFinishHexAndCreateMigration(_entityBehaviour);
            _uiFactory.SliderMigrationChooserDeactivate();
            _migrationToggleGroup.AllTogglesOff();
        }
    }
}