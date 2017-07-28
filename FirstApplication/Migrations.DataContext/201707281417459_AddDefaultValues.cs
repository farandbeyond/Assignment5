namespace FirstApplication.Migrations.DataContext
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class AddDefaultValues : DbMigration
    {
        public override void Up()
        {
            List<string> tables = new List<string>();

            tables.Add("Rating");
            tables.Add("Game");
            tables.Add("Genre");
            tables.Add("GameGenre");
            foreach(string table in tables)
            {
                string tableName = String.Format("dbo.{0}s", table);
                string primaryKey = String.Format("{0}Id", table);

                AlterColumn(tableName, primaryKey, x => x.String(nullable: false, maxLength: 128, defaultValueSql: "newId()"));
                AlterColumn(tableName, "CreateDate", c => c.DateTime(nullable: false, defaultValueSql: "getutcdate()"));
                AlterColumn(tableName, "EditDate", c => c.DateTime(nullable: false, defaultValueSql: "getutcdate()"));

            }
        }
        
        public override void Down()
        {
        }
    }
}
