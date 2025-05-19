using Code.Gameplay.Common;
using Code.Gameplay.Common.Services;
using Code.Gameplay.Common.View;
using Code.Gameplay.Features.Map.View;
using Code.Gameplay.Features.Migration.Services;
using Code.Infrastructure.View;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.Gameplay.Features.Migration.View
{
    public class MigrationStartHexHandler : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private PointerHandler _pointerHandler;
        [SerializeField] private Toggle _citizenToggle;
        [SerializeField] private Toggle _warriorsToggle;
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
            CommonMigrationToggleGroup migrationToggleGroup = GetComponentInParent<CommonMigrationToggleGroup>();
            migrationToggleGroup.AddToggle(_warriorsToggle);
            migrationToggleGroup.AddToggle(_citizenToggle);

            _citizenToggle.onValueChanged.AsObservable().Subscribe(isOn =>
            {
                if (!isOn)
                {
                    _uiFactory.SliderMigrationChooserDeactivate();
                    return;
                }
             
                migrationToggleGroup.AllTogglesOffExceptOne(_warriorsToggle, _citizenToggle);
                DestructAllMigrationTrailsView();
                _uiFactory.SliderMigrationChooserActivate(_entityBehaviour, ManMigrationType.Citizens);
                _migrationFactory.CreateMigrationViewTrail(_entityBehaviour, ManMigrationType.Citizens);
                
            }).AddTo(this);
            
            _warriorsToggle.onValueChanged.AsObservable().Subscribe(isOn =>
            {
                if (!isOn)
                {
                    _uiFactory.SliderMigrationChooserDeactivate();
                    return;
                }
             
                migrationToggleGroup.AllTogglesOffExceptOne(_warriorsToggle, _citizenToggle);
                DestructAllMigrationTrailsView();
                _uiFactory.SliderMigrationChooserActivate(_entityBehaviour, ManMigrationType.Warriors);
                _migrationFactory.CreateMigrationViewTrail(_entityBehaviour, ManMigrationType.Warriors);
                
            }).AddTo(this);
            
            gameObject
                .UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Escape))
                .Subscribe(_ =>
                {
                    DestructAllMigrationTrailsView();
                    AllTogglesOff();
                })
                .AddTo(this);
        }

        public void CitizenButtonActive(bool enable) =>
            _citizenToggle.gameObject.SetActive(enable);

        public void WarriorButtonActive(bool enable) =>
            _warriorsToggle.gameObject.SetActive(enable);

        private void DestructAllMigrationTrailsView()
        {
            foreach (GameEntity trailEntity in _game.GetGroup(GameMatcher.MigrationArrow))
                trailEntity.isDestructed = true;
        }

        private void AllTogglesOff()
        {
            _warriorsToggle.isOn = false;
            _citizenToggle.isOn = false;
        }
    }
}