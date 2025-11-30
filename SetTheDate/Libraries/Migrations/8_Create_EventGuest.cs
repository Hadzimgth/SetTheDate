using FluentMigrator;

namespace SetTheDate.Migrations
{
    [Migration(2025113008)]
    public class Create_EventGuest : Migration
    {
        public override void Up()
        {
            Create.Table("EventGuest")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("GuestName").AsString(200).NotNullable()
                .WithColumn("PhoneNumber").AsString(50).Nullable()
                .WithColumn("UserEventId").AsInt32().NotNullable();

            // Optional FK to UserEvent
            if (Schema.Table("UserEvent").Exists())
            {
                Create.ForeignKey("FK_EventGuest_UserEvent")
                    .FromTable("EventGuest").ForeignColumn("UserEventId")
                    .ToTable("UserEvent").PrimaryColumn("Id");
            }
        }

        public override void Down()
        {
            Delete.Table("EventGuest");
        }
    }
}
