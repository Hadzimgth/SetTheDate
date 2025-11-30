using FluentMigrator;

namespace SetTheDate.Migrations
{
    [Migration(2025113005)]
    public class Create_ContactInformation : Migration
    {
        public override void Up()
        {
            Create.Table("ContactInformation")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("WeddingCardInformationId").AsInt32().NotNullable()
                .WithColumn("Name").AsString(200).NotNullable()
                .WithColumn("FamilyRole").AsString(100).Nullable()
                .WithColumn("PhoneNumber").AsString(50).Nullable();

            // Optional FK if referenced table exists
            if (Schema.Table("WeddingCardInformation").Exists())
            {
                Create.ForeignKey("FK_ContactInformation_WeddingCardInformation")
                    .FromTable("ContactInformation").ForeignColumn("WeddingCardInformationId")
                    .ToTable("WeddingCardInformation").PrimaryColumn("Id");
            }
        }

        public override void Down()
        {
            Delete.Table("ContactInformation");
        }
    }
}
