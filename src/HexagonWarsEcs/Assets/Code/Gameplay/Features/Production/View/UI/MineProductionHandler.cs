using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.Features.Production.View.UI
{
    public class MineProductionHandler : ProductionHandler
    {
        [SerializeField] private TMP_Text _minersAmount;
        [SerializeField] private Button _plusButton;
        [SerializeField] private Button _minusButton;
        [SerializeField] private Button _ironButton;
        [SerializeField] private Button _coalButton;
        
        public override void Setup(GameEntity hexEntity)
        {
            _minersAmount.text = hexEntity.mine.Miners.ToString();
            _ironButton.onClick.AsObservable().Subscribe(_ => hexEntity.mine.OreType = OreType.Iron).AddTo(this);
            _coalButton.onClick.AsObservable().Subscribe(_ => hexEntity.mine.OreType = OreType.Coal).AddTo(this);
            
            _plusButton.onClick.AsObservable().Subscribe(_ =>
            {
                if (hexEntity.citizensAmount.Value <= 0)
                    return;
                
                hexEntity.ReplaceCitizensAmount(hexEntity.citizensAmount.Value - 1);
                hexEntity.mine.Miners++;
                _minersAmount.text = hexEntity.mine.Miners.ToString();
            }).AddTo(this);
            
            _minusButton.onClick.AsObservable().Subscribe(_ =>
            {
                if (hexEntity.mine.Miners <= 0)
                    return;
                
                hexEntity.ReplaceCitizensAmount(hexEntity.citizensAmount.Value + 1);
                hexEntity.mine.Miners--;
                _minersAmount.text = hexEntity.mine.Miners.ToString();
            }).AddTo(this);

        }

        public override void UpdateProductionUi()
        {
        }
    }
}