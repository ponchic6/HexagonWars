using System;
using Code.Gameplay.Features.Battle.Services;
using Code.Gameplay.Features.Migration.Services;
using Code.Infrastructure.View;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.Gameplay.Features.Migration.View.UI
{
    public class MigrationAmountChooser : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _sliderStandartScale;
        [SerializeField] private float _sliderMouseScale;
        
        private TweenerCore<Vector3,Vector3,VectorOptions> _sliderTween;
        private int _selectedPeople;
        private IMigrationFactory _migrationFactory;
        private EntityBehaviour _entityBehaviour;
        private IBattleFieldFactory _battleFieldFactory;

        public EntityBehaviour EntityBehaviour => _entityBehaviour;

        [Inject]
        public void Construct(IMigrationFactory migrationFactory, IBattleFieldFactory battleFieldFactory)
        {
            _migrationFactory = migrationFactory;
            _battleFieldFactory = battleFieldFactory;
        }

        private void Awake()
        {
            _slider.transform.localScale *= _sliderStandartScale;
            _slider.onValueChanged.AsObservable().Subscribe(OnSliderValueChanged).AddTo(this);
        }
        
        public void Show(EntityBehaviour hexEntity)
        { ;
            _selectedPeople = 0;
            
            _entityBehaviour = hexEntity;
            _text.text = _entityBehaviour.Entity.manAmount.Value + " человек";

            _slider.value = 0;
            gameObject.SetActive(true);
            
            if (_sliderTween != null)
                _sliderTween.Complete();
            
            _sliderTween = _slider.transform.DOScale(_sliderMouseScale, 0.3f).SetEase(Ease.OutQuad);
        }

        public void Hide()
        {
            if (_sliderTween != null)
                _sliderTween.Complete();

            _sliderTween = _slider.transform
                .DOScale(_sliderStandartScale, 0.3f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    _entityBehaviour = null;
                });
            
            _slider.value = 0;
        }

        public void UpdateUi()
        {
            if (_slider.value == 0)
                _text.text = _entityBehaviour.Entity.manAmount.Value + " человек";
            else
            {
                int value = _entityBehaviour.Entity.manAmount.Value;
                _selectedPeople = (int)Math.Round(value * _slider.value);
                _text.text = _selectedPeople + "/" + value + " человек";
            }
        }

        private void OnSliderValueChanged(float sliderValue)
        {
            if (_entityBehaviour == null)
                return;

            int value = _entityBehaviour.Entity.manAmount.Value;
            _selectedPeople = (int)Math.Round(value * sliderValue);
            _text.text = _selectedPeople + "/" + value + " человек";
            _migrationFactory.SetInitialHex(_entityBehaviour, _selectedPeople);
        }
    }
}