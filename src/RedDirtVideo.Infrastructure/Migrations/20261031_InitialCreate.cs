using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                PasswordHash = table.Column<string>(type: "text", nullable: true),
                FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                CompanyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "USER"),
                EmailVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_users", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_users_Email",
            table: "users",
            column: "Email",
            unique: true);

        migrationBuilder.CreateTable(
            name: "artists",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                StageName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                LegalName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                Genre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Bio = table.Column<string>(type: "text", nullable: true),
                WebsiteUrl = table.Column<string>(type: "text", nullable: true),
                FacebookUrl = table.Column<string>(type: "text", nullable: true),
                InstagramUrl = table.Column<string>(type: "text", nullable: true),
                YoutubeUrl = table.Column<string>(type: "text", nullable: true),
                SpotifyUrl = table.Column<string>(type: "text", nullable: true),
                LogoUrl = table.Column<string>(type: "text", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_artists", x => x.Id);
                table.ForeignKey(
                    name: "FK_artists_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "assets",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Filename = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                FileType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                MimeType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                FileSize = table.Column<long>(type: "bigint", nullable: true),
                StorageUrl = table.Column<string>(type: "text", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_assets", x => x.Id);
                table.ForeignKey(
                    name: "FK_assets_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "subscription_plans",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                MonthlyPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                MonthlyCredits = table.Column<int>(type: "integer", nullable: false),
                MaxProjects = table.Column<int>(type: "integer", nullable: true),
                MaxStorageGb = table.Column<int>(type: "integer", nullable: true),
                MaxResolution = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_subscription_plans", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "payments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "USD"),
                Provider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "STRIPE"),
                ProviderTransactionId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "PENDING"),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_payments", x => x.Id);
                table.ForeignKey(
                    name: "FK_payments_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "audit_logs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Action = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                EntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                EntityId = table.Column<Guid>(type: "uuid", nullable: true),
                Details = table.Column<string>(type: "text", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_audit_logs", x => x.Id);
                table.ForeignKey(
                    name: "FK_audit_logs_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "songs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                Genre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Bpm = table.Column<int>(type: "integer", nullable: true),
                DurationSeconds = table.Column<int>(type: "integer", nullable: true),
                AudioAssetId = table.Column<Guid>(type: "uuid", nullable: true),
                CoverArtAssetId = table.Column<Guid>(type: "uuid", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_songs", x => x.Id);
                table.ForeignKey(
                    name: "FK_songs_artists_ArtistId",
                    column: x => x.ArtistId,
                    principalTable: "artists",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "lyrics",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SongId = table.Column<Guid>(type: "uuid", nullable: false),
                LyricsText = table.Column<string>(type: "text", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_lyrics", x => x.Id);
                table.ForeignKey(
                    name: "FK_lyrics_songs_SongId",
                    column: x => x.SongId,
                    principalTable: "songs",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "venues",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                VenueType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Capacity = table.Column<int>(type: "integer", nullable: true),
                Description = table.Column<string>(type: "text", nullable: true),
                ThumbnailUrl = table.Column<string>(type: "text", nullable: true),
                Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_venues", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "projects",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                ArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                SongId = table.Column<Guid>(type: "uuid", nullable: true),
                Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                Style = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_projects", x => x.Id);
                table.ForeignKey(
                    name: "FK_projects_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_projects_artists_ArtistId",
                    column: x => x.ArtistId,
                    principalTable: "artists",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_projects_songs_SongId",
                    column: x => x.SongId,
                    principalTable: "songs",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "subscriptions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                CreditsRemaining = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "ACTIVE"),
                StartsAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                EndsAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_subscriptions", x => x.Id);
                table.ForeignKey(
                    name: "FK_subscriptions_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_subscriptions_subscription_plans_PlanId",
                    column: x => x.PlanId,
                    principalTable: "subscription_plans",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "ai_jobs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                ProjectId = table.Column<Guid>(type: "uuid", nullable: true),
                JobType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                QueueName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 5),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "QUEUED"),
                Progress = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                ErrorMessage = table.Column<string>(type: "text", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ai_jobs", x => x.Id);
                table.ForeignKey(
                    name: "FK_ai_jobs_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ai_jobs_projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "storyboards",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                GeneratedPrompt = table.Column<string>(type: "text", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_storyboards", x => x.Id);
                table.ForeignKey(
                    name: "FK_storyboards_projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "venues",
            columns: new[] { "Id", "Name", "VenueType", "Capacity", "Description", "ThumbnailUrl", "Active", "CreatedAt" },
            values: new object[,]
            {
                { Guid.Parse("11111111-1111-1111-1111-111111111111"), "Red Dirt Venue - Main Stage", "Concert", 5000, "Flagship Texas-inspired main stage with lighting and crowd energy.", null, true, DateTime.UtcNow },
                { Guid.Parse("22222222-2222-2222-2222-222222222222"), "Red Dirt Venue - Festival Grounds", "Festival", 25000, "Wide festival grounds built for large-scale country performances.", null, true, DateTime.UtcNow },
                { Guid.Parse("33333333-3333-3333-3333-333333333333"), "Red Dirt Venue - Acoustic Room", "Acoustic", 500, "Intimate venue for acoustic songwriter moments.", null, true, DateTime.UtcNow }
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "storyboards");
        migrationBuilder.DropTable(name: "ai_jobs");
        migrationBuilder.DropTable(name: "subscriptions");
        migrationBuilder.DropTable(name: "projects");
        migrationBuilder.DropTable(name: "lyrics");
        migrationBuilder.DropTable(name: "songs");
        migrationBuilder.DropTable(name: "payments");
        migrationBuilder.DropTable(name: "audit_logs");
        migrationBuilder.DropTable(name: "assets");
        migrationBuilder.DropTable(name: "subscription_plans");
        migrationBuilder.DropTable(name: "artists");
        migrationBuilder.DropTable(name: "venues");
        migrationBuilder.DropTable(name: "users");
    }
}
