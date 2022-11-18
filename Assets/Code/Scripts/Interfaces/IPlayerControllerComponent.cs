using Code.Player;

namespace Code.Interfaces
{
    public interface IPlayerControllerComponent
    {
        PlayerController playerController { get; set; }
        void SetPlayerController(PlayerController playerController)
        {
            this.playerController = playerController;
        }
    }
}