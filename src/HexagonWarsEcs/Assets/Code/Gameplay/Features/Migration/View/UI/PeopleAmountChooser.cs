using System;
using Code.Gameplay.Features.Map.View;
using Code.Gameplay.Features.Migration.Services;
using Code.Infrastructure.View;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Code.Gameplay.Features.Migration.View.UI
{
    public class PeopleAmountChooser : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _sliderStandartScale;
        [SerializeField] private float _sliderMouseScale;
        [SerializeField] private HexagonPopulationView _hexagonPopulationView;
        [SerializeField] private EntityBehaviour _entityBehaviour;
        
        private MigrationAllSlidersBlocker _migrationAllSlidersBlocker;
        private TweenerCore<Vector3,Vector3,VectorOptions> _sliderTween;
        private int _selectedPeople;
        private IMigrationFactory _migrationFactory;

        [Inject]
        public void Construct(IMigrationFactory migrationFactory)
        {
            _migrationFactory = migrationFactory;
        }

        private void Awake()
        {
            _slider.transform.localScale *= _sliderStandartScale;
            _slider.onValueChanged.AsObservable().Subscribe(OnSliderValueChanged).AddTo(this);
            
            _migrationAllSlidersBlocker = GetComponentInParent<MigrationAllSlidersBlocker>();

            _migrationAllSlidersBlocker.isSlidersBlocked.Subscribe(x => HidePeopleUi()).AddTo(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _migrationAllSlidersBlocker.isSlidersBlocked.Value = false;
            }
        }

        public void OnPointerEnter(PointerEventData eventData) =>
            ShowPeopleUi();

        public void OnPointerExit(PointerEventData eventData) =>
            HidePeopleUi();

        private void OnSliderValueChanged(float sliderValue)
        {
            if (sliderValue == 0)
                return;
            
            int value = _entityBehaviour.Entity.citizensAmount.Value;
            _selectedPeople = (int)Math.Round(value * sliderValue);
            _text.text = _selectedPeople + "/" + value;
            _migrationAllSlidersBlocker.isSlidersBlocked.Value = true;
            _migrationFactory.SetInitialHex(_entityBehaviour, _selectedPeople);
        }

        private void ShowPeopleUi()
        {
            if (_migrationAllSlidersBlocker.isSlidersBlocked.Value) 
                return;
            
            _hexagonPopulationView.enabled = false;
            
            if (_sliderTween != null)
                _sliderTween.Complete();
            
            _sliderTween = _slider.transform.DOScale(_sliderMouseScale, 0.3f).SetEase(Ease.OutQuad);
        }

        private void HidePeopleUi()
        {
            if (_migrationAllSlidersBlocker.isSlidersBlocked.Value) 
                return;
            
            _hexagonPopulationView.enabled = true;
            
            if (_sliderTween != null)
                _sliderTween.Complete();

            _sliderTween = _slider.transform.DOScale(_sliderStandartScale, 0.3f).SetEase(Ease.OutQuad);
            _slider.value = 0;
        }
    }
}