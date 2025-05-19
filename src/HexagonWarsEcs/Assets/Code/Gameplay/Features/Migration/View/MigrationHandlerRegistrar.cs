using Code.Infrastructure.View;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Gameplay.Features.Migration.View
{
    public class MigrationHandlerRegistrar : EntityComponentRegistrar
    {
        [FormerlySerializedAs("_migrationHandler")] [SerializeField] private MigrationStartHexHandler migrationStartHexHandler;
        
        public override void RegisterComponent() =>
            Entity.AddMigrationHandler(migrationStartHexHandler);

        public override void UnregisterComponent() =>
            Entity.RemoveMigrationHandler();
    }
}