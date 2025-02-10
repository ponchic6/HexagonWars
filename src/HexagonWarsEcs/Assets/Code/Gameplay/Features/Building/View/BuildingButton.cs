using System;
using Code.Gameplay.Features.Building.DataStructure;
using Code.Infrastructure.View;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.Features.Building.View
{
    public class BuildingButton : MonoBehaviour
    {
        public event Action<BuildProgressContainer> OnCancelButton;
        public event Action<float> OnSliderValueChanged;
        
        [SerializeField] private Slider _slider;
        [SerializeField] private Button _cancelBuildingButton;
        [SerializeField] private Button _startBuildingButton;
        private float _maxProgress;
        
        public Slider Slider => _slider;

        public void UpdateUI(BuildProgressContainer buildProgress) =>
            _cancelBuildingButton.image.fillAmount = buildProgress.currentProgress / buildProgress.fullProgress;

        public void Setup(BuildProgressContainer buildProgress, EntityBehaviour entityBehaviour)
        {
            if (buildProgress.currentProgress != 0)
            {
                _startBuildingButton.gameObject.SetActive(false);
                _slider.gameObject.SetActive(true);
                _slider.value = (float)buildProgress.buildersAmount / (entityBehaviour.Entity.citizensAmount.Value + buildProgress.buildersAmount);
                UpdateUI(buildProgress);
            }
            
            _cancelBuildingButton.onClick.AsObservable().Subscribe(_ =>
            {
                _startBuildingButton.gameObject.SetActive(true);
                _slider.gameObject.SetActive(false);
                OnCancelButton?.Invoke(buildProgress);
            }).AddTo(this);
            
            _startBuildingButton.onClick.AsObservable().Subscribe(_ =>
            {
                _startBuildingButton.gameObject.SetActive(false);
                _slider.gameObject.SetActive(true);
                _slider.value = 0;
            }).AddTo(this);
            
            _slider.onValueChanged.AsObservable().Subscribe(x =>
            {
                OnSliderValueChanged?.Invoke(x);
            }).AddTo(this);
        }
    }
}