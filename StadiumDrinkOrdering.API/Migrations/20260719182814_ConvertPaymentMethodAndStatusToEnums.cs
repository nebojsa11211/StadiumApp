using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <summary>
    /// Converts Payments.PaymentMethod and Payments.Status from free-text columns to real enums
    /// (stored as integers, matching every other enum in this schema), and adds Payments.Direction.
    ///
    /// The text columns had drifted: "CreditCard" and "Credit Card" both existed, "Stripe" recorded a
    /// provider rather than a rail, and "CardRefund"/"CashPayout" encoded the direction into the method.
    /// The API masked all of it with an Enum.TryParse fallback to CreditCard, so a card refund displayed
    /// as a card purchase. Direction now carries the in/out axis and the method carries only the rail.
    ///
    /// Hand-written rather than left as scaffolded: EF emits a bare AlterColumn, which PostgreSQL rejects
    /// for varchar -> integer without a USING clause, and it defaulted Direction to 0 (not a valid enum
    /// value — PaymentDirection starts at In = 1). PostgreSQL rebuilds the indexes on these columns as
    /// part of ALTER COLUMN ... TYPE, so they need no explicit drop/recreate.
    /// </summary>
    public partial class ConvertPaymentMethodAndStatusToEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Direction, defaulting to In (=1) — every existing row is a customer payment unless it is
            //    one of the two legacy payout rails handled in step 2.
            migrationBuilder.Sql(@"
                ALTER TABLE ""Payments""
                ADD COLUMN ""Direction"" integer NOT NULL DEFAULT 1;
            ");

            // 2. Reclassify the legacy payout rails while PaymentMethod is still text.
            migrationBuilder.Sql(@"
                UPDATE ""Payments""
                SET ""Direction"" = 2
                WHERE ""PaymentMethod"" IN ('CardRefund', 'CashPayout');
            ");

            // 3. Refuse to convert anything we can't map. Guessing is what produced the bug this migration
            //    fixes, so an unrecognised value must stop the deploy loudly instead of silently becoming
            //    a credit-card payment. Keep this list in sync with PaymentMethodParser.
            migrationBuilder.Sql(@"
                DO $$
                DECLARE unmapped text;
                BEGIN
                    SELECT string_agg(DISTINCT ""PaymentMethod"", ', ')
                    INTO unmapped
                    FROM ""Payments""
                    WHERE lower(regexp_replace(""PaymentMethod"", '[^a-zA-Z0-9]', '', 'g')) NOT IN (
                        'creditcard', 'card', 'stripe', 'debitcard', 'digitalwallet', 'wallet',
                        'banktransfer', 'cash', 'ticketwallet', 'cardrefund', 'cashpayout'
                    );

                    IF unmapped IS NOT NULL THEN
                        RAISE EXCEPTION
                            'Cannot convert Payments.PaymentMethod: unmapped value(s): %. Add them to PaymentMethodParser and this migration before deploying.',
                            unmapped;
                    END IF;

                    SELECT string_agg(DISTINCT ""Status"", ', ')
                    INTO unmapped
                    FROM ""Payments""
                    WHERE lower(regexp_replace(""Status"", '[^a-zA-Z0-9]', '', 'g')) NOT IN (
                        'pending', 'processing', 'completed', 'failed', 'refunded', 'cancelled', 'canceled'
                    );

                    IF unmapped IS NOT NULL THEN
                        RAISE EXCEPTION
                            'Cannot convert Payments.Status: unmapped value(s): %.',
                            unmapped;
                    END IF;
                END $$;
            ");

            // 4. PaymentMethod -> PaymentMethod enum. "Stripe" is a provider (card charge) and the two
            //    payout rails collapse onto their underlying rail, their direction already recorded above.
            migrationBuilder.Sql(@"
                ALTER TABLE ""Payments""
                ALTER COLUMN ""PaymentMethod"" TYPE integer
                USING (
                    CASE lower(regexp_replace(""PaymentMethod"", '[^a-zA-Z0-9]', '', 'g'))
                        WHEN 'creditcard'    THEN 1
                        WHEN 'card'          THEN 1
                        WHEN 'stripe'        THEN 1
                        WHEN 'cardrefund'    THEN 1
                        WHEN 'debitcard'     THEN 2
                        WHEN 'digitalwallet' THEN 3
                        WHEN 'wallet'        THEN 3
                        WHEN 'banktransfer'  THEN 4
                        WHEN 'cash'          THEN 5
                        WHEN 'cashpayout'    THEN 5
                        WHEN 'ticketwallet'  THEN 6
                    END
                );
            ");

            // 5. Status -> PaymentStatus enum ('Canceled' tolerated alongside 'Cancelled').
            migrationBuilder.Sql(@"
                ALTER TABLE ""Payments""
                ALTER COLUMN ""Status"" TYPE integer
                USING (
                    CASE lower(regexp_replace(""Status"", '[^a-zA-Z0-9]', '', 'g'))
                        WHEN 'pending'    THEN 1
                        WHEN 'processing' THEN 2
                        WHEN 'completed'  THEN 3
                        WHEN 'failed'     THEN 4
                        WHEN 'refunded'   THEN 5
                        WHEN 'cancelled'  THEN 6
                        WHEN 'canceled'   THEN 6
                    END
                );
            ");

            // The DEFAULT 1 was only needed to backfill existing rows; the model supplies the value now.
            migrationBuilder.Sql(@"ALTER TABLE ""Payments"" ALTER COLUMN ""Direction"" DROP DEFAULT;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverses the type change. A payout row comes back as its original rail name
            // ("CardRefund"/"CashPayout"), reconstructed from Direction before that column is dropped,
            // so a round trip preserves the strings the application used to write.
            migrationBuilder.Sql(@"
                ALTER TABLE ""Payments""
                ALTER COLUMN ""PaymentMethod"" TYPE character varying(50)
                USING (
                    CASE
                        WHEN ""Direction"" = 2 AND ""PaymentMethod"" = 1 THEN 'CardRefund'
                        WHEN ""Direction"" = 2 AND ""PaymentMethod"" = 5 THEN 'CashPayout'
                        WHEN ""PaymentMethod"" = 1 THEN 'CreditCard'
                        WHEN ""PaymentMethod"" = 2 THEN 'DebitCard'
                        WHEN ""PaymentMethod"" = 3 THEN 'DigitalWallet'
                        WHEN ""PaymentMethod"" = 4 THEN 'BankTransfer'
                        WHEN ""PaymentMethod"" = 5 THEN 'Cash'
                        WHEN ""PaymentMethod"" = 6 THEN 'TicketWallet'
                    END
                );
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Payments""
                ALTER COLUMN ""Status"" TYPE character varying(20)
                USING (
                    CASE ""Status""
                        WHEN 1 THEN 'Pending'
                        WHEN 2 THEN 'Processing'
                        WHEN 3 THEN 'Completed'
                        WHEN 4 THEN 'Failed'
                        WHEN 5 THEN 'Refunded'
                        WHEN 6 THEN 'Cancelled'
                    END
                );
            ");

            migrationBuilder.Sql(@"ALTER TABLE ""Payments"" DROP COLUMN ""Direction"";");
        }
    }
}
