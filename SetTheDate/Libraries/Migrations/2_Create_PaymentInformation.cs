using FluentMigrator;

namespace SetTheDate.Migrations
{
    [Migration(2025113002)]
    public class Create_PaymentInformation : Migration
    {
        public override void Up()
        {
            Create.Table("PaymentInformation")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("PaymentDate").AsDateTime().NotNullable()
                .WithColumn("Amount").AsDecimal(18, 2).NotNullable()
                .WithColumn("PaymentVia").AsString(100).Nullable()
                .WithColumn("Status").AsString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Table("PaymentInformation");
        }
    }
}