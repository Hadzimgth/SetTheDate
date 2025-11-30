using FluentMigrator;

namespace SetTheDate.Migrations
{
    [Migration(2025113009)]
    public class Create_EventImageAttachment : Migration
    {
        public override void Up()
        {
            Create.Table("EventImageAttachment")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FileName").AsString(200).NotNullable()
                .WithColumn("Position").AsString(50).Nullable()
                .WithColumn("FilePath").AsString(500).NotNullable()
                .WithColumn("WeddingCardId").AsInt32().NotNullable();

            // Optional FK to WeddingCardInformation
            if (Schema.Table("WeddingCardInformation").Exists())
            {
                Create.ForeignKey("FK_EventImageAttachment_WeddingCardInformation")
                    .FromTable("EventImageAttachment").ForeignColumn("WeddingCardId")
                    .ToTable("WeddingCardInformation").PrimaryColumn("Id");
            }
        }

        public override void Down()
        {
            Delete.Table("EventImageAttachment");
        }
    }
}
