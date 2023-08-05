using System;
using ASP.NET_RazorPage_P8.Models;
using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NET_RazorPage_P8.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles", x => x.Id);
                });
            // Insert data -> khi ta muốn DbContext tạo ra database và update lên DB gốc
            // mà kèm theo dữ liệu được thêm vào của các table tương ứng

            // --------- Seed Database với thư viện Bogus -----------
            // Fake Data: Dữ liệu giả định

            Randomizer.Seed = new Random(8675309);

            var fakerArticle = new Faker<Article>();
            fakerArticle.RuleFor(a => a.Title, f => f.Lorem.Sentence(5, 5));
            fakerArticle.RuleFor(a => a.Created, f => f.Date.Between(new DateTime(2021, 1, 1), new DateTime(2023, 1, 1)));
            fakerArticle.RuleFor(a => a.Content, f => f.Lorem.Paragraphs(1, 4));

            for (int i = 0; i < 150; i++)
            {
                Article article = fakerArticle.Generate();

                migrationBuilder.InsertData(
                    table: "articles",
                    columns: new[] { "Title", "Created", "Content" },
                    values: new object[]
                    {
                    article.Title,
                    article.Created,
                    article.Content
                    }
                );
            }    
            

            //migrationBuilder.InsertData(
            //    table: "articles",
            //    columns: new[] { "Title", "Created", "Content" },
            //    values: new object[]
            //    {
            //        "Bai viet 2",
            //        new DateTime(2023, 11, 16),
            //        "Noi dung 2"
            //    }
            //);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articles");
        }
    }
}
