using Code.Gameplay.Common.View.UI;
using Code.Infrastructure.View;
using Logic.Logistic;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Features.Logistics.Services
{
    public class SupplyArrowFactory : ISupplyArrowFactory
    {
        private const string PATH_TO_SUPPLY_ARROW = "Arrows/SupplyArrow/SupplyArrow";
        
        private readonly DiContainer _diContainer;
        private LineRenderer _currentLine;
        private Material _lineMaterial;

        public SupplyArrowFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public LineRenderer AddPoint(Vector3 position)
        {
            if (_currentLine == null)
            {
                _currentLine = _diContainer.InstantiatePrefabResourceForComponent<LineRenderer>(PATH_TO_SUPPLY_ARROW);
                _lineMaterial = _currentLine.material;
            }

            position.y = 0.5f;
            _currentLine.positionCount += 1;
            _currentLine.SetPosition(_currentLine.positionCount - 1, position);
            UpdateTiling();
            return _currentLine;
        }

        public void RemoveLastPoint()
        {
            if (_currentLine == null)
                return;
            
            int positionCount = _currentLine.positionCount;
            
            Vector3[] newPositions = new Vector3[positionCount - 1];
            
            for (int i = 0; i < newPositions.Length; i++)
            {
                newPositions[i] = _currentLine.GetPosition(i);
            }
            
            _currentLine.positionCount = newPositions.Length;
            _currentLine.SetPositions(newPositions);
            UpdateTiling();
        }

        public GameEntity CreateArrow()
        {
            GameContext game = Contexts.sharedInstance.game;
            GameEntity entity = game.CreateEntity();
            EntityBehaviour entityBehaviour = _currentLine.gameObject.GetComponent<EntityBehaviour>();
            entityBehaviour.SetEntity(entity);
            UpdateTiling();
            _currentLine.gameObject.GetComponent<LineRendererCollider>().UpdateMeshCollider();
            _lineMaterial = null;
            _currentLine = null;
            return entity;
        }
        
        public void DestroyCurrentArrow()
        {
            if (_currentLine == null)
                return;
            
            Object.Destroy(_currentLine.gameObject);
            _lineMaterial = null;
            _currentLine = null;
        }

        private void UpdateTiling()
        {
            if (_currentLine == null || _lineMaterial == null) 
                return;
            
            float lineLength = CalculateLineLength();
            
            float tiling = lineLength / 0.08f;
            _lineMaterial.SetVector("_Tiling", new Vector4(tiling, 0));
        }

        private float CalculateLineLength()
        {
            if (_currentLine.positionCount < 2) return 0;

            float length = 0;
            for (int i = 1; i < _currentLine.positionCount; i++)
            {
                length += Vector3.Distance(_currentLine.GetPosition(i - 1), _currentLine.GetPosition(i));
            }
            return length;
        }
    }
}