namespace Wallet_solution.Models.DTOs
{
    public class ResponseModel
    {
        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public object? Data { get; set; }
    }
}
