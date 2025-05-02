using UnityEngine;

namespace Logic.Logistic
{
    public interface ISupplyArrowFactory
    {
        LineRenderer AddPoint(Vector3 transformPosition, Color color);
        void RemoveLastPoint(Color color);
        void DestroyCurrentArrow();
        GameEntity CreateArrow();
    }
}