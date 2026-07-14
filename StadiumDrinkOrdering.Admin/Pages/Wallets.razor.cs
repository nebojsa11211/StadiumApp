using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Admin.Common;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Wallets : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;

    private WalletAdminListDto? list;
    private string search = "";
    private WalletAdminDto? managed;
    private WalletTransactionListDto? ledger;
    private bool busy;

    private decimal adjustAmount;
    private string adjustReason = "";
    private decimal refundAmount;
    private string refundReason = "";

    private string alertMessage = "";
    private string alertType = "";

    private readonly PagedView<WalletAdminDto> pager = new();

    // Sorting — main wallets table
    private readonly TableSortState sortState = new();
    private static readonly Dictionary<string, Func<WalletAdminDto, object?>> SortSelectors = new()
    {
        // Sort by whichever owner identifier the wallet carries (username for user wallets, ticket
        // number for anonymous ones) so both kinds interleave sensibly.
        ["user"] = w => string.IsNullOrEmpty(w.Username) ? w.TicketNumber : w.Username,
        ["balance"] = w => w.Balance,
        ["status"] = w => w.Status,
    };

    // Primary owner label for a wallet row / the manage modal title.
    private string OwnerName(WalletAdminDto w) =>
        w.OwnerType == "Ticket"
            ? $"{L["Wallets_TicketOwner"]} {w.TicketNumber}".Trim()
            : w.Username;

    // Secondary line under the owner name.
    private string OwnerSub(WalletAdminDto w) =>
        w.OwnerType == "Ticket" ? L["Wallets_Anonymous"] : w.Email;

    private IEnumerable<WalletAdminDto> SortedWallets =>
        sortState.Column is null ? list!.Wallets : sortState.Apply(list!.Wallets, SortSelectors);

    private void SortBy(string column)
    {
        sortState.Toggle(column);
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync() => await Load();

    private async Task OnSearchKey(KeyboardEventArgs e)
    {
        if (e.Key == "Enter") await Load();
    }

    private async Task Load()
    {
        list = null;
        var q = string.IsNullOrWhiteSpace(search) ? "" : $"&search={Uri.EscapeDataString(search.Trim())}";
        list = await ApiService.GetAsync<WalletAdminListDto>($"api/admin/wallets?page=1&pageSize=500{q}")
               ?? new WalletAdminListDto();
        pager.Reset();
    }

    private async Task OpenManage(WalletAdminDto w)
    {
        managed = w;
        ledger = null;
        adjustAmount = 0; adjustReason = "";
        refundAmount = 0; refundReason = "";
        ledger = await ApiService.GetAsync<WalletTransactionListDto>($"api/admin/wallets/{w.WalletId}/transactions?page=1&pageSize=50");
    }

    private async Task Freeze() => await SetStatus("freeze");
    private async Task Unfreeze() => await SetStatus("unfreeze");

    private async Task SetStatus(string action)
    {
        if (managed == null) return;
        busy = true;
        try
        {
            var res = await ApiService.PostAsync<object>($"api/admin/wallets/{managed.WalletId}/{action}", null);
            await AfterMutation(res != null, action == "freeze" ? L["Wallets_Frozen"] : L["Wallets_Unfrozen"]);
        }
        finally { busy = false; }
    }

    private async Task Adjust()
    {
        if (managed == null) return;
        busy = true;
        try
        {
            var dto = new AdjustWalletDto { Amount = adjustAmount, Reason = adjustReason.Trim() };
            var txn = await ApiService.PostAsync<WalletTransactionDto>($"api/admin/wallets/{managed.WalletId}/adjust", dto);
            await AfterMutation(txn != null, L["Wallets_AdjustDone"]);
        }
        finally { busy = false; }
    }

    private async Task Refund()
    {
        if (managed == null) return;
        busy = true;
        try
        {
            var dto = new RefundWalletDto { Amount = refundAmount, Reason = refundReason.Trim() };
            var txn = await ApiService.PostAsync<WalletTransactionDto>($"api/admin/wallets/{managed.WalletId}/refund", dto);
            await AfterMutation(txn != null, L["Wallets_RefundDone"]);
        }
        finally { busy = false; }
    }

    private async Task AfterMutation(bool ok, string successMsg)
    {
        if (!ok)
        {
            ShowAlert(L["Wallets_ActionFailed"], "danger");
            return;
        }
        ShowAlert(successMsg, "success");
        var id = managed!.WalletId;
        await Load();
        // Re-bind the managed wallet + ledger to reflect the new state.
        managed = list?.Wallets.FirstOrDefault(x => x.WalletId == id) ?? managed;
        if (managed != null)
            ledger = await ApiService.GetAsync<WalletTransactionListDto>($"api/admin/wallets/{id}/transactions?page=1&pageSize=50");
        adjustAmount = 0; adjustReason = "";
        refundAmount = 0; refundReason = "";
    }

    private static string FormatMoney(decimal amount, string currency)
    {
        var symbol = currency == "EUR" ? "€" : currency + " ";
        return $"{symbol}{Math.Abs(amount).ToString("N2")}";
    }

    private void ShowAlert(string message, string type)
    {
        alertMessage = message;
        alertType = type;
        StateHasChanged();
        _ = Task.Delay(4000).ContinueWith(_ => { alertMessage = ""; InvokeAsync(StateHasChanged); });
    }
}
