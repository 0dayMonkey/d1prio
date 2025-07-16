namespace NotificationService.Infrastructure.RTDS
{
    public enum ConnectionState
    {
        None,
        Opening,
        Renaming,
        Sending,
        Initialized,
        Closing,
        Closed
    }
}