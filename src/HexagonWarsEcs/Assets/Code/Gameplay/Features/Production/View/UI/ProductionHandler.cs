using System;
using Code.Gameplay.Features.Building;
using Code.Gameplay.Features.Building.DataStructure;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.Features.Production.View.UI
{
    public class ProductionHandler : MonoBehaviour
    {
        public event Action<float> OnSliderValueChanged;
        
        [SerializeField] private TMP_Text _buildingName;
        [SerializeField] private Slider _slider;

        public void Setup(GameEntity entity, BuildProgressContainer build)
        {
            switch (build.buildingType)
            {
                case BuildingsType.FoodFarm:
                    _slider.value = (float)entity.foodFarm.Workers / (entity.foodFarm.Workers + entity.citizensAmount.Value);
                    _buildingName.text = BuildingsType.FoodFarm.ToString();
                    break;
            }
            
            _slider.onValueChanged.AsObservable().Subscribe(x =>
            {
                OnSliderValueChanged?.Invoke(x);
            }).AddTo(this);
        }
    }
}
