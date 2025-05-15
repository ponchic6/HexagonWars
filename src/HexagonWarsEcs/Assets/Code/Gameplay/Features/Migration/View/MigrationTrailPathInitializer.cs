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
        private GameContext _game = Contexts.sharedInstance.game;
        private float _distancePercentage;
        private float _distancePercentageMain;
        private float _speed;
        private Transform _fillingFragment;
        private Transform _mainTrail;
        private float[] _complexity;

        private void Update()
        {
            GameEntity migrationEntity = _game.GetEntityWithId(_entityBehaviour.Entity.migrationArrow.MigrationId);
            
            if (migrationEntity == null)
                return;
            
            if (!migrationEntity.hasWayIdPoints)
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

            TryDestroy();
            MoveMainTrail();
            MoveFillingTrail();
        }

        private void CreateSpline()
        {
            GameContext game = _game;

            List<Vector3> path = new();

            GameEntity migrationEntity = _game.GetEntityWithId(_entityBehaviour.Entity.migrationArrow.MigrationId);
            
            foreach (int id in migrationEntity.wayIdPoints.Value)
            {
                Vector3 position = game.GetEntityWithId(id).transform.Value.position + Vector3.up * 0.34f;
                path.Add(position);
            }
            
            Vector3[] array = path.ToArray();

            foreach (Vector3 vector3 in array) 
                _splineContainer.Spline.Add(vector3);

            _complexity = migrationEntity.migrationComplexityWay.Value.ToArray();
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

        private void TryDestroy()
        {
            if ((int)Math.Floor(_distancePercentage * _complexity.Length) != 0) 
                _entityBehaviour.Entity.isDestructed = true;
        }
    }
}
