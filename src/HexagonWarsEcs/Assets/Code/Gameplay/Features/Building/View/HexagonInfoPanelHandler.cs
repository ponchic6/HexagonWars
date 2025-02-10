using Code.Gameplay.Common.Services;
using Code.Infrastructure.View;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.Gameplay.Features.Building.View
{
    public class HexagonInfoPanelHandler : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        private IUIFactory _uiFactory;

        [Inject]
        public void Construct(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _uiFactory.HideInfoPanel(_entityBehaviour);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _uiFactory.ShowInfoPanel(_entityBehaviour);
            }
        }
    }
}