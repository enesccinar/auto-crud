using System;
using Microsoft.EntityFrameworkCore.Migrations;
// ReSharper disable InconsistentNaming

namespace Firebend.AutoCrud.Web.Sample.Migrations
{
#pragma warning disable IDE1006
    public partial class addedModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                "CreatedDate",
                "EfPeople",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                "ModifiedDate",
                "EfPeople",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "CreatedDate",
                "EfPeople");

            migrationBuilder.DropColumn(
                "ModifiedDate",
                "EfPeople");
        }
    }

#pragma warning restore IDE1006
}
