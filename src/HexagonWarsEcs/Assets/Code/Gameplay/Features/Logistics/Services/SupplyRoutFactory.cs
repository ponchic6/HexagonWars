using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Features.Logistics.View;
using Code.Infrastructure.Services;
using Logic.Logistic;
using UnityEngine;

namespace Code.Gameplay.Features.Logistics.Services
{
    public class SupplyRoutFactory : ISupplyRoutFactory
    {
        private readonly ISupplyArrowFactory _supplyArrowFactory;
        private readonly IIdentifierService _identifierService;
        
        private List<LogisticNode> _currentNodes = new();

        public SupplyRoutFactory(ISupplyArrowFactory supplyArrowFactory, IIdentifierService identifierService)
        {
            _supplyArrowFactory = supplyArrowFactory;
            _identifierService = identifierService;
        }

        public void StartCreateRoute(LogisticNode logisticNode)
        {
            _supplyArrowFactory.AddPoint(logisticNode.transform.position, Color.green);
            _currentNodes.Add(logisticNode);
        }

        public bool TryAjustLogicNode(LogisticNode logisticNode)
        {
            if (_currentNodes.Count == 0)
                return false;

            Color supplyColor;

            if (_currentNodes.Contains(logisticNode))
            {
                if (_currentNodes.Count > 1 && logisticNode == _currentNodes[^2])
                {
                    _currentNodes.RemoveAt(_currentNodes.Count - 1);
                    supplyColor = PaintSupplyRout(logisticNode);
                    _supplyArrowFactory.RemoveLastPoint(supplyColor);
                    return true;
                }
                return false;
            }
            
            supplyColor = PaintSupplyRout(logisticNode);
            _supplyArrowFactory.AddPoint(logisticNode.transform.position, supplyColor);
            _currentNodes.Add(logisticNode);
            return true;
        }

        public bool TryFinishOfCreatingRoute()
        {
            if (_currentNodes.Count <= 1 ||
                _currentNodes.Any(x => x == !x.EntityBehaviour.Entity.isAvailabilityForSupplyRout))
            {
                DestroySupplyRoute();
                return false;
            }

            GameEntity entity = _supplyArrowFactory.CreateArrow();
            entity.AddId(_identifierService.Next());
            entity.AddCouriersProgressList(new ());
            entity.AddWayIdPoints(_currentNodes.Select(x => x.EntityBehaviour.Entity.id.Value).ToList());
            entity.AddSupplyComplexityWay(10);
            entity.isSupplyRoute = true;
            _currentNodes.Clear();
            return true;
        }

        private void DestroySupplyRoute()
        {
            _currentNodes.Clear();
            _supplyArrowFactory.DestroyCurrentArrow();
        }

        private Color PaintSupplyRout(LogisticNode logisticNode)
        {
            Color supplyColor = Color.green;
            
            if (!logisticNode.EntityBehaviour.Entity.isAvailabilityForSupplyRout)
                supplyColor = Color.red;
            
            foreach (LogisticNode node in _currentNodes)
            {
                if (!node.EntityBehaviour.Entity.isAvailabilityForSupplyRout)
                {
                    supplyColor = Color.red;
                    break;
                }
            }

            return supplyColor;
        }
    }
}