using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class PlayerData
    {
        public static PlayerData Instance { get; } = new PlayerData();

        public int PlayerId { get; }
        public string PlayerName
        {
            get
            {
                return PlayerName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    PlayerName = value;
                }
            }
        }
        public byte HealthCount { get; set; }
        public byte PlayerTurn { get; set; }
        public int RoomId { get; }
        public string Wordkey { get; set; }
        public byte PlayerNumber { get; set; }

        private PlayerData() { }
    }
}
