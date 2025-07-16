namespace FloorServer.Client.Enums
{
    public enum EctResultCodes : int
    {
        ERR_CM_ACK = -2, //When an ack with STATUS="ERR" is received after sending an ECT START command
        RESPONSE_TIMEOUT = -1,//When no response is received after sending of an ECT START command (no  reception of EXC 1092 after sending of CMD 3000 (ECT_START command)).
        ECT_OK = 0,
        REJECT_BY_PLAYER = 1,
        TIMEOUT_OF_PLAYER_PROMPT=2,
        TIMEOUT_OF_REQUEST = 3,//(currently not sent by the devices)
        BUSY = 4,
        INVALID_CARD = 5,//(card number does not match)
        NO_CARD = 6,
        DOOR_OPEN = 7,
        CASH_OUT = 8,//(currently not sent by the devices)
        ECT_TRANSFER_LIMIT_EXCEED = 9,
        GENERIC_ERROR = 10,
        PARTIAL_TRANSFER = 129,
        /**
         * FATAL_ERROR = 130:
         * in this case, the ECT_AMOUNT reported is 0, but the SMIB cannot report the actual transferred amount, 
	     * so further investigation on the gaming machine is required to find out how much was actually transferred.) 
	     * These error codes are provided for information only (display). The host system must always commit the transferred amount, 
	     * independently of the error code.
         */

        FATAL_ERROR = 130


    }
}
