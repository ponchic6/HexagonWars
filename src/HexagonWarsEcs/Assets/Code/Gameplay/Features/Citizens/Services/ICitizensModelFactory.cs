namespace Code.Gameplay.Features.Citizens.Services
{
    public interface ICitizensModelFactory
    {
        void TryCreateIdleCitizen(int idHex);
        void TryRemoveIdleCitizen(int idHex);
    }
}