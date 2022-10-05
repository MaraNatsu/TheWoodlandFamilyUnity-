using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class TelegramManager : MonoBehaviour
    {
        public void Share()
        {
            Application.OpenURL($"tg://msg_url?url={Player.Instance.Nickname} invites you to join a round. " +
                $"CLick to copy your wordkey:&text=\"`{Player.Instance.Wordkey}\"`");
        }
    }
}
