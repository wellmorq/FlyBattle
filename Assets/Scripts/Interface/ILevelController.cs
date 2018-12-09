namespace FlyBattle.Interface
{
    public interface ILevelController
    {
        /// <summary>
        /// Завершает сессию контролёра уровня
        /// </summary>
        void Clear();

        /// <summary>
        /// Сброс состояния контролёра уровня
        /// </summary>
        void Reset();

        bool CheckReady(GameState state);
    }
}