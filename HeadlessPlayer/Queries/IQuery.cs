namespace HeadlessPlayer.Queries
{
    using System.Threading.Tasks;

    public interface IQuery<T>
    {
        Task<T> ExecuteAsync();
    }
}