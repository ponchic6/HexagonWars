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
        public event Action<int> OnInputFieldSubmit;
        
        [SerializeField] private TMP_Text _buildingName;
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_InputField _inputField;

        public Slider Slider => _slider;
        public TMP_InputField InputField => _inputField;

        public void Setup(GameEntity entity, BuildProgressContainer build)
        {
            switch (build.buildingType)
            {
                case BuildingsType.FoodFarm:
                    _slider.value = (float)entity.foodFarm.Workers / (entity.foodFarm.Workers + entity.citizensAmount.Value);
                    _buildingName.text = BuildingsType.FoodFarm.ToString();
                    _slider.onValueChanged.AsObservable().Subscribe(x => OnSliderValueChanged?.Invoke(x)).AddTo(this);
                    _inputField.gameObject.SetActive(false);
                    break;
                
                case BuildingsType.Barracks:
                    _buildingName.text = BuildingsType.Barracks.ToString();
                    _inputField.onSubmit.AsObservable().Subscribe(x => OnInputFieldSubmit?.Invoke(int.Parse(x))).AddTo(this);
                    _slider.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
