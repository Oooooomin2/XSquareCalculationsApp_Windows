namespace XSquareCalculationsApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ATHENTICATES",
                c => new
                    {
                        AUTHENTICATE_ID = c.Int(nullable: false, identity: true),
                        USER_ID = c.Int(nullable: false),
                        ID_TOKEN = c.String(unicode: false, storeType: "text"),
                        EXPIRED_DATETIME = c.DateTime(nullable: false, precision: 0),
                        CREATED_TIME = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.AUTHENTICATE_ID);
            
            CreateTable(
                "dbo.TEMPLATES",
                c => new
                    {
                        TEMPLATE_ID = c.Int(nullable: false, identity: true),
                        TEMPLATE_NAME = c.String(maxLength: 45, unicode: false),
                        TEMPLATE_BLOB = c.Binary(storeType: "mediumblob"),
                        THUMBNAIL = c.String(unicode: false, storeType: "text"),
                        LIKE_COUNT = c.Int(nullable: false),
                        DOWNLOAD_COUNT = c.Int(nullable: false),
                        USER_ID = c.Int(nullable: false),
                        DEL_FLG = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        CREATED_TIME = c.DateTime(nullable: false, precision: 0),
                        UPDATED_TIME = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.TEMPLATE_ID);
            
            CreateTable(
                "dbo.USERS",
                c => new
                    {
                        USER_ID = c.Int(nullable: false, identity: true),
                        USER_NAME = c.String(nullable: false, maxLength: 35, unicode: false),
                        USER_PASSWORD = c.String(nullable: false, unicode: false, storeType: "text"),
                        PASSWORD_SALT = c.String(unicode: false, storeType: "text"),
                        LIKE_NUMBER_SUM = c.Int(nullable: false),
                        DEL_FLG = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        CREATED_TIME = c.DateTime(nullable: false, precision: 0),
                        UPDATED_TIME = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.USER_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.USERS");
            DropTable("dbo.TEMPLATES");
            DropTable("dbo.ATHENTICATES");
        }
    }
}
