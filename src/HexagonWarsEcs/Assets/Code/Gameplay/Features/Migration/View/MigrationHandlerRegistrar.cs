using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Gameplay.Features.Migration.View
{
    public class MigrationHandlerRegistrar : EntityComponentRegistrar
    {
        [SerializeField] private MigrationHandler _migrationHandler;
        
        public override void RegisterComponent() =>
            Entity.AddMigrationHandler(_migrationHandler);

        public override void UnregisterComponent() =>
            Entity.RemoveMigrationHandler();
    }
}