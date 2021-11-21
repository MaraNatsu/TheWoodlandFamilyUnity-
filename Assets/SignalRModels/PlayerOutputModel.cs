using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SignalRModels
{
    public class PlayerOutputModel
    {
        public int RoomId { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public byte HealthCount { get; set; }
        public byte PlayerTurn { get; set; }
    }
}
