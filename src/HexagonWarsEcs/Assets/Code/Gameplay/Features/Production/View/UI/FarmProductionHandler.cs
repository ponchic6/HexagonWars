using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.Features.Production.View.UI
{
    public class FarmProductionHandler : ProductionHandler
    {
        [SerializeField] private TMP_Text _workersAmount;
        [SerializeField] private Button _plusButton;
        [SerializeField] private Button _minusButton;
        
        public override void Setup(GameEntity hexEntity)
        {
            _workersAmount.text = hexEntity.foodFarm.Workers.ToString();
            
            _plusButton.onClick.AsObservable().Subscribe(_ =>
            {
                if (hexEntity.manAmount.Value <= 0)
                    return;
                
                hexEntity.ReplaceManAmount(hexEntity.manAmount.Value - 1);
                hexEntity.foodFarm.Workers++;
                _workersAmount.text = hexEntity.foodFarm.Workers.ToString();
            }).AddTo(this);
            
            _minusButton.onClick.AsObservable().Subscribe(_ =>
            {
                if (hexEntity.foodFarm.Workers <= 0)
                    return;
                
                hexEntity.ReplaceManAmount(hexEntity.manAmount.Value + 1);
                hexEntity.foodFarm.Workers--;
                _workersAmount.text = hexEntity.foodFarm.Workers.ToString();
            }).AddTo(this);
        }

        public override void UpdateProductionUi()
        {
        }
    }
}