namespace FloorServer.Client.Communication
{
    public interface IBomQuery
    {
        IBomQuery Get(params string[] fields);
        IBomQuery On(string table);
        IBomQuery Where(string whereClause);
    }
}
