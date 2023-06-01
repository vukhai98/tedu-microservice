namespace Customer.API.Services.Interfaces
{
    public interface ICustomerService 
    {
        Task<IResult> GetUserByName(string userName);
    }
}
