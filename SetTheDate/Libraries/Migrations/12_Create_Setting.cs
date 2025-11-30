using FluentMigrator;

namespace SetTheDate.Migrations
{
    [Migration(2025113012)]
    public class Create_Setting : Migration
    {
        public override void Up()
        {
            Create.Table("Setting")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(200).NotNullable()
                .WithColumn("Value").AsString(1000).Nullable();
        }

        public override void Down()
        {
            Delete.Table("Setting");
        }
    }
}
