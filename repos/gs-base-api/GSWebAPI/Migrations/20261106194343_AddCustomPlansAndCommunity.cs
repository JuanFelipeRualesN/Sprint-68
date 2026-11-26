using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GSWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomPlansAndCommunity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "community_forum_categories",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    moderators_only = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_forum_categories", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "community_groups",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    community_name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    community_location = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    community_description = table.Column<string>(type: "text", nullable: true),
                    created_by_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    visibility = table.Column<string>(type: "enum('public','private')", nullable: false, defaultValue: "public"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_groups", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "curriculum_frameworks",
                columns: table => new
                {
                    id = table.Column<ushort>(type: "smallint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    country_code = table.Column<string>(type: "char(2)", nullable: false),
                    region = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_curriculum_frameworks", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "custom_plan_types",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_plan_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "meeting_formats",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    code = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meeting_formats", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "community_forums",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    community_group_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    community_forum_category_id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    title = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_pinned = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    is_locked = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_forums", x => x.id);
                    table.ForeignKey(
                        name: "FK_community_forums_community_forum_categories_community_forum_~",
                        column: x => x.community_forum_category_id,
                        principalTable: "community_forum_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_community_forums_community_groups_community_group_id",
                        column: x => x.community_group_id,
                        principalTable: "community_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "community_members",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    community_group_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    role = table.Column<string>(type: "enum('owner','moderator','member')", nullable: false, defaultValue: "member"),
                    status = table.Column<string>(type: "enum('pending','active','banned')", nullable: false, defaultValue: "active"),
                    joined_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_members", x => x.id);
                    table.ForeignKey(
                        name: "FK_community_members_community_groups_community_group_id",
                        column: x => x.community_group_id,
                        principalTable: "community_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "community_resources",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    community_group_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    author_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    title = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    resource_type = table.Column<string>(type: "enum('link','document','video')", nullable: false, defaultValue: "link"),
                    link = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    published_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_resources", x => x.id);
                    table.ForeignKey(
                        name: "FK_community_resources_community_groups_community_group_id",
                        column: x => x.community_group_id,
                        principalTable: "community_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "custom_plan_client",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    custom_plan_type_id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    client_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    student_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    level_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    curriculum_framework_id = table.Column<ushort>(type: "smallint unsigned", nullable: true),
                    plan_duration_weeks = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "enum('draft','active','paused','completed','cancelled')", nullable: false, defaultValue: "draft"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_plan_client", x => x.id);
                    table.ForeignKey(
                        name: "FK_custom_plan_client_curriculum_frameworks_curriculum_framewor~",
                        column: x => x.curriculum_framework_id,
                        principalTable: "curriculum_frameworks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_custom_plan_client_custom_plan_types_custom_plan_type_id",
                        column: x => x.custom_plan_type_id,
                        principalTable: "custom_plan_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "community_events",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    community_group_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    organizer_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    event_title = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    event_description = table.Column<string>(type: "text", nullable: true),
                    meeting_format_id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    event_location = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    event_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    event_end_date_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    event_timezone = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    event_availability = table.Column<string>(type: "enum('open','closed','cancelled')", nullable: false, defaultValue: "open"),
                    event_max_allowed_persons = table.Column<ushort>(type: "smallint unsigned", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_events", x => x.id);
                    table.CheckConstraint("CK_community_events_end_after_start", "`event_end_date_time` IS NULL OR `event_end_date_time` > `event_date_time`");
                    table.ForeignKey(
                        name: "FK_community_events_community_groups_community_group_id",
                        column: x => x.community_group_id,
                        principalTable: "community_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_community_events_meeting_formats_meeting_format_id",
                        column: x => x.meeting_format_id,
                        principalTable: "meeting_formats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "community_forum_posts",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    community_forum_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    parent_post_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    post_text = table.Column<string>(type: "text", nullable: false),
                    edited_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_forum_posts", x => x.id);
                    table.ForeignKey(
                        name: "FK_community_forum_posts_community_forum_posts_parent_post_id",
                        column: x => x.parent_post_id,
                        principalTable: "community_forum_posts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_community_forum_posts_community_forums_community_forum_id",
                        column: x => x.community_forum_id,
                        principalTable: "community_forums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "custom_plan_client_days",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    custom_plan_client_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    day_of_week = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_plan_client_days", x => x.id);
                    table.CheckConstraint("CK_custom_plan_client_days_day_of_week", "`day_of_week` BETWEEN 1 AND 7");
                    table.ForeignKey(
                        name: "FK_custom_plan_client_days_custom_plan_client_custom_plan_clien~",
                        column: x => x.custom_plan_client_id,
                        principalTable: "custom_plan_client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "custom_plan_client_lessons",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    custom_plan_client_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    lesson_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    standard_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    subject_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    sequence = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    learning_assigned_date = table.Column<DateOnly>(type: "date", nullable: false),
                    learning_due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    learning_timezone = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    learning_completed_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    lesson_status = table.Column<string>(type: "enum('not_started','in_progress','completed','skipped')", nullable: false, defaultValue: "not_started"),
                    concept_completed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    definitions_completed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    examples_completed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    pre_completed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    practices_completed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    test_completed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    post_completed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    completed_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_plan_client_lessons", x => x.id);
                    table.CheckConstraint("CK_custom_plan_client_lessons_learning_dates", "`learning_due_date` >= `learning_assigned_date`");
                    table.ForeignKey(
                        name: "FK_custom_plan_client_lessons_custom_plan_client_custom_plan_cl~",
                        column: x => x.custom_plan_client_id,
                        principalTable: "custom_plan_client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "custom_plan_client_subjects",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    custom_plan_client_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    subject_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_plan_client_subjects", x => x.id);
                    table.ForeignKey(
                        name: "FK_custom_plan_client_subjects_custom_plan_client_custom_plan_c~",
                        column: x => x.custom_plan_client_id,
                        principalTable: "custom_plan_client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "community_events_interests",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    community_event_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    response = table.Column<string>(type: "enum('interested','going','not_going')", nullable: false, defaultValue: "going"),
                    quantity_of_persons_assisting = table.Column<byte>(type: "tinyint unsigned", nullable: false, defaultValue: (byte)1),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_events_interests", x => x.id);
                    table.CheckConstraint("CK_community_events_interests_quantity", "`quantity_of_persons_assisting` >= 1");
                    table.ForeignKey(
                        name: "FK_community_events_interests_community_events_community_event_~",
                        column: x => x.community_event_id,
                        principalTable: "community_events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "custom_plan_client_day_times",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    custom_plan_client_day_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    duration_minutes = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    timezone = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_plan_client_day_times", x => x.id);
                    table.ForeignKey(
                        name: "FK_custom_plan_client_day_times_custom_plan_client_days_custom_~",
                        column: x => x.custom_plan_client_day_id,
                        principalTable: "custom_plan_client_days",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "custom_plan_client_lesson_dates",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    custom_plan_client_lesson_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    assigned_date = table.Column<DateOnly>(type: "date", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_plan_client_lesson_dates", x => x.id);
                    table.ForeignKey(
                        name: "FK_custom_plan_client_lesson_dates_custom_plan_client_lessons_c~",
                        column: x => x.custom_plan_client_lesson_id,
                        principalTable: "custom_plan_client_lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "custom_plan_client_lesson_exam_dates",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    custom_plan_client_lesson_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    exam_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    assigned_date = table.Column<DateOnly>(type: "date", nullable: false),
                    due_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    timezone = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    duration_minutes = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    gwp_user_result_id = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "enum('not_started','in_progress','completed')", nullable: false, defaultValue: "not_started"),
                    completed_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_plan_client_lesson_exam_dates", x => x.id);
                    table.CheckConstraint("CK_custom_plan_client_lesson_exam_dates_duration", "`duration_minutes` BETWEEN 1 AND 480");
                    table.ForeignKey(
                        name: "FK_cpc_lesson_exam_dates_usqui_user_quiz_results",
                        column: x => x.gwp_user_result_id,
                        principalTable: "usqui_user_quiz_results",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_custom_plan_client_lesson_exam_dates_custom_plan_client_less~",
                        column: x => x.custom_plan_client_lesson_id,
                        principalTable: "custom_plan_client_lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "custom_plan_client_lesson_practices",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    custom_plan_client_lesson_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    practice_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    sequence = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    assigned_date = table.Column<DateOnly>(type: "date", nullable: false),
                    due_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    timezone = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    status = table.Column<string>(type: "enum('not_started','in_progress','completed')", nullable: false, defaultValue: "not_started"),
                    score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    best_score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    total_correct = table.Column<ushort>(type: "smallint unsigned", nullable: true),
                    total_questions = table.Column<ushort>(type: "smallint unsigned", nullable: true),
                    attempts = table.Column<ushort>(type: "smallint unsigned", nullable: false, defaultValue: (ushort)0),
                    last_attempt_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    completed_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_plan_client_lesson_practices", x => x.id);
                    table.CheckConstraint("CK_custom_plan_client_lesson_practices_best_score", "`best_score` IS NULL OR `best_score` BETWEEN 0 AND 100");
                    table.CheckConstraint("CK_custom_plan_client_lesson_practices_score", "`score` IS NULL OR `score` BETWEEN 0 AND 100");
                    table.ForeignKey(
                        name: "FK_custom_plan_client_lesson_practices_custom_plan_client_lesso~",
                        column: x => x.custom_plan_client_lesson_id,
                        principalTable: "custom_plan_client_lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "custom_plan_client_lesson_tests",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    custom_plan_client_lesson_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    test_type = table.Column<string>(type: "enum('pre_test','post_test')", nullable: false),
                    test_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    assigned_date = table.Column<DateOnly>(type: "date", nullable: false),
                    due_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    timezone = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    duration_minutes = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    status = table.Column<string>(type: "enum('not_started','in_progress','completed')", nullable: false, defaultValue: "not_started"),
                    gwp_user_result_id = table.Column<int>(type: "int", nullable: true),
                    completed_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_plan_client_lesson_tests", x => x.id);
                    table.CheckConstraint("CK_custom_plan_client_lesson_tests_duration", "`duration_minutes` BETWEEN 1 AND 480");
                    table.ForeignKey(
                        name: "FK_cpc_lesson_tests_usqui_user_quiz_results",
                        column: x => x.gwp_user_result_id,
                        principalTable: "usqui_user_quiz_results",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_custom_plan_client_lesson_tests_custom_plan_client_lessons_c~",
                        column: x => x.custom_plan_client_lesson_id,
                        principalTable: "custom_plan_client_lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "custom_plan_evidences",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    custom_plan_client_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    custom_plan_client_lesson_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    evidence_type = table.Column<string>(type: "enum('evidence','project')", nullable: false, defaultValue: "evidence"),
                    title = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    blob_name = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    file_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    content_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    size_bytes = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    uploaded_by_user_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_plan_evidences", x => x.id);
                    table.ForeignKey(
                        name: "FK_custom_plan_evidences_custom_plan_client_custom_plan_client_~",
                        column: x => x.custom_plan_client_id,
                        principalTable: "custom_plan_client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_custom_plan_evidences_custom_plan_client_lessons_custom_plan~",
                        column: x => x.custom_plan_client_lesson_id,
                        principalTable: "custom_plan_client_lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.InsertData(
                table: "community_forum_categories",
                columns: new[] { "id", "code", "created_at", "name", "updated_at" },
                values: new object[,]
                {
                    { (byte)1, "general", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), "General", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { (byte)2, "questions", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Questions", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "community_forum_categories",
                columns: new[] { "id", "code", "created_at", "moderators_only", "name", "updated_at" },
                values: new object[] { (byte)3, "announcements", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), true, "Announcements", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "community_forum_categories",
                columns: new[] { "id", "code", "created_at", "name", "updated_at" },
                values: new object[,]
                {
                    { (byte)4, "resources", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Resources", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { (byte)5, "events", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Events", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "curriculum_frameworks",
                columns: new[] { "id", "code", "country_code", "created_at", "is_active", "name", "region", "updated_at" },
                values: new object[] { (ushort)1, "DEPR", "PR", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), true, "Departamento de Educación de Puerto Rico", null, new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "custom_plan_types",
                columns: new[] { "id", "code", "created_at", "name", "updated_at" },
                values: new object[,]
                {
                    { (byte)1, "homeschool", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Homeschool", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { (byte)2, "reinforcements", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Reinforcements", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { (byte)3, "specific_task", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Specific task", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { (byte)4, "customized", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Customized", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "meeting_formats",
                columns: new[] { "id", "code", "created_at", "name", "updated_at" },
                values: new object[,]
                {
                    { (byte)1, "online", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Online", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { (byte)2, "in_person", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), "In person", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_community_events_community_group_id_event_date_time",
                table: "community_events",
                columns: new[] { "community_group_id", "event_date_time" });

            migrationBuilder.CreateIndex(
                name: "IX_community_events_meeting_format_id",
                table: "community_events",
                column: "meeting_format_id");

            migrationBuilder.CreateIndex(
                name: "IX_community_events_organizer_user_id",
                table: "community_events",
                column: "organizer_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_community_events_interests_community_event_id_user_id",
                table: "community_events_interests",
                columns: new[] { "community_event_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_community_events_interests_user_id",
                table: "community_events_interests",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_community_forum_categories_code",
                table: "community_forum_categories",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_community_forum_posts_community_forum_id_created_at",
                table: "community_forum_posts",
                columns: new[] { "community_forum_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_community_forum_posts_parent_post_id",
                table: "community_forum_posts",
                column: "parent_post_id");

            migrationBuilder.CreateIndex(
                name: "IX_community_forum_posts_user_id",
                table: "community_forum_posts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_community_forums_community_forum_category_id",
                table: "community_forums",
                column: "community_forum_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_community_forums_community_group_id_is_pinned",
                table: "community_forums",
                columns: new[] { "community_group_id", "is_pinned" });

            migrationBuilder.CreateIndex(
                name: "IX_community_forums_user_id",
                table: "community_forums",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_community_groups_created_by_user_id",
                table: "community_groups",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_community_members_community_group_id_user_id",
                table: "community_members",
                columns: new[] { "community_group_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_community_members_user_id",
                table: "community_members",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_community_resources_author_id",
                table: "community_resources",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_community_resources_community_group_id",
                table: "community_resources",
                column: "community_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_curriculum_frameworks_code",
                table: "curriculum_frameworks",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_client_id",
                table: "custom_plan_client",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_curriculum_framework_id",
                table: "custom_plan_client",
                column: "curriculum_framework_id");

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_custom_plan_type_id",
                table: "custom_plan_client",
                column: "custom_plan_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_student_id_status",
                table: "custom_plan_client",
                columns: new[] { "student_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_day_times_custom_plan_client_day_id_start~",
                table: "custom_plan_client_day_times",
                columns: new[] { "custom_plan_client_day_id", "start_time" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_days_custom_plan_client_id_day_of_week",
                table: "custom_plan_client_days",
                columns: new[] { "custom_plan_client_id", "day_of_week" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lesson_dates_custom_plan_client_lesson_id~",
                table: "custom_plan_client_lesson_dates",
                columns: new[] { "custom_plan_client_lesson_id", "assigned_date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lesson_exam_dates_custom_plan_client_less~",
                table: "custom_plan_client_lesson_exam_dates",
                columns: new[] { "custom_plan_client_lesson_id", "exam_id", "assigned_date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lesson_exam_dates_due_date_time",
                table: "custom_plan_client_lesson_exam_dates",
                column: "due_date_time");

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lesson_exam_dates_gwp_user_result_id",
                table: "custom_plan_client_lesson_exam_dates",
                column: "gwp_user_result_id");

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lesson_practices_custom_plan_client_lesso~",
                table: "custom_plan_client_lesson_practices",
                columns: new[] { "custom_plan_client_lesson_id", "practice_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lesson_practices_due_date_time",
                table: "custom_plan_client_lesson_practices",
                column: "due_date_time");

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lesson_tests_custom_plan_client_lesson_id~",
                table: "custom_plan_client_lesson_tests",
                columns: new[] { "custom_plan_client_lesson_id", "test_type", "test_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lesson_tests_due_date_time",
                table: "custom_plan_client_lesson_tests",
                column: "due_date_time");

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lesson_tests_gwp_user_result_id",
                table: "custom_plan_client_lesson_tests",
                column: "gwp_user_result_id");

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lessons_custom_plan_client_id_learning_du~",
                table: "custom_plan_client_lessons",
                columns: new[] { "custom_plan_client_id", "learning_due_date" });

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lessons_custom_plan_client_id_lesson_id",
                table: "custom_plan_client_lessons",
                columns: new[] { "custom_plan_client_id", "lesson_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lessons_custom_plan_client_id_sequence",
                table: "custom_plan_client_lessons",
                columns: new[] { "custom_plan_client_id", "sequence" });

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_lessons_custom_plan_client_id_subject_id",
                table: "custom_plan_client_lessons",
                columns: new[] { "custom_plan_client_id", "subject_id" });

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_client_subjects_custom_plan_client_id_subject_id",
                table: "custom_plan_client_subjects",
                columns: new[] { "custom_plan_client_id", "subject_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_evidences_custom_plan_client_id_custom_plan_clie~",
                table: "custom_plan_evidences",
                columns: new[] { "custom_plan_client_id", "custom_plan_client_lesson_id" });

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_evidences_custom_plan_client_lesson_id",
                table: "custom_plan_evidences",
                column: "custom_plan_client_lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_evidences_uploaded_by_user_id",
                table: "custom_plan_evidences",
                column: "uploaded_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_custom_plan_types_code",
                table: "custom_plan_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_meeting_formats_code",
                table: "meeting_formats",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "community_events_interests");

            migrationBuilder.DropTable(
                name: "community_forum_posts");

            migrationBuilder.DropTable(
                name: "community_members");

            migrationBuilder.DropTable(
                name: "community_resources");

            migrationBuilder.DropTable(
                name: "custom_plan_client_day_times");

            migrationBuilder.DropTable(
                name: "custom_plan_client_lesson_dates");

            migrationBuilder.DropTable(
                name: "custom_plan_client_lesson_exam_dates");

            migrationBuilder.DropTable(
                name: "custom_plan_client_lesson_practices");

            migrationBuilder.DropTable(
                name: "custom_plan_client_lesson_tests");

            migrationBuilder.DropTable(
                name: "custom_plan_client_subjects");

            migrationBuilder.DropTable(
                name: "custom_plan_evidences");

            migrationBuilder.DropTable(
                name: "community_events");

            migrationBuilder.DropTable(
                name: "community_forums");

            migrationBuilder.DropTable(
                name: "custom_plan_client_days");

            migrationBuilder.DropTable(
                name: "custom_plan_client_lessons");

            migrationBuilder.DropTable(
                name: "meeting_formats");

            migrationBuilder.DropTable(
                name: "community_forum_categories");

            migrationBuilder.DropTable(
                name: "community_groups");

            migrationBuilder.DropTable(
                name: "custom_plan_client");

            migrationBuilder.DropTable(
                name: "curriculum_frameworks");

            migrationBuilder.DropTable(
                name: "custom_plan_types");
        }
    }
}
