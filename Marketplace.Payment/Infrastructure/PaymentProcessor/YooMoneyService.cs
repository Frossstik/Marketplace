using Marketplace.Payment.Domain.Enums;
using Marketplace.Payment.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Marketplace.Payment.Infrastructure.PaymentProcessor
{
    public class YooMoneyService : IPaymentProcessor
    {
        private readonly HttpClient _httpClient;
        private readonly YooMoneySettings _settings;

        public YooMoneyService(HttpClient httpClient, IOptions<YooMoneySettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;

            _httpClient.BaseAddress = new Uri(_settings.ApiUrl);

            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_settings.ShopId}:{_settings.SecretKey}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
        }

        public async Task<string> ProcessPaymentAsync(
            Guid paymentId,
            decimal amount,
            string currency,
            Dictionary<string, string> paymentDetails,
            CancellationToken cancellationToken)
        {
            var request = new
            {
                amount = new { value = amount, currency },
                capture = true,
                payment_method_data = new
                {
                    type = "yoo_money"
                },
                confirmation = new
                {
                    type = "redirect",
                    return_url = "https://example.com/"
                },
                description = $"Payment for order {paymentId}",
                metadata = new { payment_id = paymentId.ToString() }
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            content.Headers.Add("Idempotence-Key", Guid.NewGuid().ToString());

            var response = await _httpClient.PostAsync("payments", content, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"YooMoney API error: {response.StatusCode} - {responseBody}");

            var responseData = JsonSerializer.Deserialize<JsonElement>(responseBody);
            return responseData.GetProperty("id").GetString() ?? throw new Exception("No transaction ID returned");
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

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            content.Headers.Add("Idempotence-Key", Guid.NewGuid().ToString());

            var response = await _httpClient.PostAsync("refunds", content, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"YooMoney refund error: {response.StatusCode} - {responseBody}");

            return true;
        }
    }
}
