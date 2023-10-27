using Infrastructure.Extensions;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Inventory;
using System.Net.Http.Json;

namespace Saga.Orchestrator.HttpRepository
{
    public class InventoryHttpRepository : IInventoryHttpRepository
    {
        private readonly HttpClient _httpClient;
        public InventoryHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CreateSalesOrder(SalesProductDto model)
        {
            var respone = await _httpClient.PostAsJsonAsync($"inventory/sales/{model.ItemNo}", model);

            if (!respone.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                throw new Exception($"Create sale order for item: {model.ItemNo} not success");
            }
            var inventory = await respone.ReadContentAs<InventoryEntryDto>();

            return inventory.DocumentNo;
        }

        public async Task<bool> DeleteOrderByDocumentNo(string documentNo)
        {
            var respone = await _httpClient.DeleteAsync($"inventory/document-no/{documentNo}");

            if (!respone.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                throw new Exception($"Delete order for Document No: {documentNo} not succes");
            }

            var result = await respone.ReadContentAs<bool>();

            return result;
        }
    }
}
