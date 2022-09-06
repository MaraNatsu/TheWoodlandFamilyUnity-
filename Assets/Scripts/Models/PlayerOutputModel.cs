using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class PlayerOutputModel
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public byte Turn { get; set; }
        public byte HealthCount { get; set; }
    }
}
