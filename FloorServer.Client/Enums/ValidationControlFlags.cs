#region

using System;

#endregion

namespace FloorServer.Client.Enums
{
    [Flags]
    public enum ValidationControlFlags : int
    {
        NONE = 0x00,
        USE_PRINTER_AS_CASHOUT_DEVICE = 0x01,
        USE_PRINTER_AS_HANDPAY_RECEIPT_DEVICE = 0x02,
        VALIDATE_HANDPAYS_AND_RECEIPTS = 0x04,
        PRINT_RESTRICTED_TICKETS = 0x08,
        TICKETS_FOR_FOREIGN_RESTRICTED_AMOUNTS = 0x10,
        ALLOW_TICKET_REDEMPTION = 0x20
    }
}
