using Code.Gameplay.Common;
using Code.Gameplay.Common.Services;
using Code.Gameplay.Common.View;
using Code.Gameplay.Features.Migration.Services;
using Code.Infrastructure.View;
using UniRx;
using UniRx.Triggers;
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
        private GameContext _game;

        [Inject]
        public void Construct(IMigrationFactory migrationFactory, IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _migrationFactory = migrationFactory;
            _game = Contexts.sharedInstance.game;
        }

        private void Awake()
        {
            _citizenButton.onClick.AsObservable().Subscribe(_ =>
            {
                DestructAllMigrationTrailsView();
                _uiFactory.SliderMigrationChooserActivate(_entityBehaviour, ManMigrationType.Citizens);
                _migrationFactory.CreateMigrationViewTrail(_entityBehaviour, ManMigrationType.Citizens);
            }).AddTo(this);
            
            _warriorButton.onClick.AsObservable().Subscribe(_ =>
            {
                DestructAllMigrationTrailsView();
                _uiFactory.SliderMigrationChooserActivate(_entityBehaviour, ManMigrationType.Warriors);
                _migrationFactory.CreateMigrationViewTrail(_entityBehaviour, ManMigrationType.Warriors);
            }).AddTo(this);
            
            gameObject
                .UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Escape))
                .Subscribe(_ => DestructAllMigrationTrailsView())
                .AddTo(this);

            _pointerHandler.OnPointerDownEvent += OnPointerDown;
        }

        private void DestructAllMigrationTrailsView()
        {
            foreach (GameEntity trailEntity in _game.GetGroup(GameMatcher.MigrationArrow))
                trailEntity.isDestructed = true;
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