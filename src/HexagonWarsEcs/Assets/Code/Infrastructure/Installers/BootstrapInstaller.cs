using Code.Gameplay.Common.Services;
using Code.Gameplay.Features.Logistics.Services;
using Code.Gameplay.Features.Map;
using Code.Gameplay.Features.Map.Services;
using Code.Gameplay.Features.Map.View;
using Code.Gameplay.Features.Migration.Services;
using Code.Infrastructure.Services;
using Code.Infrastructure.StaticData;
using Code.Infrastructure.Systems;
using Code.Infrastructure.View;
using Logic.Logistic;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private CommonStaticData _commonStaticData;
        
        public override void InstallBindings()
        {
            Container.Bind<CommonStaticData>().FromInstance(_commonStaticData).AsSingle();
            Container.Bind<IIdentifierService>().To<IdentifierService>().AsSingle();
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
            Container.Bind<ISupplyArrowFactory>().To<SupplyArrowFactory>().AsSingle();
            Container.Bind<ISupplyRouteFactory>().To<SupplyRouteFactory>().AsSingle();
            Container.Bind<ISystemFactory>().To<SystemFactory>().AsSingle();
            Container.Bind<IEntityViewFactory>().To<EntityViewFactory>().AsSingle();
            Container.Bind<IMapFactory>().To<MapFactory>().AsSingle();
            Container.Bind<IMigrationFactory>().To<MigrationFactory>().AsSingle();
        }
    }
}