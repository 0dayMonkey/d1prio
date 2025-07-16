using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloorServer.Client.Commands
{
    public class PeriodicReadingCommand : FSCommand
    {
        public string ReadName { get; set; }
        public ulong ReadID { get; set; }
        public ulong ReadRepeat { get; set; }
        public TimeSpan ReadOffset { get; set; }
        public bool SendTerminatorException { get; set; }
        public List<string> ReadedFields { get; set; }
    }
}
