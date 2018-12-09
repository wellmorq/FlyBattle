namespace FlyBattle.Controllers
{
    public class AIInputController: InputController
    {
        protected override void CancelCheck()
        {
            
        }

        protected override void MoveCheck()
        {
            Receiver?.Move(0);
        }

        protected override void FireCheck()
        {
            Receiver?.Shoot();
        }
    }
}