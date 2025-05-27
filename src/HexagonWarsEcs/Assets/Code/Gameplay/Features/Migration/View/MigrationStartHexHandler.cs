using Code.Gameplay.Common.View;
using Code.Infrastructure.View;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.Features.Migration.View
{
    public class MigrationStartHexHandler : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private PointerHandler _pointerHandler;
        [SerializeField] private Toggle _citizenToggle;
        [SerializeField] private Toggle _warriorsToggle;
        
        private void Awake()
        {
            _citizenToggle.onValueChanged.AsObservable().Subscribe(isOn => _entityBehaviour.Entity.isCitizenToggleEnabling = isOn).AddTo(this);
            _warriorsToggle.onValueChanged.AsObservable().Subscribe(isOn => _entityBehaviour.Entity.isSoldiersToggleEnabling = isOn).AddTo(this);
            
            gameObject
                .UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Escape))
                .Subscribe(_ =>
                {
                    _entityBehaviour.Entity.isCitizenToggleEnabling = false;
                    _entityBehaviour.Entity.isSoldiersToggleEnabling = false;
                })
                .AddTo(this);
        }

        public void CitizenButtonActive(bool enable) =>
            _citizenToggle.gameObject.SetActive(enable);

        public void WarriorButtonActive(bool enable) =>
            _warriorsToggle.gameObject.SetActive(enable);
        
        public void AllTogglesOffWithoutNotify()
        {
            _citizenToggle.SetIsOnWithoutNotify(false);
            _warriorsToggle.SetIsOnWithoutNotify(false);
        }
    }
}