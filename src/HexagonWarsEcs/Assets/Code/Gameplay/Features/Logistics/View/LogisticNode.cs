using Code.Gameplay.Features.Logistics.Services;
using Code.Infrastructure.View;
using Logic.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.Gameplay.Features.Logistics.View
{
    public class LogisticNode : MonoBehaviour
    {
        [SerializeField] private PointerHandler pointerHandler;
        [SerializeField] private EntityBehaviour _entityBehaviour;
        private ISupplyRouteFactory _supplyRouteFactory;
        
        public EntityBehaviour EntityBehaviour => _entityBehaviour;

        [Inject]
        public void Construct(ISupplyRouteFactory supplyRouteFactory)
        {
            _supplyRouteFactory = supplyRouteFactory;
        }

        private void Awake()
        {
            pointerHandler.OnPointerDownEvent += OnPointerHandlerDownEvent;
            pointerHandler.OnPointerUpEvent += OnPointerHandlerUpEvent;
            pointerHandler.OnPointerEnterEvent += OnPointerHandlerEnterEvent;
        }

        private void OnDisable()
        {
            pointerHandler.OnPointerDownEvent -= OnPointerHandlerDownEvent;
            pointerHandler.OnPointerUpEvent -= OnPointerHandlerUpEvent;
            pointerHandler.OnPointerEnterEvent -= OnPointerHandlerEnterEvent;
        }

        private void OnPointerHandlerDownEvent(PointerEventData pointerEventData)
        {
            if (pointerEventData.button != PointerEventData.InputButton.Left)
                return;

            _supplyRouteFactory.StartCreateRoute(this);
        }

        private void OnPointerHandlerEnterEvent(PointerEventData pointerEventData)
        {
            _supplyRouteFactory.TryAjustLogicNode(this);
        }

        private void OnPointerHandlerUpEvent(PointerEventData pointerEventData)
        {
            _supplyRouteFactory.TryFinishOfCreatingRoute(this);
        }
    }
}