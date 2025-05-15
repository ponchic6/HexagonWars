using Code.Infrastructure.StaticData;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.Gameplay.Features.Production.View.UI
{
    public class CityProductionHandler : ProductionHandler
    {
        [SerializeField] private TMP_Text _orderedCitizens;
        [SerializeField] private Button _plusButton;
        [SerializeField] private Button _minusButton;
        [SerializeField] private Image _cooldownImage;
        private GameEntity _hexEntity;
        private CommonStaticData _commonStaticData;

        [Inject]
        public void Construct(CommonStaticData commonStaticData) =>
            _commonStaticData = commonStaticData;

        public override void Setup(GameEntity hexEntity)
        {
            _hexEntity = hexEntity;
            _orderedCitizens.text = _hexEntity.city.CitizenOrdered.ToString();

            _plusButton.onClick.AsObservable().Subscribe(_ =>
            {
                if (_hexEntity.foodAmount.Value < _commonStaticData.FoodPricePerCitizen)
                    return;

                _hexEntity.city.CitizenOrdered++;
                _hexEntity.foodAmount.Value -= _commonStaticData.FoodPricePerCitizen;
                _orderedCitizens.text = _hexEntity.city.CitizenOrdered.ToString();
            }).AddTo(this);
            
            _minusButton.onClick.AsObservable().Subscribe(_ =>
            {
                if (_hexEntity.city.CitizenOrdered <= 0)
                    return;
                
                _hexEntity.foodAmount.Value += _commonStaticData.FoodPricePerCitizen;
                _orderedCitizens.text = _hexEntity.city.CitizenOrdered.ToString();
            }).AddTo(this);
        }

        public override void UpdateProductionUi()
        {
            if (_hexEntity == null)
                return;
            
            _cooldownImage.fillAmount = _hexEntity.city.CurrentCooldown / _hexEntity.city.Cooldown;
            _orderedCitizens.text = _hexEntity.city.CitizenOrdered.ToString();
        }
    }
}