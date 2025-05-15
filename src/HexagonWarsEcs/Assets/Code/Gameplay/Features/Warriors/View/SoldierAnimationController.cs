using UnityEngine;

namespace Code.Gameplay.Features.Warriors.View
{
    public class SoldierAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Shooting = Animator.StringToHash("Shooting");
        
        public Animator Animator => _animator;

        public void StartRun() =>
            _animator.SetTrigger(Run);

        public void StartIdle() =>
            _animator.SetTrigger(Idle);

        public void StartShooting() =>
            _animator.SetTrigger(Shooting);
    }
} 