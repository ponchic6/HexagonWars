//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly Code.Gameplay.Features.Production.LivingArea livingAreaComponent = new Code.Gameplay.Features.Production.LivingArea();

    public bool isLivingArea {
        get { return HasComponent(GameComponentsLookup.LivingArea); }
        set {
            if (value != isLivingArea) {
                var index = GameComponentsLookup.LivingArea;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : livingAreaComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherLivingArea;

    public static Entitas.IMatcher<GameEntity> LivingArea {
        get {
            if (_matcherLivingArea == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.LivingArea);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherLivingArea = matcher;
            }

            return _matcherLivingArea;
        }
    }
}
