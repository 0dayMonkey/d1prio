namespace FloorServer.Client.Ect
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class for ECT transaction event
    /// </summary>
    public class EctEventArgs : EventArgs
    {
        public IEctTransaction EctTransaction { get; set; }

        public EctEventArgs(IEctTransaction ectTransaction)
        {
            EctTransaction = ectTransaction;
        }
    }
}
