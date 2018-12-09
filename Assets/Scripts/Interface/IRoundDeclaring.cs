namespace FlyBattle.Interface
{
    public interface IRoundDeclaring
    {
        /// <summary>
        /// Stage of the UI element
        /// </summary>
        /// <returns>
        /// -1 : not started
        ///  0 : started,
        ///  1 : has already been done
        /// </returns>
        int GetStatus();
    }
}