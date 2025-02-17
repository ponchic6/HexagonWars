using Code.Gameplay.Features.PopUp.DataStructures;
using Code.Gameplay.Features.PopUp.Services;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.PopUp.Systems
{
    public class ResourcesAmountPopUpSystem : IExecuteSystem
    {
        private readonly IPopUpFactory _popUpFactory;
        private readonly IGroup<GameEntity> _entities;

        public ResourcesAmountPopUpSystem(IPopUpFactory popUpFactory)
        {
            _popUpFactory = popUpFactory;
            
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.AllOf
                (GameMatcher.AmountPopUpEvents,
                    GameMatcher.AmountPopUpCooldown,
                    GameMatcher.Transform));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.amountPopUpCooldown.RemainingTime == 0)
                {
                    foreach (AmountPopUpContainer amountPopUpContainer in entity.amountPopUpEvents.Value) 
                        _popUpFactory.CreatePopUp(amountPopUpContainer.sprite, amountPopUpContainer.count, amountPopUpContainer.hexTransform);
                }
            }
        }
    }
}