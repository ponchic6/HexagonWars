using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Features.Logistics.View;
using Code.Infrastructure.Services;
using Logic.Logistic;

namespace Code.Gameplay.Features.Logistics.Services
{
    public class SupplyRouteFactory : ISupplyRouteFactory
    {
        private readonly ISupplyArrowFactory _supplyArrowFactory;
        private readonly IIdentifierService _identifierService;
        
        private List<LogisticNode> _currentNodes = new();

        public SupplyRouteFactory(ISupplyArrowFactory supplyArrowFactory, IIdentifierService identifierService)
        {
            _supplyArrowFactory = supplyArrowFactory;
            _identifierService = identifierService;
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
            entity.AddId(_identifierService.Next());
            entity.AddCouriersProgressList(new ());
            entity.AddWayIdPoints(_currentNodes.Select(x => x.EntityBehaviour.Entity.id.Value).ToList());
            entity.AddMaxSupplyComplexityWay(10);
            entity.isSupplyRoute = true;
            _currentNodes.Clear();
            return true;
        }
        

        private void BackButtonDown() =>
            StopCreatingSupplyRoute();

        private void StopCreatingSupplyRoute() =>
            _currentNodes.Clear();
    }
}