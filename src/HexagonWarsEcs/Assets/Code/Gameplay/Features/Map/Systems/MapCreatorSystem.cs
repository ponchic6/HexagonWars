using Code.Gameplay.Features.Map.Services;
using Entitas;

namespace Code.Gameplay.Features.Map.Systems
{
    public class MapCreatorSystem : IInitializeSystem
    {
        private readonly IMapFactory _mapFactory;

        public MapCreatorSystem(IMapFactory mapFactory) =>
            _mapFactory = mapFactory;

        public void Initialize() =>
            _mapFactory.CreateMap();
    }
}