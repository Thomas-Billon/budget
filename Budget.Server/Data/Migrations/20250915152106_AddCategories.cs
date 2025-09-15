using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Transactions_TransactionId",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Categories",
                newName: "ParentCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_TransactionId",
                table: "Categories",
                newName: "IX_Categories_ParentCategoryId");

            migrationBuilder.AddColumn<int>(
                name: "Color",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CategoryTransaction",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    TransactionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTransaction", x => new { x.CategoriesId, x.TransactionsId });
                    table.ForeignKey(
                        name: "FK_CategoryTransaction_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTransaction_Transactions_TransactionsId",
                        column: x => x.TransactionsId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTransaction_TransactionsId",
                table: "CategoryTransaction",
                column: "TransactionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentCategoryId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "CategoryTransaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "ParentCategoryId",
                table: "Category",
                newName: "TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Category",
                newName: "IX_Category_TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Transactions_TransactionId",
                table: "Category",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }
    }
}
