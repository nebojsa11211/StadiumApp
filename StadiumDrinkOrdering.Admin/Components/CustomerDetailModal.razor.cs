using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Admin.Services;
using System.Text;

namespace StadiumDrinkOrdering.Admin.Components;

public partial class CustomerDetailModalBase : ComponentBase
{
    [Inject] protected IAdminApiService ApiService { get; set; } = default!;
    [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;

    protected CustomerSpendingDetailDto? customerDetail;
    protected bool isLoading = false;
    protected string? errorMessage = null;
    protected string currentCustomerEmail = string.Empty;

    public async Task ShowCustomerDetails(string customerEmail)
    {
        currentCustomerEmail = customerEmail;
        errorMessage = null;
        
        await JSRuntime.InvokeVoidAsync("eval", "new bootstrap.Modal(document.getElementById('customerDetailModal')).show()");
        await LoadCustomerDetails(customerEmail);
    }

    protected async Task LoadCustomerDetails(string customerEmail)
    {
        if (string.IsNullOrEmpty(customerEmail))
            return;

        isLoading = true;
        errorMessage = null;
        StateHasChanged();

        try
        {
            customerDetail = await ApiService.GetCustomerSpendingDetailsAsync(customerEmail);
            if (customerDetail == null)
            {
                errorMessage = "Customer details not found.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Error loading customer details: {ex.Message}";
            customerDetail = null;
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    protected async Task ExportCustomerData()
    {
        if (customerDetail == null)
            return;

        try
        {
            var fileName = $"customer-details-{customerDetail.CustomerEmail.Replace("@", "-").Replace(".", "-")}-{DateTime.Now:yyyyMMdd}.csv";
            var csvContent = GenerateCustomerCsv();
            
            await JSRuntime.InvokeVoidAsync("downloadFile", fileName, csvContent);
        }
        catch (Exception ex)
        {
            errorMessage = $"Error exporting data: {ex.Message}";
            StateHasChanged();
        }
    }

    protected string GenerateCustomerCsv()
    {
        if (customerDetail == null)
            return string.Empty;

        var csv = new StringBuilder();
        csv.AppendLine("Customer Details Export");
        csv.AppendLine($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        csv.AppendLine();
        
        csv.AppendLine("Customer Information");
        csv.AppendLine("Field,Value");
        csv.AppendLine($"Name,\"{customerDetail.CustomerName}\"");
        csv.AppendLine($"Email,\"{customerDetail.CustomerEmail}\"");
        csv.AppendLine($"First Purchase,\"{customerDetail.Summary.FirstPurchase:yyyy-MM-dd}\"");
        csv.AppendLine($"Last Purchase,\"{customerDetail.Summary.LastPurchase:yyyy-MM-dd}\"");
        csv.AppendLine();
        
        csv.AppendLine("Spending Summary");
        csv.AppendLine("Category,Amount");
        csv.AppendLine($"Total Spent,{customerDetail.Summary.TotalSpent:F2}");
        csv.AppendLine($"Ticket Spending,{customerDetail.Summary.TicketSpending:F2}");
        csv.AppendLine($"Drink Spending,{customerDetail.Summary.DrinkSpending:F2}");
        csv.AppendLine($"Average Ticket Price,{customerDetail.Summary.AverageTicketPrice:F2}");
        csv.AppendLine($"Average Drink Order,{customerDetail.Summary.AverageDrinkOrderValue:F2}");
        csv.AppendLine();
        
        csv.AppendLine("Activity Stats");
        csv.AppendLine("Metric,Count");
        csv.AppendLine($"Total Tickets,{customerDetail.Summary.TotalTickets}");
        csv.AppendLine($"Events Attended,{customerDetail.Summary.TotalEvents}");
        csv.AppendLine($"Drink Orders,{customerDetail.Summary.TotalDrinkOrders}");
        
        if (customerDetail.TicketPurchases?.Any() == true)
        {
            csv.AppendLine();
            csv.AppendLine("Ticket Purchases");
            csv.AppendLine("Date,Event,Section,Seat,Price");
            
            foreach (var purchase in customerDetail.TicketPurchases.Take(50))
            {
                csv.AppendLine($"\"{purchase.PurchaseDate:yyyy-MM-dd}\",\"{purchase.EventName}\",\"{purchase.SectionName}\",\"{purchase.SeatCode}\",{purchase.Price:F2}");
            }
        }
        
        return csv.ToString();
    }
}