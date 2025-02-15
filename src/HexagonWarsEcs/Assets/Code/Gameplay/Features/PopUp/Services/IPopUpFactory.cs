using UnityEngine;

namespace Code.Gameplay.Features.PopUp.Services
{
    public interface IPopUpFactory
    {
        public void CreatePopUp(Sprite sprite, float count, Transform transform);
    }
}