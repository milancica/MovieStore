using System;
using System.Collections.Generic;
using System.Linq;
using MovieStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MovieStore.Repository.DbSeed
{
    public static class SeedData
    {
        public static void Seed(IApplicationBuilder appBuilder)
        {
            DatabaseContext _context = appBuilder
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<DatabaseContext>();

            if (_context.Database.GetPendingMigrations().Any())
                _context.Database.Migrate();


            var anyArticles = _context.Articles.Any();

            if (!anyArticles)
            {
                var ArticleTypes = GetMainArticleTypes();
                _context.ArticleTypes.AddRange(ArticleTypes);

                _context.SaveChanges();

                var ArticlesWithArticleTypes = GetArticles();

                var Articles = ArticlesWithArticleTypes.Select(x => x.Key)
                    .ToList();

                _context.Articles.AddRange(Articles);


                foreach (var Article in ArticlesWithArticleTypes.Keys)
                {
                    var ArticleArticleType = ArticlesWithArticleTypes
                        .Where(x => x.Key == Article)
                        .FirstOrDefault().Value ?? "";

                    Article.ArticleTypeId = _context.ArticleTypes.FirstOrDefault(x => x.Name == ArticleArticleType).ArticleTypeId;
                }

                _context.SaveChanges();
            }
        }

        static List<string> GetTableNames(this DatabaseContext context)
        {
            return new List<string>()
            {
                "Articles", "ArticleTypes", "ArticleArticleTypes"
            };
        }

        static Dictionary<Article, string> GetArticles()
        {
            return new Dictionary<Article, string>()
            {
                {
                    new Article()
                    {
                        Name = "The Vanished",
                        Description = "A Civil War veteran agrees to deliver a girl, taken by the Kiowa people years ago, to her aunt and uncle, against her will. They travel hundreds of miles and face grave dangers as they search for a place that either can call home.",
                        Price = GetRandomPrice()
                    }, 
                    GetRandomArticleType()
                },
                {
                    new Article()
                    {
                        Name = "One Night in Miami",
                        Description = "A fictional account of one incredible night where icons Muhammad Ali, Malcolm X, Sam Cooke, and Jim Brown gathered discussing their roles in the Civil Rights Movement and cultural upheaval of the 60s.",
                        Price = GetRandomPrice()
                    }, 
                    GetRandomArticleType()
                },
                {
                    new Article()
                    {
                        Name = "Dara of Jasenovac",
                        Description = "In the summer of 1942, the family of twelve year old Serbian Dara was taken away and separated into two concentration camps. She is witnessing all the horrors of the Ustashe regime. After her brother and mother are killed, she tries to save the life of her younger brother, hoping that her father is still alive.",
                        Price = GetRandomPrice()
                    }, 
                    GetRandomArticleType()
                },
                {
                    new Article()
                    {
                        Name = "Palm Springs",
                        Description = "When carefree Nyles and reluctant maid of honor Sarah have a chance encounter at a Palm Springs wedding, things get complicated as they are unable to escape the venue, themselves, or each other.",
                        Price = GetRandomPrice()
                    }, 
                    GetRandomArticleType()
                },
                {
                    new Article()
                    {
                        Name = "The Witches",
                        Description = "A young boy and his grandmother have a run-in with a coven of witches and their leader.",
                        Price = GetRandomPrice()
                    }, 
                    GetRandomArticleType()
                }
            };
        }

        private static string GetRandomArticleType()
        {
            var random = new Random();
            var ArticleTypes = GetMainArticleTypes();
            var ArticleTypesCount = ArticleTypes.Count;
            int numRandomArticleType = random.Next(1, ArticleTypesCount);

            return ArticleTypes[numRandomArticleType].Name;
        }

        private static decimal GetRandomPrice()
        {
            var random = new Random();
            var basePrice = (decimal)(random.Next(6, 75) * 1.0);
            var decimalPrice = (decimal)(random.NextDouble() * 99);
            return basePrice + decimalPrice;
        }

        static List<ArticleType> GetMainArticleTypes()
        {
            return new List<ArticleType>()
            {
                new ArticleType() { Name = "Fantasy", Description = "Fantasy" },
                new ArticleType() { Name = "Action", Description = "Action" },
                new ArticleType() { Name = "Mystery", Description = "Mystery" },
                new ArticleType() { Name = "Sci-Fi", Description = "Science fiction" },
            };
        }
    }
}
