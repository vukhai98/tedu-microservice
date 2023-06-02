namespace Customer.API.Services.Interfaces
{
    public interface ICustomerServices
    {
        Task<IResult> GetCustomerByUserName(string userName);
    }
}
