namespace FlyBattle.Interface
{
    /// <summary>
    /// Designed to receive a commands from the InputController
    /// </summary>
    public interface IControllerReceiver
    {
        void Shoot();
        void Move(float h_Input);
    }
}