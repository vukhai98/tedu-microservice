﻿using Infrastructure.Extensions;
using Newtonsoft.Json;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Baskets;
using System.Net;

namespace Saga.Orchestrator.HttpRepository
{
    public class BasketHttpRepository : IBasketHttpRepository
    {
        private readonly HttpClient _httpClient;

        public BasketHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeleteBasket(string userName)
        {
            var response = await _httpClient.DeleteAsync($"baskets/{userName}");

            var result = await response.ReadContentAs<bool>();

            return result;
        }

        public async Task<CartDTO> GetBasket(string userName)
        {
            var response = await _httpClient.GetAsync($"baskets/{userName}");

            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Kiểm tra xem response có thành công và status code là OK không

                    // Đọc dữ liệu từ response
                    var content = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(content))
                        return null;

                    // Chuyển đổi dữ liệu thành CartDTO
                    var cart = JsonConvert.DeserializeObject<CartDTO>(content);

                    return cart;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"An error when get baseket: {userName} - {ex.Message}");
                return null;
            }

        }
    }
}
