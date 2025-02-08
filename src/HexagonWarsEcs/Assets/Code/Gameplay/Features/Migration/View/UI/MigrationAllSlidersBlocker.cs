using UniRx;
using UnityEngine;

namespace Code.Gameplay.Features.Migration.View.UI
{
    public class MigrationAllSlidersBlocker : MonoBehaviour
    {
        public ReactiveProperty<bool> isSlidersBlocked = new();
    }
}