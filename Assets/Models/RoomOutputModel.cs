using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SignalRModels
{
    class RoomOutputModel
    {
        public int RoomId { get; set; }
        public byte PlayerNumber { get; set; }
        public string Wordkey { get; set; }
    }
}
