using FluentMigrator;

namespace SetTheDate.Migrations
{
    [Migration(2025113010)]
    public class Create_EventGuestAnswer : Migration
    {
        public override void Up()
        {
            Create.Table("EventGuestAnswer")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("EventGuestId").AsInt32().NotNullable()
                .WithColumn("EventQuestionId").AsInt32().NotNullable()
                .WithColumn("EventAnswerId").AsInt32().NotNullable()
                .WithColumn("EventId").AsInt32().NotNullable();

            // Optional FKs
            if (Schema.Table("EventGuest").Exists())
            {
                Create.ForeignKey("FK_EventGuestAnswer_EventGuest")
                    .FromTable("EventGuestAnswer").ForeignColumn("EventGuestId")
                    .ToTable("EventGuest").PrimaryColumn("Id");
            }

            if (Schema.Table("EventQuestion").Exists())
            {
                Create.ForeignKey("FK_EventGuestAnswer_EventQuestion")
                    .FromTable("EventGuestAnswer").ForeignColumn("EventQuestionId")
                    .ToTable("EventQuestion").PrimaryColumn("Id");
            }

            if (Schema.Table("UserEvent").Exists())
            {
                Create.ForeignKey("FK_EventGuestAnswer_UserEvent")
                    .FromTable("EventGuestAnswer").ForeignColumn("EventId")
                    .ToTable("UserEvent").PrimaryColumn("Id");
            }
        }

        public override void Down()
        {
            Delete.Table("EventGuestAnswer");
        }
    }
}
