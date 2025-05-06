using Code.Gameplay.Common;
using Code.Gameplay.Common.Services;
using Code.Gameplay.Common.View;
using Code.Gameplay.Features.Migration.Services;
using Code.Infrastructure.View;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Code.Gameplay.Features.Migration.View
{
    public class MigrationHandler : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private PointerHandler _pointerHandler;
        [SerializeField] private Button _citizenButton;
        [SerializeField] private Button _warriorButton;
        private IMigrationFactory _migrationFactory;
        private IUIFactory _uiFactory;

        [Inject]
        public void Construct(IMigrationFactory migrationFactory, IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _migrationFactory = migrationFactory;
        }

        private void Awake()
        {
            _citizenButton.onClick.AsObservable().Subscribe(_ =>
            {
                _uiFactory.SliderMigrationChooserActivate(_entityBehaviour, ManMigrationType.Citizens);
            }).AddTo(this);
            
            _warriorButton.onClick.AsObservable().Subscribe(_ =>
            {
                _uiFactory.SliderMigrationChooserActivate(_entityBehaviour, ManMigrationType.Warriors);
            }).AddTo(this);
            
            _pointerHandler.OnPointerDownEvent += OnPointerDown;
        }

        private void OnDisable() =>
            _pointerHandler.OnPointerDownEvent -= OnPointerDown;

        public void CitizenButtonActive(bool enable) =>
            _citizenButton.gameObject.SetActive(enable);
        
        public void WarriorButtonActive(bool enable) =>
            _warriorButton.gameObject.SetActive(enable);

        private void OnPointerDown(PointerEventData eventData)
        {
            if (_entityBehaviour.Entity.isEnemyHexagon)
                return;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                _migrationFactory.SetFinishHexAndCreateMigration(_entityBehaviour);
                _uiFactory.SliderMigrationChooserDeactivate();
            }
        }
    }
}