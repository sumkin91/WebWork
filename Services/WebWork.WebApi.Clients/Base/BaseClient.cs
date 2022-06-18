namespace WebWork.WebApi.Clients.Base;

public abstract class BaseClient
{
    protected HttpClient Http { get; }
    protected string Address { get; }

    protected BaseClient(HttpClient Client, string Address)
    {
        Http = Client;
        this.Address = Address;
    }
}
