using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Features.Logistics.View;
using Logic.Logistic;

namespace Code.Gameplay.Features.Logistics.Services
{
    public class SupplyRouteFactory : ISupplyRouteFactory
    {
        private readonly ISupplyArrowFactory _supplyArrowFactory;
        private List<LogisticNode> _currentNodes = new();

        public SupplyRouteFactory(ISupplyArrowFactory supplyArrowFactory)
        {
            _supplyArrowFactory = supplyArrowFactory;
        }

        public void StartCreateRoute(LogisticNode logisticNode)
        {
            _supplyArrowFactory.AddPoint(logisticNode.transform.position);
            _currentNodes.Add(logisticNode);
        }

        public bool TryAjustLogicNode(LogisticNode logisticNode)
        {
            if (_currentNodes.Count == 0)
                return false;

            if (_currentNodes.Count >= 2)
            {
                if (logisticNode == _currentNodes[^2])
                {
                    _supplyArrowFactory.RemoveLastPoint();
                    _currentNodes.RemoveRange(_currentNodes.Count - 1, 1);
                    return true;
                }
            }
            
            _supplyArrowFactory.AddPoint(logisticNode.transform.position);
            _currentNodes.Add(logisticNode);
            return true;
        }

        public bool TryFinishOfCreatingRoute(LogisticNode logisticNode)
        {
            if (_currentNodes.Count <= 1)
            {
                _currentNodes.Clear();
                _supplyArrowFactory.DestroyCurrentArrow();
                return false;
            }
            
            GameEntity entity = _supplyArrowFactory.CreateArrow();
            entity.AddCouriersAmount(0);
            entity.AddWayIdPoints(_currentNodes.Select(x => x.EntityBehaviour.Entity.id.Value).ToList());
            entity.AddCurrentSupplyComplexityWay(0);
            entity.AddMaxSupplyComplexityWay(20);
            entity.isSupplyRoute = true;
            _currentNodes.Clear();
            return true;
        }

        public void DestroyRoute()
        {
            //_supplyArrowFactory.DestroyArrow(supplyRoute);
        }


        private void BackButtonDown() =>
            StopCreatingSupplyRoute();

        private void StopCreatingSupplyRoute() =>
            _currentNodes.Clear();
    }
}