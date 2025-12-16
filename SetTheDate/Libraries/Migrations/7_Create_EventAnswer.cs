using FluentMigrator;

namespace SetTheDate.Migrations
{
    [Migration(2025113007)]
    public class Create_EventAnswer : Migration
    {
        public override void Up()
        {
            Create.Table("EventAnswer")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Answer").AsString(1000).NotNullable()
                .WithColumn("AnswerKeyword").AsInt32().NotNullable()
                .WithColumn("EventQuestionId").AsInt32().NotNullable()
                .WithColumn("EventId").AsInt32().NotNullable();

            // Optional FKs
            if (Schema.Table("EventQuestion").Exists())
            {
                Create.ForeignKey("FK_EventAnswer_EventQuestion")
                    .FromTable("EventAnswer").ForeignColumn("EventQuestionId")
                    .ToTable("EventQuestion").PrimaryColumn("Id");
            }

            if (Schema.Table("UserEvent").Exists())
            {
                Create.ForeignKey("FK_EventAnswer_UserEvent")
                    .FromTable("EventAnswer").ForeignColumn("EventId")
                    .ToTable("UserEvent").PrimaryColumn("Id");
            }
        }

        public override void Down()
        {
            Delete.Table("EventAnswer");
        }
    }
}
