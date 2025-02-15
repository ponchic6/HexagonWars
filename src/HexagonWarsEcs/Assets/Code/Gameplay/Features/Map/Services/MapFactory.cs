using Code.Gameplay.Features.Building;
using Code.Gameplay.Features.Building.DataStructure;
using Code.Gameplay.Features.Migration.View.UI;
using Code.Infrastructure.Services;
using Code.Infrastructure.StaticData;
using UnityEngine;

namespace Code.Gameplay.Features.Map.Services
{
    public class MapFactory : IMapFactory
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IIdentifierService _identifierService;
        
        private readonly float hexSize = 0.45f;
        private int _mapWidth = 4;
        private int _mapHeight = 4;
        private GameContext _game;

        public MapFactory(CommonStaticData commonStaticData, IIdentifierService identifierService)
        {
            _commonStaticData = commonStaticData;
            _identifierService = identifierService;
            _game = Contexts.sharedInstance.game;
        }
        
        public void CreateMap() =>
            GenerateHexMap();

        private void GenerateHexMap()
        {
            GameObject map = new GameObject("Map");
            map.AddComponent<MigrationAllSlidersBlocker>();
            
            float hexWidth = hexSize * 2f;
            float hexHeight = Mathf.Sqrt(3f) * hexSize;
            float offsetX = hexWidth * 0.75f;
            float offsetY = hexHeight;

            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    GameEntity hexagonEntity = CreateHexagonEntity();
                    float xPos = x * offsetX;
                    float yPos = y * offsetY;
                    
                    if (x % 2 == 1)
                    {
                        yPos += hexHeight / 2f;
                    }

                    hexagonEntity.AddInitialTransform(new Vector3(xPos, 0, yPos), Quaternion.Euler(new Vector3(-90, 0, 0)));
                    hexagonEntity.AddViewPrefabWithParent(_commonStaticData.Hexagon, map);
                }
            }
        }

        private GameEntity CreateHexagonEntity()
        {
            GameEntity entity = _game.CreateEntity();
            entity.isChildHexagon = true;
            entity.AddId(_identifierService.Next());

            if (Random.value > 0.5f)
            {
                entity.isPlayerHexagon = true;
            }
            else
            {
                entity.isEnemyHexagon = true;
            }
            
            entity.AddCitizensAmount(new System.Random().Next(1, 101));
            entity.AddWarriorsAmount(new System.Random().Next(1, 101));
            entity.AddFoodAmount(100000);
            
            entity.AddBuildingProgress(new ()
            {
                new BuildProgressContainer
                {
                    fullProgress = 200,
                    currentProgress = 0,
                    buildingType = BuildingsType.FoodFarm,
                    buildersAmount = 0,
                    ready = false
                }
            });

            return entity;
        }
    }
}