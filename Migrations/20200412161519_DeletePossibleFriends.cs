using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ChatApi.Migrations
{
    public partial class DeletePossibleFriends : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Users_SenderId",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_SenderId",
                table: "Friendships");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Friendships_SenderId",
                table: "Friendships",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Users_SenderId",
                table: "Friendships",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
