using System;
using Code.Gameplay.Common;
using Code.Gameplay.Features.Battle.Services;
using Code.Gameplay.Features.Migration.Services;
using Code.Gameplay.Features.Migration.View.UI;
using Code.Infrastructure.View;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Logic.Common;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Code.Gameplay.Features.Battle.View.UI
{
    public class WarriorsAmountChooser : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _sliderStandartScale;
        [SerializeField] private float _sliderMouseScale;
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private PointerHandler _pointerHandler;

        private MigrationAllSlidersBlocker _migrationAllSlidersBlocker;
        private TweenerCore<Vector3,Vector3,VectorOptions> _sliderTween;
        private IBattleFieldFactory _battleFieldFactory;
        private IMigrationFactory _migrationFactory;
        private int _selectedWarriors;

        [Inject]
        public void Construct(IBattleFieldFactory battleFieldFactory, IMigrationFactory migrationFactory)
        {
            _migrationFactory = migrationFactory;
            _battleFieldFactory = battleFieldFactory;
        }

        private void Awake()
        {
            _slider.transform.localScale *= _sliderStandartScale;
            _slider.onValueChanged.AsObservable().Subscribe(OnSliderValueChanged).AddTo(this);
            
            _migrationAllSlidersBlocker = GetComponentInParent<MigrationAllSlidersBlocker>();
            _migrationAllSlidersBlocker.isSlidersBlocked.Subscribe(x => HideWarriorUi()).AddTo(this);
            
            _pointerHandler.OnPointerEnterEvent += OnPointerEnter;
            _pointerHandler.OnPointerExitEvent += OnPointerExit;
        }

        private void OnDisable()
        {
            _pointerHandler.OnPointerEnterEvent -= OnPointerEnter;
            _pointerHandler.OnPointerExitEvent -= OnPointerExit;
        }
        
        private void OnPointerEnter(PointerEventData eventData)
        {
            if (_entityBehaviour.Entity.isEnemyHexagon)
                return;

            ShowWarriorUi();
        }

        private void OnPointerExit(PointerEventData eventData)
        {
            if (_entityBehaviour.Entity.isEnemyHexagon)
                return;

            HideWarriorUi();
        }

        private void OnSliderValueChanged(float sliderValue)
        {
            if (_entityBehaviour.Entity.isEnemyHexagon)
            {
                _slider.value = 0;
                return;
            }
            
            if (_migrationAllSlidersBlocker.isSlidersBlocked.Value && _selectedWarriors == 0)
            {
                _slider.value = 0;
                return;
            }
            
            if (sliderValue == 0)
                return;
            
            int value = _entityBehaviour.Entity.warriorsAmount.Value;
            _selectedWarriors = (int)Math.Round(value * sliderValue);
            _text.text = _selectedWarriors + "/" + value;
            _migrationAllSlidersBlocker.isSlidersBlocked.Value = true;
            _battleFieldFactory.SetAttackers(_entityBehaviour, _selectedWarriors);
            _migrationFactory.SetInitialHex(_entityBehaviour, _selectedWarriors, ManMigrationType.Warriors);
        }

        private void ShowWarriorUi()
        {
            if (_migrationAllSlidersBlocker.isSlidersBlocked.Value) 
                return;
            
            if (_sliderTween != null)
                _sliderTween.Complete();
            
            _sliderTween = _slider.transform.DOScale(_sliderMouseScale, 0.3f).SetEase(Ease.OutQuad);
        }

        private void HideWarriorUi()
        {
            if (_migrationAllSlidersBlocker.isSlidersBlocked.Value) 
                return;
            
            if (_sliderTween != null)
                _sliderTween.Complete();

            _sliderTween = _slider.transform.DOScale(_sliderStandartScale, 0.3f).SetEase(Ease.OutQuad);
            _slider.value = 0;
        }

    }
}