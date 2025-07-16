using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerTester
{
    public class CardMessage
    {
        public int CasinoId { get; set; }

        public int MachineId { get; set; }

        public CardMessageType CardMessageType { get; set; }

        public long CardNum { get; set; }
    }

    public enum CardMessageType
    {
        cardin,
        cardout
    }
}
