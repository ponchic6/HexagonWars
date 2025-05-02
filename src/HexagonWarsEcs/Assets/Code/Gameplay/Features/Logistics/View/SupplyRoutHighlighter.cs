using Code.Infrastructure.View;
using DG.Tweening;
using UnityEngine;

namespace Code.Gameplay.Features.Logistics.View
{
    public class SupplyRoutHighlighter : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _scaleFactor;
        [SerializeField] private float _durationOfScaling;
        [SerializeField] private EntityBehaviour _entityBehaviour;
        
        private Tweener _lineRendererScalingTweener;
        private float _startWidthCache;
        
        private void Awake()
        {
            _startWidthCache = _lineRenderer.startWidth;
        }
        
        public void HighlightRout()
        {
            _lineRendererScalingTweener?.Kill();
            _lineRenderer.material.SetColor("_Color", Color.blue);
            _lineRenderer.sortingOrder = 1000;
            _lineRendererScalingTweener = DOTween.To(() => _lineRenderer.startWidth, x =>
            {
                _lineRenderer.startWidth = x;
                _lineRenderer.endWidth = x;
            }, _startWidthCache * _scaleFactor, _durationOfScaling);
        }

        public void UnhighlightRout()
        {
            _lineRendererScalingTweener?.Kill();
            _lineRenderer.material.SetColor("_Color", Color.green);
            _lineRenderer.sortingOrder = 1;
            _lineRendererScalingTweener = DOTween.To(() => _lineRenderer.startWidth, x =>
            {
                _lineRenderer.startWidth = x;
                _lineRenderer.endWidth = x;
            }, _startWidthCache, _durationOfScaling);
        }
    }
}