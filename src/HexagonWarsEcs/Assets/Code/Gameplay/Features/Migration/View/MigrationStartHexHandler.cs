using Code.Gameplay.Common.View;
using Code.Infrastructure.View;
using TMPro;
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
        [SerializeField] private Toggle _manToggle;
        [SerializeField] private TMP_Text _citizenCount;

        private void Awake()
        {
            _manToggle.onValueChanged.AsObservable().Subscribe(isOn => _entityBehaviour.Entity.isManToggleEnabling = isOn).AddTo(this);

            gameObject
                .UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Escape))
                .Subscribe(_ =>
                {
                    _entityBehaviour.Entity.isManToggleEnabling = false;
                })
                .AddTo(this);
        }

        public void ManButtonActive(bool enable) =>
            _manToggle.gameObject.SetActive(enable);
        
        public void AllTogglesOffWithoutNotify()
        {
            _manToggle.SetIsOnWithoutNotify(false);
        }

        public void UpdateManCountsView() =>
            _citizenCount.text = _entityBehaviour.Entity.manAmount.Value.ToString();
    }
}
