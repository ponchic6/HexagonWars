using Code.Infrastructure.Services;
using Code.Infrastructure.StaticData;

namespace Code.Gameplay.Features.Map.Services
{
    public class MapFactory : IMapFactory
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IIdentifierService _identifierService;

        public MapFactory(CommonStaticData commonStaticData, IIdentifierService identifierService)
        {
            _commonStaticData = commonStaticData;
            _identifierService = identifierService;
        }
        
        public GameEntity CreateMap()
        {
            var game = Contexts.sharedInstance.game;
            GameEntity map = game.CreateEntity();
            map.AddViewPrefab(_commonStaticData.Map);
            map.AddId(_identifierService.Next());
            return map;
        }
    }
}