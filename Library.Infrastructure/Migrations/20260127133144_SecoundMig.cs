using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SecoundMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chat_notifications_chats_NotificationId",
                table: "chat_notifications");

            migrationBuilder.AddColumn<int>(
                name: "TotalBooks",
                table: "Categories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "books",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 27, 13, 31, 42, 939, DateTimeKind.Utc).AddTicks(460));

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "authors",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_chat_notifications_chats_ChatId",
                table: "chat_notifications",
                column: "ChatId",
                principalTable: "chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chat_notifications_chats_ChatId",
                table: "chat_notifications");

            migrationBuilder.DropColumn(
                name: "TotalBooks",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "books");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "authors");

            migrationBuilder.AddForeignKey(
                name: "FK_chat_notifications_chats_NotificationId",
                table: "chat_notifications",
                column: "NotificationId",
                principalTable: "chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
