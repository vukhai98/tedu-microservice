namespace Saga.Orchestrator.OrderManager
{
    public class OrderRespone
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public OrderRespone(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public OrderRespone(bool success)
        {
            Success = success;
        }
    }
}
