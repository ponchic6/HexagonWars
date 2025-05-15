using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.Features.Production.View.UI
{
    public class BarracksProductionHandler : ProductionHandler
    {
        [SerializeField] private TMP_Text _orderedWarriors;
        [SerializeField] private Button _plusButton;
        [SerializeField] private Button _minusButton;
        [SerializeField] private Image _cooldownImage;
        private GameEntity _hexEntity;

        public override void Setup(GameEntity hexEntity)
        {
            _hexEntity = hexEntity;
            _orderedWarriors.text = _hexEntity.barracks.WarriorsOrdered.ToString();

            _plusButton.onClick.AsObservable().Subscribe(_ =>
            {
                if (_hexEntity.citizensAmount.Value <= 0)
                    return;
                
                _hexEntity.ReplaceCitizensAmount(_hexEntity.citizensAmount.Value - 1);
                _hexEntity.barracks.WarriorsOrdered++;
                _orderedWarriors.text = _hexEntity.barracks.WarriorsOrdered.ToString();
            }).AddTo(this);
            
            _minusButton.onClick.AsObservable().Subscribe(_ =>
            {
                if (_hexEntity.barracks.WarriorsOrdered <= 0)
                    return;
                
                _hexEntity.ReplaceCitizensAmount(_hexEntity.citizensAmount.Value + 1);
                _hexEntity.barracks.WarriorsOrdered--;
                _orderedWarriors.text = _hexEntity.barracks.WarriorsOrdered.ToString();
            }).AddTo(this);
        }

        public override void UpdateProductionUi()
        {
            if (_hexEntity == null)
                return;
            
            _cooldownImage.fillAmount = _hexEntity.barracks.CurrentCooldown / _hexEntity.barracks.Cooldown;
            _orderedWarriors.text = _hexEntity.barracks.WarriorsOrdered.ToString();
        }
    }
}