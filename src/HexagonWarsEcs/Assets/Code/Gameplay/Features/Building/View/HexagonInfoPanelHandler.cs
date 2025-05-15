using Code.Gameplay.Common.Services;
using Code.Gameplay.Common.View;
using Code.Gameplay.Features.Map.View;
using Code.Infrastructure.View;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.Gameplay.Features.Building.View
{
    public class HexagonInfoPanelHandler : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private PointerHandler _pointerHandler;
        [SerializeField] private GameObject _outline;
        private IUIFactory _uiFactory;
        private MapOutlinesController _outlinesController;

        public GameObject Outline => _outline;

        [Inject]
        public void Construct(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        private void Awake()
        {
            _pointerHandler.OnPointerDownEvent += OnPointerDown;
            _outlinesController = GetComponentInParent<MapOutlinesController>();
            _outlinesController.AddOutline(_outline);
        }

        private void OnDisable() =>
            _pointerHandler.OnPointerDownEvent -= OnPointerDown;

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) 
                return;
            
            _uiFactory.HideInfoPanel(_entityBehaviour);
            _outlinesController.DeactivateAllOutline();
        }

        private void OnPointerDown(PointerEventData eventData)
        {
            if (_entityBehaviour.Entity.isEnemyHexagon)
                return;

            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            
            _uiFactory.ShowInfoPanel(_entityBehaviour);
            _outlinesController.DeactivateAllOutline();
            _outline.SetActive(true);
        }
    }
}