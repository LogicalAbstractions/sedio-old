using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sedio.Server.Runtime.Model.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceId = table.Column<string>(maxLength: 48, nullable: false),
                    HealthAggregation_ProviderId = table.Column<string>(maxLength: 48, nullable: false),
                    HealthAggregation_ProviderParametersJson = table.Column<string>(nullable: true),
                    CacheTime = table.Column<TimeSpan>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceVersions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Version = table.Column<string>(maxLength: 128, nullable: false),
                    VersionOrder = table.Column<int>(nullable: false),
                    HealthCheck_ProviderId = table.Column<string>(maxLength: 48, nullable: false),
                    HealthCheck_ProviderParametersJson = table.Column<string>(nullable: true),
                    HealthAggregation_ProviderId = table.Column<string>(maxLength: 48, nullable: false),
                    HealthAggregation_ProviderParametersJson = table.Column<string>(nullable: true),
                    Notification_ProviderId = table.Column<string>(maxLength: 48, nullable: false),
                    Notification_ProviderParametersJson = table.Column<string>(nullable: true),
                    InstanceRouting_ProviderId = table.Column<string>(maxLength: 48, nullable: false),
                    InstanceRouting_ProviderParametersJson = table.Column<string>(nullable: true),
                    InstanceRetirement_ProviderId = table.Column<string>(maxLength: 48, nullable: false),
                    InstanceRetirement_ProviderParametersJson = table.Column<string>(nullable: true),
                    Orchestration_ProviderId = table.Column<string>(maxLength: 48, nullable: false),
                    Orchestration_ProviderParametersJson = table.Column<string>(nullable: true),
                    CacheTime = table.Column<TimeSpan>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ServiceId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceVersions_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceDependencies",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceId = table.Column<string>(maxLength: 48, nullable: false),
                    VersionRequirement = table.Column<string>(maxLength: 128, nullable: false),
                    ServiceVersionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDependencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceDependencies_ServiceVersions_ServiceVersionId",
                        column: x => x.ServiceVersionId,
                        principalTable: "ServiceVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceEndpoints",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Protocol = table.Column<string>(maxLength: 48, nullable: false),
                    Port = table.Column<int>(nullable: false),
                    ServiceVersionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceEndpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceEndpoints_ServiceVersions_ServiceVersionId",
                        column: x => x.ServiceVersionId,
                        principalTable: "ServiceVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceInstances",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(maxLength: 48, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ServiceVersionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceInstances_ServiceVersions_ServiceVersionId",
                        column: x => x.ServiceVersionId,
                        principalTable: "ServiceVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ServiceInstanceId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceStatus_ServiceInstances_ServiceInstanceId",
                        column: x => x.ServiceInstanceId,
                        principalTable: "ServiceInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDependencies_ServiceId",
                table: "ServiceDependencies",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDependencies_ServiceVersionId",
                table: "ServiceDependencies",
                column: "ServiceVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDependencies_VersionRequirement",
                table: "ServiceDependencies",
                column: "VersionRequirement");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceEndpoints_ServiceVersionId",
                table: "ServiceEndpoints",
                column: "ServiceVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInstances_Address",
                table: "ServiceInstances",
                column: "Address");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInstances_ServiceVersionId",
                table: "ServiceInstances",
                column: "ServiceVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceId",
                table: "Services",
                column: "ServiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatus_ServiceInstanceId",
                table: "ServiceStatus",
                column: "ServiceInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceStatus_Status",
                table: "ServiceStatus",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceVersions_ServiceId",
                table: "ServiceVersions",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceVersions_Version",
                table: "ServiceVersions",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceVersions_VersionOrder",
                table: "ServiceVersions",
                column: "VersionOrder");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceDependencies");

            migrationBuilder.DropTable(
                name: "ServiceEndpoints");

            migrationBuilder.DropTable(
                name: "ServiceStatus");

            migrationBuilder.DropTable(
                name: "ServiceInstances");

            migrationBuilder.DropTable(
                name: "ServiceVersions");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
