using UnityEngine;

namespace Logic.Logistic
{
    public interface ISupplyArrowFactory
    {
        LineRenderer AddPoint(Vector3 transformPosition);
        void RemoveLastPoint();
        void DestroyCurrentArrow();
        GameEntity CreateArrow();
    }
}