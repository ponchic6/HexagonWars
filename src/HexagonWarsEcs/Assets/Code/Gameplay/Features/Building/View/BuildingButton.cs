using Code.Gameplay.Features.Building.DataStructure;
using Code.Gameplay.Features.Production.View.UI;
using Code.Infrastructure.View;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Code.Gameplay.Features.Building.View
{
    public class BuildingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _buildStatusImage;
        [SerializeField] private TMP_InputField _workersInputField;
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private Button _plusWorkersButton;
        [SerializeField] private Button _fivePlusWorkersButton;
        [SerializeField] private Button _minusWorkersButton;
        [SerializeField] private Button _fiveMinusWorkersButton;
        [SerializeField] private Slider _buildProgressSlider;
        [SerializeField] private RectTransform _managePanel;
        [SerializeField] private TMP_Text _buildingName;
        [SerializeField] private float _slideDistance;
        [SerializeField] private float _animationDuration;
        private RectTransform _verticalLayoutGroupRectTransform;
        private RectTransform _rectTransform;
        private float _startManagePanelPosition;
        private float _finishManagePanelPosition;
        private Vector2 _initialSize;
        private Vector2 _openedSize;
        private EntityBehaviour _hexEntityBehaviour;
        private BuildProgressContainer _buildProgress;
        private ProductionHandler _productionHandler;
        private DiContainer _dicontainer;

        public BuildProgressContainer BuildingProgressContainer => _buildProgress;

        [Inject]
        public void Construct(DiContainer container) =>
            _dicontainer = container;

        private void Awake()
        {
            CalculatePositions();
            _plusWorkersButton.onClick.AsObservable().Subscribe(OnPlusButton).AddTo(this);
            _minusWorkersButton.onClick.AsObservable().Subscribe(OnMinusButton).AddTo(this);
            
            _fivePlusWorkersButton.onClick.AsObservable().Subscribe(unit =>
            {
                for (int i = 0; i < 5; i++) 
                    OnPlusButton(unit);
            }).AddTo(this);
            
            _fiveMinusWorkersButton.onClick.AsObservable().Subscribe(unit =>
            {
                for (int i = 0; i < 5; i++) 
                    OnMinusButton(unit);
            }).AddTo(this);
        }

        public void Setup(BuildProgressContainer buildProgress, EntityBehaviour entityBehaviour)
        {
            _buildProgress = buildProgress;
            _hexEntityBehaviour = entityBehaviour;
            _buildStatusImage.color = Color.red;
            _statusText.text = "Не построено";
            _buildingName.text = $"{buildProgress.buildingType}";
            _workersInputField.text = _buildProgress.buildersAmount.ToString();
            _productionHandler = _dicontainer.InstantiatePrefabResourceForComponent<ProductionHandler>(
                 $"Hexagons/UI/BuildingUiControllers/{buildProgress.buildingType}", _managePanel.transform);
            _productionHandler.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _rectTransform.DOSizeDelta(_openedSize, _animationDuration);
            _managePanel
                .DOAnchorPosY(_finishManagePanelPosition, _animationDuration)
                .SetEase(Ease.OutQuad)
                .OnUpdate(() => LayoutRebuilder.ForceRebuildLayoutImmediate(_verticalLayoutGroupRectTransform));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _rectTransform.DOSizeDelta(_initialSize, _animationDuration);
            _managePanel
                .DOAnchorPosY(_startManagePanelPosition, _animationDuration)
                .SetEase(Ease.OutQuad)
                .OnUpdate(() => LayoutRebuilder.ForceRebuildLayoutImmediate(_verticalLayoutGroupRectTransform));
        }

        public void UpdateUI(BuildProgressContainer buildProgress)
        {
            _buildProgressSlider.value = buildProgress.currentProgress / buildProgress.fullProgress;
        }

        public void SetBuildedStatus()
        {
            _plusWorkersButton.gameObject.SetActive(false);
            _minusWorkersButton.gameObject.SetActive(false);
            _workersInputField.gameObject.SetActive(false);
            _buildProgressSlider.gameObject.SetActive(false);
            _buildStatusImage.color = Color.green;
            _statusText.text = "Построено";
            _productionHandler.gameObject.SetActive(true);
            _productionHandler.Setup(_hexEntityBehaviour.Entity);
        }

        private void OnPlusButton(Unit _)
        {
            if (_hexEntityBehaviour.Entity.manAmount.Value <= 0)
                return;
            
            _hexEntityBehaviour.Entity.ReplaceManAmount(_hexEntityBehaviour.Entity.manAmount.Value - 1);
            _buildProgress.buildersAmount++;
            _workersInputField.text = _buildProgress.buildersAmount.ToString();
        }

        private void OnMinusButton(Unit _)
        {
            if (_buildProgress.buildersAmount <= 0)
                return;
            
            _hexEntityBehaviour.Entity.ReplaceManAmount(_hexEntityBehaviour.Entity.manAmount.Value + 1);
            _buildProgress.buildersAmount--;
            _workersInputField.text = _buildProgress.buildersAmount.ToString();
        }

        private void CalculatePositions()
        {
            _startManagePanelPosition = _managePanel.anchoredPosition.y;
            _finishManagePanelPosition = _managePanel.anchoredPosition.y - _slideDistance;
            
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _verticalLayoutGroupRectTransform = GetComponentInParent<VerticalLayoutGroup>().GetComponent<RectTransform>();
            
            _initialSize = _rectTransform.sizeDelta;
            Vector2 sizeDelta = _rectTransform.sizeDelta;
            sizeDelta.y += _slideDistance;
            _openedSize = sizeDelta;
        }
    }
}