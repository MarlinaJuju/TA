namespace Showvey.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accesses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        PermissionId = c.Guid(nullable: false),
                        IsGranted = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permissions", t => t.PermissionId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Username = c.String(),
                        Gender = c.String(),
                        CityId = c.Guid(),
                        PhoneNumber = c.String(),
                        Birthdate = c.DateTime(nullable: false),
                        LastLogin = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsBlocked = c.Boolean(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        IsComplete = c.Boolean(nullable: false),
                        PasswordToken = c.String(),
                        TokenActivated = c.Boolean(nullable: false),
                        PasswordTokenExpired = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        CountryId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Responses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        QuestionId = c.Guid(nullable: false),
                        ResponseAnswer = c.Guid(nullable: false),
                        RespondentId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Respondents", t => t.RespondentId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.QuestionId)
                .Index(t => t.RespondentId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Content = c.String(),
                        Number = c.Int(nullable: false),
                        QuestionTypeId = c.Guid(nullable: false),
                        TimeLength = c.Time(nullable: false, precision: 7),
                        SurveyId = c.Guid(nullable: false),
                        IsFreeText = c.Boolean(nullable: false),
                        Count = c.Int(nullable: false),
                        IsScale = c.Boolean(nullable: false),
                        FontColor = c.String(),
                        FontSize = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionTypes", t => t.QuestionTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.QuestionTypeId)
                .Index(t => t.SurveyId);
            
            CreateTable(
                "dbo.Animates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        QuestionId = c.Guid(),
                        ImageId = c.Guid(),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        PosX = c.Double(nullable: false),
                        PosY = c.Double(nullable: false),
                        Depth = c.Int(nullable: false),
                        TimeStart = c.Time(nullable: false, precision: 7),
                        TimeEnd = c.Time(nullable: false, precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.ImageId)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .Index(t => t.QuestionId)
                .Index(t => t.ImageId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Location = c.String(),
                        Name = c.String(),
                        ImageTypeId = c.Guid(nullable: false),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ImageTypes", t => t.ImageTypeId, cascadeDelete: true)
                .Index(t => t.ImageTypeId);
            
            CreateTable(
                "dbo.ImageTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.String(),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionAnswers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        QuestionId = c.Guid(nullable: false),
                        Answer = c.String(),
                        OrderNumber = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.QuestionTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        UserId = c.Guid(nullable: false),
                        SurveyTypeId = c.Guid(nullable: false),
                        IsBlock = c.Boolean(nullable: false),
                        Description = c.String(),
                        IsCompleted = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SurveyTypes", t => t.SurveyTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SurveyTypeId);
            
            CreateTable(
                "dbo.SurveyTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Respondents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BrowserName = c.String(),
                        IPAdress = c.String(),
                        IsRegistered = c.Boolean(nullable: false),
                        SurveyId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.String(),
                        Message = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FromId = c.Guid(),
                        ToId = c.Guid(),
                        Message = c.String(),
                        IsRead = c.Boolean(nullable: false),
                        ReceiverDeleted = c.Boolean(nullable: false),
                        ReceiverDeletedDate = c.DateTime(),
                        SenderDeleted = c.Boolean(nullable: false),
                        SenderDeletedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeletionDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        CreatedUserId = c.Guid(),
                        DeletionUserId = c.Guid(),
                        ModifiedUserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Responses", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Responses", "RespondentId", "dbo.Respondents");
            DropForeignKey("dbo.Surveys", "UserId", "dbo.Users");
            DropForeignKey("dbo.Surveys", "SurveyTypeId", "dbo.SurveyTypes");
            DropForeignKey("dbo.Questions", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.Responses", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Questions", "QuestionTypeId", "dbo.QuestionTypes");
            DropForeignKey("dbo.QuestionAnswers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Animates", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Images", "ImageTypeId", "dbo.ImageTypes");
            DropForeignKey("dbo.Animates", "ImageId", "dbo.Images");
            DropForeignKey("dbo.Users", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Cities", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Accesses", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Accesses", "PermissionId", "dbo.Permissions");
            DropIndex("dbo.Surveys", new[] { "SurveyTypeId" });
            DropIndex("dbo.Surveys", new[] { "UserId" });
            DropIndex("dbo.QuestionAnswers", new[] { "QuestionId" });
            DropIndex("dbo.Images", new[] { "ImageTypeId" });
            DropIndex("dbo.Animates", new[] { "ImageId" });
            DropIndex("dbo.Animates", new[] { "QuestionId" });
            DropIndex("dbo.Questions", new[] { "SurveyId" });
            DropIndex("dbo.Questions", new[] { "QuestionTypeId" });
            DropIndex("dbo.Responses", new[] { "User_Id" });
            DropIndex("dbo.Responses", new[] { "RespondentId" });
            DropIndex("dbo.Responses", new[] { "QuestionId" });
            DropIndex("dbo.Cities", new[] { "CountryId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Users", new[] { "CityId" });
            DropIndex("dbo.Accesses", new[] { "PermissionId" });
            DropIndex("dbo.Accesses", new[] { "RoleId" });
            DropTable("dbo.Notifications");
            DropTable("dbo.Logs");
            DropTable("dbo.Respondents");
            DropTable("dbo.SurveyTypes");
            DropTable("dbo.Surveys");
            DropTable("dbo.QuestionTypes");
            DropTable("dbo.QuestionAnswers");
            DropTable("dbo.ImageTypes");
            DropTable("dbo.Images");
            DropTable("dbo.Animates");
            DropTable("dbo.Questions");
            DropTable("dbo.Responses");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Permissions");
            DropTable("dbo.Accesses");
        }
    }
}
