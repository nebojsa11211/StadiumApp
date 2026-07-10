using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface ITicketCardPdfService
{
    /// <summary>Renders a printable ticket card (A6 landscape) with the ticket details and QR image embedded.</summary>
    byte[] GenerateTicketCard(Ticket ticket, byte[] qrPng);
}

public class TicketCardPdfService : ITicketCardPdfService
{
    private const string Ink = "#0f172a";      // slate-900
    private const string Accent = "#1d4ed8";   // blue-700
    private const string Muted = "#64748b";    // slate-500
    private const string Line = "#e2e8f0";     // slate-200

    public byte[] GenerateTicketCard(Ticket ticket, byte[] qrPng)
    {
        var eventName = string.IsNullOrWhiteSpace(ticket.EventName) ? "Event" : ticket.EventName!;
        var eventDate = ticket.EventDate?.ToString("dddd, dd MMM yyyy • HH:mm") ?? "—";

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A6.Landscape());
                page.Margin(18);
                page.DefaultTextStyle(t => t.FontSize(9).FontColor(Ink));

                page.Content().Border(1).BorderColor(Line).Column(root =>
                {
                    // Accent header band
                    root.Item().Background(Accent).PaddingVertical(10).PaddingHorizontal(14).Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("STADIUM ENTRY TICKET").FontSize(8).FontColor("#dbeafe").LetterSpacing(0.25f).SemiBold();
                            c.Item().Text(eventName).FontSize(14).FontColor("#ffffff").Bold();
                        });
                        if (ticket.Kind == TicketKind.Season)
                        {
                            row.ConstantItem(78).AlignRight().AlignMiddle()
                               .Background("#ffffff").PaddingVertical(3).PaddingHorizontal(8)
                               .Text("SEASON").FontSize(8).FontColor(Accent).Bold();
                        }
                    });

                    // Body: details on the left, QR on the right
                    root.Item().PaddingHorizontal(14).PaddingVertical(12).Row(row =>
                    {
                        row.RelativeItem().Column(info =>
                        {
                            info.Spacing(7);
                            info.Item().Element(e => Field(e, "DATE & TIME", eventDate));
                            info.Item().Row(r =>
                            {
                                r.RelativeItem().Element(e => Field(e, "SECTION", string.IsNullOrWhiteSpace(ticket.Section) ? "—" : ticket.Section!));
                                r.RelativeItem().Element(e => Field(e, "ROW", string.IsNullOrWhiteSpace(ticket.Row) ? "—" : ticket.Row!));
                                r.RelativeItem().Element(e => Field(e, "SEAT", string.IsNullOrWhiteSpace(ticket.SeatNumber) ? "—" : ticket.SeatNumber!));
                            });
                            if (!string.IsNullOrWhiteSpace(ticket.CustomerName))
                                info.Item().Element(e => Field(e, "HOLDER", ticket.CustomerName!));
                        });

                        row.ConstantItem(96).AlignRight().Column(qr =>
                        {
                            qr.Item().AlignRight().Width(88).Image(qrPng).FitWidth();
                            qr.Item().PaddingTop(3).AlignRight().Text(ticket.TicketNumber).FontSize(8).FontColor(Muted);
                        });
                    });

                    // Perforation-style divider
                    root.Item().PaddingHorizontal(14).LineHorizontal(1).LineColor(Line);

                    // Price footer
                    root.Item().PaddingHorizontal(14).PaddingVertical(10).Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("TICKET PRICE").FontSize(7).FontColor(Muted).LetterSpacing(0.25f);
                            c.Item().Text(FormatMoney(ticket.Price)).FontSize(15).FontColor(Accent).Bold();
                        });
                        row.RelativeItem().AlignRight().AlignBottom()
                           .Text($"Purchased {ticket.PurchaseDate:dd MMM yyyy}").FontSize(8).FontColor(Muted);
                    });
                });
            });
        }).GeneratePdf();
    }

    private static void Field(IContainer container, string label, string value)
    {
        container.Column(c =>
        {
            c.Item().Text(label).FontSize(7).FontColor(Muted).LetterSpacing(0.25f);
            c.Item().Text(value).FontSize(11).FontColor(Ink).SemiBold();
        });
    }

    private static string FormatMoney(decimal amount) => $"€{amount:0.00}";
}
