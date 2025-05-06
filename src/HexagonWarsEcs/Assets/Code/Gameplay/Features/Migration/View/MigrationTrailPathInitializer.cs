using System;
using System.Collections.Generic;
using Code.Infrastructure.View;
using UnityEngine;
using UnityEngine.Splines;

namespace Code.Gameplay.Features.Migration.View
{
    public class MigrationTrailPathInitializer : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;
        [SerializeField] private Transform _migrationTrailPrefab;
        [SerializeField] private SplineContainer _splineContainer;
        private float _distancePercentage;
        private float _distancePercentageMain;
        private float _speed;
        private Transform _fillingFragment;
        private Transform _mainTrail;
        private float[] _complexity;

        private void Update()
        {
            if (!_entityBehaviour.Entity.hasWayIdPoints)
                return;

            if (_splineContainer.Spline.Count == 0)
            {
                _distancePercentageMain = 0;
                _distancePercentage = 0;
                CreateSpline();
                _fillingFragment = Instantiate(_migrationTrailPrefab, transform);
                TrailRenderer trailRenderer = _fillingFragment.GetComponent<TrailRenderer>();
                trailRenderer.sortingOrder = -1;
                _mainTrail = Instantiate(_migrationTrailPrefab, transform);
                TrailRenderer mainTrailRenderer = _mainTrail.GetComponent<TrailRenderer>();
                mainTrailRenderer.startColor = Color.red;
                mainTrailRenderer.endColor = Color.red;
                mainTrailRenderer.sortingOrder = -2;
            }
            
            MoveMainTrail();
            MoveFillingTrail();
        }
        
        private void CreateSpline()
        {
            GameContext game = Contexts.sharedInstance.game;

            List<Vector3> path = new();

            foreach (int id in _entityBehaviour.Entity.wayIdPoints.Value)
            {
                Vector3 position = game.GetEntityWithId(id).transform.Value.position + Vector3.up * 0.34f;
                path.Add(position);
            }
            
            Vector3[] array = path.ToArray();

            foreach (Vector3 vector3 in array) 
                _splineContainer.Spline.Add(vector3);

            _complexity = _entityBehaviour.Entity.migrationComplexityWay.Value.ToArray();
        }

        private void MoveMainTrail()
        {
            if (_distancePercentageMain >= 1f)
                return;
            
            _distancePercentageMain += 2f * Time.deltaTime;
            Vector3 currentPosition = _splineContainer.Spline.EvaluatePosition(_distancePercentageMain);
            _mainTrail.transform.position = currentPosition;
        }

        private void MoveFillingTrail()
        {
            if (_distancePercentage >= 1f)
                return;
            
            int index = (int)Math.Floor(_distancePercentage * _complexity.Length);
            _speed = 1f / _complexity[index] / _complexity.Length;
            _distancePercentage += _speed * Time.deltaTime;
            Vector3 currentPosition = _splineContainer.Spline.EvaluatePosition(_distancePercentage);
            _fillingFragment.transform.position = currentPosition;
        }
    }
}
