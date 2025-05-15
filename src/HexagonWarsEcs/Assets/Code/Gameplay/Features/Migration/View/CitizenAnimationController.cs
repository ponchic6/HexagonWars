using UnityEngine;

namespace Code.Gameplay.Features.Migration.View
{
    public class CitizenAnimationController : MonoBehaviour
    {
        private readonly int _run = UnityEngine.Animator.StringToHash("Run");
        private readonly int _idle = UnityEngine.Animator.StringToHash("Idle");
        
        [SerializeField] private Animator _animator;
        
        public Animator Animator => _animator;
        
        public void StartRun() =>
            _animator.SetTrigger(_run);

        public void StartIdle() =>
            _animator.SetTrigger(_idle);
    }
}
