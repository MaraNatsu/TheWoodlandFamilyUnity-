using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Player
    {
        public static Player Instance { get; } = new Player();
        public string Nickname { get; set; }
        private int Id { get; }
        private byte HealthCount { get; set; }
        private byte Turn { get; set; }
        private int RoomId { get; }
        private string Wordkey { get; set; }
        private byte PlayerNumber { get; set; }

        private Player() { }
    }
}
