using Marketplace.Payment.Domain.Enums;
using Marketplace.Payment.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Marketplace.Payment.Infrastructure.PaymentProcessor
{
    public class BankCardService : IPaymentProcessor
    {
        private readonly HttpClient _httpClient;
        private readonly YooMoneySettings _settings;

        public BankCardService(HttpClient httpClient, IOptions<YooMoneySettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.BaseAddress = new Uri(_settings.ApiUrl);

            var auth = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_settings.ShopId}:{_settings.SecretKey}")
            );
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
        }

        //Не работает
        //public async Task<string> ProcessPaymentAsync(
        //     Guid paymentId,
        //     decimal amount,
        //     string currency,
        //     Dictionary<string, string> paymentDetails,
        //     CancellationToken cancellationToken)
        //{
        //    if (!paymentDetails.TryGetValue("card_number", out var cardNumber) ||
        //        !paymentDetails.TryGetValue("expiry_date", out var expiryDate) ||
        //        !paymentDetails.TryGetValue("cvv", out var cvv))
        //    {
        //        throw new Exception("Не указаны данные карты");
        //    }

        //    try
        //    {
        //        var expiryParts = expiryDate.Split('/');
        //        var request = new
        //        {
        //            amount = new { value = amount, currency },
        //            capture = true,
        //            payment_method_data = new
        //            {
        //                type = "bank_card",
        //                card = new
        //                {
        //                    card_number = cardNumber,
        //                    expiry_month = expiryParts[0],
        //                    expiry_year = expiryParts[1],
        //                    csc = cvv
        //                }
        //            },
        //            confirmation = new
        //            {
        //                type = "redirect",
        //                return_url = "https://example.com/"
        //            },
        //            description = $"Payment for order {paymentId}"
        //        };

        //        Console.WriteLine("Sending: " + JsonSerializer.Serialize(request));

        //        _httpClient.DefaultRequestHeaders.Remove("Idempotence-Key");
        //        _httpClient.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

        //        var content = new StringContent(
        //            JsonSerializer.Serialize(request),
        //            Encoding.UTF8,
        //            "application/json"
        //        );

        //        var response = await _httpClient.PostAsync("payments", content, cancellationToken);
        //        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        //        Console.WriteLine("Received: " + responseBody);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            //var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        //            Console.WriteLine($"YooKassa error response: {responseBody}");
        //            var errorJson = JsonSerializer.Deserialize<JsonElement>(responseBody);
        //            var errorCode = errorJson.TryGetProperty("code", out var code) ? code.GetString() : "Unknown";
        //            var errorDescription = errorJson.TryGetProperty("description", out var desc) ? desc.GetString() : "No description";
        //            throw new Exception($"YooKassa API error: Code={errorCode}, Description={errorDescription}");
        //        }

        //        var responseJson = JsonSerializer.Deserialize<JsonElement>(responseBody);
        //        return responseJson.GetProperty("id").GetString();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Payment failed: {ex}");
        //        throw;
        //    }
        //}

        public async Task<string> ProcessPaymentAsync(
            Guid paymentId,
            decimal amount,
            string currency,
            Dictionary<string, string> paymentDetails,
            CancellationToken cancellationToken)
        {
            await Task.Delay(100);
            return Guid.NewGuid().ToString();
        }

        public async Task<bool> ProcessRefundAsync(
            string transactionId,
            decimal amount,
            string currency,
            CancellationToken cancellationToken)
        {
            var request = new
            {
                amount = new { value = amount, currency },
                payment_id = transactionId
            };

            _httpClient.DefaultRequestHeaders.Remove("Idempotence-Key");
            _httpClient.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("refunds", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception($"YooMoney refund error: {error}");
            }

            return true;
        }
    }
}