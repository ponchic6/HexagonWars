using System.Collections.Generic;
using Code.Infrastructure.View;
using Logic.Logistic;
using UnityEngine;
using Zenject;

namespace Code.Gameplay.Features.Logistics.Services
{
    public class SupplyArrowFactory : ISupplyArrowFactory
    {
        private const string PATH_TO_SUPPLY_ARROW = "Arrows/SupplyArrow/SupplyArrow";

        private LineRenderer _currentLine;
        private Material _lineMaterial;
        private GameEntity _currentEntity;
        private List<Vector3> _positions;
        
        public void AddPoint(Vector3 position, Color color)
        {
            if (_currentEntity == null)
            {
                GameContext game = Contexts.sharedInstance.game;
                GameEntity entity = game.CreateEntity();
                entity.AddViewPath(PATH_TO_SUPPLY_ARROW);
                _currentEntity = entity;
                _positions = new();
                position.y = 0.5f;
                _positions.Add(position);
                return;
            }

            if (_currentEntity.hasView && _currentLine == null)
            {
                _currentLine = _currentEntity.view.Value.GetComponent<LineRenderer>();
                _lineMaterial = _currentLine.material;
            }

            position.y = 0.5f;
            _positions.Add(position);
            _currentLine.positionCount = _positions.Count;
            
            for (var i = 0; i < _positions.Count; i++)
            {
                Vector3 currentPosition = _positions[i];
                _currentLine.SetPosition(i, currentPosition);
            }
            
            _lineMaterial.SetVector("_Color", color);
            UpdateTiling();
        }

        public void RemoveLastPoint(Color color)
        {
            if (_currentLine == null)
                return;
            
            _positions.RemoveAt(_positions.Count - 1);
            _currentLine.positionCount = _positions.Count;
            _currentLine.SetPositions(_positions.ToArray());
            _lineMaterial.SetVector("_Color", color);
            UpdateTiling();
        }

        public GameEntity CreateArrow()
        {
            GameEntity entity = _currentEntity;
            _currentEntity = null;
            
            _positions.Clear();
            _lineMaterial = null;
            _currentLine = null;
            UpdateTiling();
            return entity;
        }
        
        public void DestroyCurrentArrow()
        {
            if (_currentEntity == null)
                return;
            
            _currentEntity.isDestructed = true;
            _currentEntity = null;
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