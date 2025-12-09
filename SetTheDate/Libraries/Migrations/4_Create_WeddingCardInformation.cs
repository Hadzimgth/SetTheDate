using FluentMigrator;

namespace SetTheDate.Migrations
{
    [Migration(2025113004)]
    public class Create_WeddingCardInformation : Migration
    {
        public override void Up()
        {
            Create.Table("WeddingCardInformation")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserEventId").AsInt32().NotNullable()
                .WithColumn("GroomName").AsString(200).Nullable()
                .WithColumn("BrideName").AsString(200).Nullable()
                .WithColumn("GroomFatherName").AsString(200).Nullable()
                .WithColumn("GroomMotherName").AsString(200).Nullable()
                .WithColumn("BrideFatherName").AsString(200).Nullable()
                .WithColumn("BrideMotherName").AsString(200).Nullable()
                .WithColumn("Wishes").AsString(1000).Nullable()
                .WithColumn("Address1").AsString(500).Nullable()
                .WithColumn("Address2").AsString(500).Nullable()
                .WithColumn("Address3").AsString(500).Nullable()
                .WithColumn("Postcode").AsString(20).Nullable()
                .WithColumn("State").AsString(100).Nullable()
                .WithColumn("WeddingCardType").AsInt32().NotNullable()
                .WithColumn("LocationName").AsString(200).Nullable()
                .WithColumn("TimeFrom").AsDateTime().NotNullable()
                .WithColumn("TimeTo").AsDateTime().NotNullable();

            // Optional FK to UserEvent
            if (Schema.Table("UserEvent").Exists())
            {
                Create.ForeignKey("FK_WeddingCardInformation_UserEvent")
                    .FromTable("WeddingCardInformation").ForeignColumn("UserEventId")
                    .ToTable("UserEvent").PrimaryColumn("Id");
            }
        }

        public override void Down()
        {
            Delete.Table("WeddingCardInformation");
        }
    }
}
