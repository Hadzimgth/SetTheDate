using FluentMigrator;

namespace SetTheDate.Migrations
{
    [Migration(2025113006)]
    public class Create_EventQuestion : Migration
    {
        public override void Up()
        {
            Create.Table("EventQuestion")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Question").AsString(1000).NotNullable()
                .WithColumn("UserEventId").AsInt32().NotNullable();

            // Optional FK to UserEvent
            if (Schema.Table("UserEvent").Exists())
            {
                Create.ForeignKey("FK_EventQuestion_UserEvent")
                    .FromTable("EventQuestion").ForeignColumn("UserEventId")
                    .ToTable("UserEvent").PrimaryColumn("Id");
            }
        }

        public override void Down()
        {
            Delete.Table("EventQuestion");
        }
    }
}
