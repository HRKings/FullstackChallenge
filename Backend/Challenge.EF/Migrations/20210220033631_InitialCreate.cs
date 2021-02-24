using Microsoft.EntityFrameworkCore.Migrations;

namespace Challenge.EF.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				"course",
				table => new
				{
					id = table.Column<int>("integer", nullable: false,
						defaultValueSql: "nextval('course_id_seq'::regclass)"),
					name = table.Column<string>("character varying(80)", maxLength: 80, nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_course", x => x.id); });

			migrationBuilder.CreateTable(
				"student",
				table => new
				{
					id = table.Column<int>("integer", nullable: false,
						defaultValueSql: "nextval('student_id_seq'::regclass)"),
					name = table.Column<string>("character varying(80)", maxLength: 80, nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_student", x => x.id); });

			migrationBuilder.CreateTable(
				"teacher",
				table => new
				{
					id = table.Column<int>("integer", nullable: false,
						defaultValueSql: "nextval('teacher_id_seq'::regclass)"),
					name = table.Column<string>("character varying(80)", maxLength: 80, nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_teacher", x => x.id); });

			migrationBuilder.CreateTable(
				"_attends",
				table => new
				{
					id = table.Column<int>("integer", nullable: false,
						defaultValueSql: "nextval('attends_id_seq'::regclass)"),
					course_id = table.Column<int>("integer", nullable: false),
					student_id = table.Column<int>("integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK__attends", x => x.id);
					table.ForeignKey(
						"FK__course",
						x => x.course_id,
						"course",
						"id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						"FK_attends_student",
						x => x.student_id,
						"student",
						"id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				"_teaches",
				table => new
				{
					id = table.Column<int>("integer", nullable: false,
						defaultValueSql: "nextval('_teaches_id_seq'::regclass)"),
					teacher_id = table.Column<int>("integer", nullable: false),
					course_id = table.Column<int>("integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK__teaches", x => x.id);
					table.ForeignKey(
						"FK__course",
						x => x.course_id,
						"course",
						"id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						"FK__teacher",
						x => x.teacher_id,
						"teacher",
						"id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				"IX__attends_course_id",
				"_attends",
				"course_id");

			migrationBuilder.CreateIndex(
				"IX__attends_student_id",
				"_attends",
				"student_id");

			migrationBuilder.CreateIndex(
				"IX__teaches_course_id",
				"_teaches",
				"course_id");

			migrationBuilder.CreateIndex(
				"IX__teaches_teacher_id",
				"_teaches",
				"teacher_id");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				"_attends");

			migrationBuilder.DropTable(
				"_teaches");

			migrationBuilder.DropTable(
				"student");

			migrationBuilder.DropTable(
				"course");

			migrationBuilder.DropTable(
				"teacher");
		}
	}
}