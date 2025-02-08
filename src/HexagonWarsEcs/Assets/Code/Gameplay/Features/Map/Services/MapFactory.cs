using Code.Infrastructure.StaticData;

namespace Code.Gameplay.Features.Map.Services
{
    public class MapFactory : IMapFactory
    {
        private readonly CommonStaticData _commonStaticData;

        public MapFactory(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
        }
        
        public GameEntity CreateMap()
        {
            var game = Contexts.sharedInstance.game;
            GameEntity map = game.CreateEntity();
            map.AddViewPrefab(_commonStaticData.Map);
            return map;
        }
    }
}