using FluentMigrator;

namespace SetTheDate.Migrations
{
    [Migration(2025113003)]
    public class Create_UserEvent : Migration
    {
        public override void Up()
        {
            Create.Table("UserEvent")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("EventName").AsString(200).NotNullable()
                .WithColumn("EventDescription").AsString(1000).Nullable()
                .WithColumn("EventDate").AsDateTime().NotNullable()
                .WithColumn("EndDate").AsDateTime().NotNullable()
                .WithColumn("PurgeDate").AsDateTime().NotNullable()
                .WithColumn("Completed").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("PaymentInformationId").AsInt32().NotNullable();

            if (Schema.Table("PaymentInformation").Exists())
            {
                Create.ForeignKey("FK_UserEvent_PaymentInformation")
                    .FromTable("UserEvent").ForeignColumn("PaymentInformationId")
                    .ToTable("PaymentInformation").PrimaryColumn("Id");
            }

            if (Schema.Table("User").Exists())
            {
                Create.ForeignKey("FK_UserEvent_User")
                    .FromTable("UserEvent").ForeignColumn("UserId")
                    .ToTable("User").PrimaryColumn("Id");
            }
        }

        public override void Down()
        {
            Delete.Table("UserEvent");
        }
    }
}