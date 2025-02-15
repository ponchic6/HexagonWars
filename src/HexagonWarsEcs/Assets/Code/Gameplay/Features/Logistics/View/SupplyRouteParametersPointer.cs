using Code.Gameplay.Common.Services;
using Code.Gameplay.Common.View;
using Code.Infrastructure.View;
using DG.Tweening;
using Logic.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.Gameplay.Features.Logistics.View
{
    public class SupplyRouteParametersPointer : MonoBehaviour
    {
        [SerializeField] private PointerHandler _pointerHandler;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _scaleFactor;
        [SerializeField] private float _durationOfScaling;
        [SerializeField] private EntityBehaviour _entityBehaviour;
        
        private Tweener _lineRendererScalingTweener;
        private float _startWidthCache;
        
        private IUIFactory _uiFactory;

        [Inject]
        public void Constructor(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }
        
        private void Awake()
        {
            _pointerHandler.OnPointerEnterEvent += OnPointerHandlerEnterEvent;
            _pointerHandler.OnPointerExitEvent += OnPointerHandlerExitEvent;
            _pointerHandler.OnPointerDownEvent += OnPointerHandlerDownEvent;
            _startWidthCache = _lineRenderer.startWidth;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                BackButtonDown();
        }

        private void OnDisable()
        {
            _pointerHandler.OnPointerEnterEvent -= OnPointerHandlerEnterEvent;
            _pointerHandler.OnPointerExitEvent -= OnPointerHandlerExitEvent;
            _pointerHandler.OnPointerDownEvent -= OnPointerHandlerDownEvent;
        }

        private void BackButtonDown() =>
            _uiFactory.HideInfoPanel(_entityBehaviour);

        private void OnPointerHandlerDownEvent(PointerEventData obj) =>
            _uiFactory.ShowInfoPanel(_entityBehaviour);

        private void OnPointerHandlerExitEvent(PointerEventData obj) =>
            UnscaleLineRenderer();

        private void OnPointerHandlerEnterEvent(PointerEventData pointerEventData) =>
            ScaleLineRendererTo();

        private void ScaleLineRendererTo()
        {
            _lineRendererScalingTweener?.Kill();
            _lineRendererScalingTweener = DOTween.To(() => _lineRenderer.startWidth, x =>
            {
                _lineRenderer.startWidth = x;
                _lineRenderer.endWidth = x;
            }, _startWidthCache * _scaleFactor, _durationOfScaling);
        }

        private void UnscaleLineRenderer()
        {
            _lineRendererScalingTweener?.Kill();
            _lineRendererScalingTweener = DOTween.To(() => _lineRenderer.startWidth, x =>
            {
                _lineRenderer.startWidth = x;
                _lineRenderer.endWidth = x;
            }, _startWidthCache, _durationOfScaling);
        }
    }
}