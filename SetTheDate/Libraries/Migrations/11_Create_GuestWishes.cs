using FluentMigrator;

namespace SetTheDate.Migrations
{
    [Migration(2025113011)]
    public class Create_GuestWishes : Migration
    {
        public override void Up()
        {
            Create.Table("GuestWishes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("WeddingCardInformationId").AsInt32().NotNullable()
                .WithColumn("Name").AsString(200).Nullable()
                .WithColumn("Wish").AsString(1000).Nullable();

            // Optional FK to WeddingCardInformation
            if (Schema.Table("WeddingCardInformation").Exists())
            {
                Create.ForeignKey("FK_GuestWishes_WeddingCardInformation")
                    .FromTable("GuestWishes").ForeignColumn("WeddingCardInformationId")
                    .ToTable("WeddingCardInformation").PrimaryColumn("Id");
            }
        }

        public override void Down()
        {
            Delete.Table("GuestWishes");
        }
    }
}
