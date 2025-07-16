namespace ApiTools
{
    public interface ISwaggerEndPointInfoProvider
    {
        string GetUrl(string version);
        string GetName(string version);
    }
}
