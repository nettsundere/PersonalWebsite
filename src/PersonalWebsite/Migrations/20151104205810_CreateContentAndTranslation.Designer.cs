using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using PersonalWebsite.Models;

namespace PersonalWebsite.Migrations
{
    [ContextType(typeof(DataDbContext))]
    partial class CreateContentAndTranslation
    {
        public override string Id
        {
            get { return "20151104205810_CreateContentAndTranslation"; }
        }
        
        public override string ProductVersion
        {
            get { return "7.0.0-beta5-13549"; }
        }
        
        public override void BuildTargetModel(ModelBuilder builder)
        {
            builder
                .Annotation("SqlServer:DefaultSequenceName", "DefaultSequence")
                .Annotation("SqlServer:Sequence:.DefaultSequence", "'DefaultSequence', '', '1', '10', '', '', 'Int64', 'False'")
                .Annotation("SqlServer:ValueGeneration", "Sequence");
            
            builder.Entity("PersonalWebsite.Models.Content", b =>
                {
                    b.Property<Guid>("ContentGuid")
                        .GenerateValueOnAdd()
                        .Annotation("Relational:ColumnDefaultExpression", "newsequentialid()");
                    
                    b.Property<int>("ContentId")
                        .GenerateValueOnAdd();
                    
                    b.Property<string>("InternalCaption")
                        .Required()
                        .Annotation("MaxLength", 255);
                    
                    b.Key("ContentGuid");
                });
            
            builder.Entity("PersonalWebsite.Models.Translation", b =>
                {
                    b.Property<int>("Id")
                        .GenerateValueOnAdd()
                        .StoreGeneratedPattern(StoreGeneratedPattern.Identity);
                    
                    b.Property<int>("ContentId");
                    
                    b.Property<string>("ContentMarkup")
                        .Required();
                    
                    b.Property<string>("Description")
                        .Required();
                    
                    b.Property<int>("State");
                    
                    b.Property<string>("Title")
                        .Required();
                    
                    b.Property<DateTime>("UpdatedAt");
                    
                    b.Property<int>("Version");
                    
                    b.Key("Id");
                    
                    b.Index("Version", "ContentId");
                });
            
            builder.Entity("PersonalWebsite.Models.Translation", b =>
                {
                    b.Reference("PersonalWebsite.Models.Content")
                        .InverseCollection()
                        .ForeignKey("ContentId")
                        .PrincipalKey("ContentId");
                });
        }
    }
}
