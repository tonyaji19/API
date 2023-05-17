using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstrains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_m_account_roles",
                table: "tb_m_account_roles");

            migrationBuilder.RenameTable(
                name: "tb_m_account_roles",
                newName: "tb_tr_account_roles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_tr_account_roles",
                table: "tb_tr_account_roles",
                column: "guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_nik_email_phone_number",
                table: "tb_m_employees",
                columns: new[] { "nik", "email", "phone_number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_m_employees_nik_email_phone_number",
                table: "tb_m_employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_tr_account_roles",
                table: "tb_tr_account_roles");

            migrationBuilder.RenameTable(
                name: "tb_tr_account_roles",
                newName: "tb_m_account_roles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_m_account_roles",
                table: "tb_m_account_roles",
                column: "guid");
        }
    }
}
