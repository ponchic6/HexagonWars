using Code.Gameplay.Common.Services;
using Code.Gameplay.Common.View;
using Code.Infrastructure.View;
using Logic.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.Gameplay.Features.Building.View
{
    public class HexagonInfoPanelHandler : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private PointerHandler _pointerHandler;
        private IUIFactory _uiFactory;

        [Inject]
        public void Construct(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        private void Awake() =>
            _pointerHandler.OnPointerDownEvent += OnPointerDown;

        private void OnDisable() =>
            _pointerHandler.OnPointerDownEvent -= OnPointerDown;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _uiFactory.HideInfoPanel(_entityBehaviour);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_entityBehaviour.Entity.isEnemyHexagon)
                return;
            
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _uiFactory.ShowInfoPanel(_entityBehaviour);
            }
        }
    }
}