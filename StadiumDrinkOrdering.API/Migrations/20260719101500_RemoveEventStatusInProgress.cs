using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using StadiumDrinkOrdering.API.Data;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <summary>
    /// Retires <c>EventStatus.InProgress</c> (value 5). Nothing in the codebase ever branched on it
    /// separately from <c>Active</c> (4) — it shared the same lifecycle phase, drink-ordering rule,
    /// single-live invariant and admin styling — so it was a label that cost an irreversible click
    /// and bought no behaviour. Any event sitting at 5 folds back to <c>Active</c>, which is exactly
    /// what it already meant operationally.
    ///
    /// Data-only: <c>Events.Status</c> is a plain integer with no check constraint, so the model is
    /// unchanged and no snapshot update is required. The value 5 is left retired rather than reused
    /// by a later status, so historical rows and logs can never be silently misread.
    /// </summary>
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260719101500_RemoveEventStatusInProgress")]
    public partial class RemoveEventStatusInProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE ""Events"" SET ""Status"" = 4 WHERE ""Status"" = 5;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Not reversible: once folded into Active, the events that were at InProgress are
            // indistinguishable from ones that were always Active. Down is a deliberate no-op
            // rather than a guess that would mislabel genuinely-Active events.
        }
    }
}
