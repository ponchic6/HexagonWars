//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Code.Gameplay.Features.Citizens.CitizensAmount citizensAmount { get { return (Code.Gameplay.Features.Citizens.CitizensAmount)GetComponent(GameComponentsLookup.CitizensAmount); } }
    public bool hasCitizensAmount { get { return HasComponent(GameComponentsLookup.CitizensAmount); } }

    public void AddCitizensAmount(int newValue) {
        var index = GameComponentsLookup.CitizensAmount;
        var component = (Code.Gameplay.Features.Citizens.CitizensAmount)CreateComponent(index, typeof(Code.Gameplay.Features.Citizens.CitizensAmount));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceCitizensAmount(int newValue) {
        var index = GameComponentsLookup.CitizensAmount;
        var component = (Code.Gameplay.Features.Citizens.CitizensAmount)CreateComponent(index, typeof(Code.Gameplay.Features.Citizens.CitizensAmount));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveCitizensAmount() {
        RemoveComponent(GameComponentsLookup.CitizensAmount);
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

    static Entitas.IMatcher<GameEntity> _matcherCitizensAmount;

    public static Entitas.IMatcher<GameEntity> CitizensAmount {
        get {
            if (_matcherCitizensAmount == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CitizensAmount);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCitizensAmount = matcher;
            }

            return _matcherCitizensAmount;
        }
    }
}
