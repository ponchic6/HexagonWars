using Entitas;

namespace Code.Infrastructure.View
{
    [Game] public class View : IComponent { public EntityBehaviour Value;}
    [Game] public class ViewPath : IComponent { public string Value; }
    [Game] public class ViewPrefab : IComponent { public EntityBehaviour Value; }
}