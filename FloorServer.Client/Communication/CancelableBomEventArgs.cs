using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FloorServer.Client.Boss;

namespace FloorServer.Client.Communication
{
    public class CancelableBomEventArgs : EventArgs
    {
        public CancelableBomEventArgs(Bom message)
        {
            Message = message;
            Cancel = false;
        }

        public Bom Message { get; private set; }
        public bool Cancel { get; set; }
        
    }
}
