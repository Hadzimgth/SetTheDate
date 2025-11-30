using FluentMigrator;

namespace SetTheDate.Migrations
{
    [Migration(2025113001)]
    public class Create_User : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Email").AsString(200).NotNullable()
                .WithColumn("Password").AsString(500).NotNullable()
                .WithColumn("PhoneNumber").AsString(50).Nullable()
                .WithColumn("Verified").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsAdmin").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Table("User");
        }
    }
}
