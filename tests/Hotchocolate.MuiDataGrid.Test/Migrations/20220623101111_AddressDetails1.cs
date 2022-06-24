using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stackworx.Hotchocolate.MuiDataGrid.Migrations
{
    public partial class AddressDetails1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Apartment",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Apartment",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "House 1");

            migrationBuilder.UpdateData(
                table: "Apartment",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "House Number 2");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateOfBirth", "IdCardReceivedDate" },
                values: new object[] { new DateTime(2022, 6, 23, 12, 11, 11, 442, DateTimeKind.Local).AddTicks(4720), new DateTime(2022, 6, 23, 12, 11, 11, 442, DateTimeKind.Local).AddTicks(4750) });

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateOfBirth", "IdCardReceivedDate" },
                values: new object[] { new DateTime(2022, 6, 22, 12, 11, 11, 442, DateTimeKind.Local).AddTicks(4750), new DateTime(2022, 6, 22, 12, 11, 11, 442, DateTimeKind.Local).AddTicks(4760) });

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateOfBirth", "IdCardReceivedDate" },
                values: new object[] { new DateTime(2022, 6, 21, 12, 11, 11, 442, DateTimeKind.Local).AddTicks(4760), new DateTime(2022, 6, 21, 12, 11, 11, 442, DateTimeKind.Local).AddTicks(4770) });

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateOfBirth", "IdCardReceivedDate" },
                values: new object[] { new DateTime(2022, 6, 20, 12, 11, 11, 442, DateTimeKind.Local).AddTicks(4770), new DateTime(2022, 6, 20, 12, 11, 11, 442, DateTimeKind.Local).AddTicks(4780) });

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateOfBirth", "IdCardReceivedDate" },
                values: new object[] { new DateTime(2022, 6, 19, 12, 11, 11, 442, DateTimeKind.Local).AddTicks(4780), new DateTime(2022, 6, 19, 12, 11, 11, 442, DateTimeKind.Local).AddTicks(4780) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Apartment");

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateOfBirth", "IdCardReceivedDate" },
                values: new object[] { new DateTime(2022, 6, 23, 11, 20, 17, 295, DateTimeKind.Local).AddTicks(8300), new DateTime(2022, 6, 23, 11, 20, 17, 295, DateTimeKind.Local).AddTicks(8330) });

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateOfBirth", "IdCardReceivedDate" },
                values: new object[] { new DateTime(2022, 6, 22, 11, 20, 17, 295, DateTimeKind.Local).AddTicks(8330), new DateTime(2022, 6, 22, 11, 20, 17, 295, DateTimeKind.Local).AddTicks(8340) });

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateOfBirth", "IdCardReceivedDate" },
                values: new object[] { new DateTime(2022, 6, 21, 11, 20, 17, 295, DateTimeKind.Local).AddTicks(8350), new DateTime(2022, 6, 21, 11, 20, 17, 295, DateTimeKind.Local).AddTicks(8350) });

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateOfBirth", "IdCardReceivedDate" },
                values: new object[] { new DateTime(2022, 6, 20, 11, 20, 17, 295, DateTimeKind.Local).AddTicks(8350), new DateTime(2022, 6, 20, 11, 20, 17, 295, DateTimeKind.Local).AddTicks(8360) });

            migrationBuilder.UpdateData(
                table: "People",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateOfBirth", "IdCardReceivedDate" },
                values: new object[] { new DateTime(2022, 6, 19, 11, 20, 17, 295, DateTimeKind.Local).AddTicks(8360), new DateTime(2022, 6, 19, 11, 20, 17, 295, DateTimeKind.Local).AddTicks(8360) });
        }
    }
}
