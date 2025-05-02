using Code.Gameplay.Common.View;
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
        private ISupplyRoutFactory _supplyRoutFactory;
        
        public EntityBehaviour EntityBehaviour => _entityBehaviour;

        [Inject]
        public void Construct(ISupplyRoutFactory supplyRoutFactory)
        {
            _supplyRoutFactory = supplyRoutFactory;
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
            if (_entityBehaviour.Entity.isEnemyHexagon)
                return;
            
            if (pointerEventData.button != PointerEventData.InputButton.Left)
                return;

            _supplyRoutFactory.StartCreateRoute(this);
        }

        private void OnPointerHandlerEnterEvent(PointerEventData pointerEventData) => 
            _supplyRoutFactory.TryAjustLogicNode(this);

        private void OnPointerHandlerUpEvent(PointerEventData pointerEventData) =>
            _supplyRoutFactory.TryFinishOfCreatingRoute();
    }
}