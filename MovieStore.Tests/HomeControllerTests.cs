﻿using System;
using Moq;
using Xunit;
using MovieStore.Repository;
using MovieStore.Models;
using System.Linq;
using MovieStore.Controllers;
using MovieStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MovieStore.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Can_Use_Repository()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

            mock.Setup(m => m.Articles).Returns((new Article[]
            {
                new Article { ArticleId = 1, Name = "B1" },
                new Article { ArticleId = 2, Name = "B2" },
            }).AsQueryable());

            HomeController controller = new HomeController(mock.Object);

            var result = (controller.Index(ArticleType: null) as ViewResult)
                .ViewData
                .Model as ArticleListViewModel;

            Article[] ArticleArray = result.Articles.ToArray();

            Assert.True(ArticleArray.Length == 2);
            Assert.Equal("B1", ArticleArray[0].Name);
            Assert.Equal("B2", ArticleArray[1].Name);
        }

        [Fact]
        public void Can_Paginate()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

            mock.Setup(m => m.Articles).Returns((new Article[]
            {
                new Article { ArticleId = 1, Name = "B1" },
                new Article { ArticleId = 2, Name = "B2" },
                new Article { ArticleId = 3, Name = "B3" },
                new Article { ArticleId = 4, Name = "B4" },
                new Article { ArticleId = 5, Name = "B5" },
            }).AsQueryable());

            HomeController controller = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            var result = (controller.Index(2, ArticleType: null) as ViewResult)
                .ViewData
                .Model as ArticleListViewModel;

            Article[] ArticleArray = result.Articles.ToArray();

            Assert.True(ArticleArray.Length == 2);
            Assert.Equal("B4", ArticleArray[0].Name);
            Assert.Equal("B5", ArticleArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

            mock.Setup(m => m.Articles).Returns((new Article[]
            {
                new Article { ArticleId = 1, Name = "B1" },
                new Article { ArticleId = 2, Name = "B2" },
                new Article { ArticleId = 3, Name = "B3" },
                new Article { ArticleId = 4, Name = "B4" },
                new Article { ArticleId = 5, Name = "B5" },
            }).AsQueryable());

            HomeController controller = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            var result = (controller.Index(2, ArticleType: null) as ViewResult)
                .ViewData
                .Model as ArticleListViewModel;

            Article[] ArticleArray = result.Articles.ToArray();

            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Articles()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

            mock.Setup(m => m.Articles).Returns((new Article[]
            {
                new Article { 
                    ArticleId = 1, Name = "B1", ArticleTypeId = 1
                },
                new Article { ArticleId = 2, Name = "B2", ArticleTypeId = 2
                },
                new Article { ArticleId = 3, Name = "B3", ArticleTypeId = 3
                },
                new Article { ArticleId = 4, Name = "B4", ArticleTypeId = 2
                },
                new Article { ArticleId = 5, Name = "B5", ArticleTypeId = 3
                },
            }).AsQueryable());

            HomeController controller = new HomeController(mock.Object)
            {
                PageSize = 3
            };

            var result = (controller.Index(1, ArticleType: 2) as ViewResult)
                .ViewData
                .Model as ArticleListViewModel;

            Article[] ArticleArray = result.Articles.ToArray();

            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, ArticleArray.Length);
            Assert.Equal(2, ArticleArray[0].ArticleId);
            Assert.Equal(4, ArticleArray[1].ArticleId);
        }

        [Fact]
        public void Generate_ArticleType_Specific_Product_Count()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Articles).Returns((new Article[] {
                new Article 
                { 
                    ArticleId = 1, Name = "B1", 
                    ArticleTypeId = 1 
                },
                new Article
                {
                    ArticleId = 2, Name = "B2",
                    ArticleTypeId = 2
                },
                new Article
                {
                    ArticleId = 3, Name = "B3",
                    ArticleTypeId = 3
                },
                new Article
                {
                    ArticleId = 4, Name = "B4",
                    ArticleTypeId = 2
                },
                new Article
                {
                    ArticleId = 5, Name = "B5",
                    ArticleTypeId = 3
                }
            }).AsQueryable<Article>());

            HomeController target = new HomeController(mock.Object);
            target.PageSize = 3;

            Func<ViewResult, ArticleListViewModel> GetModel = result =>
                result?.ViewData?.Model as ArticleListViewModel;

            int? res1 = GetPagingInfoCount(target.Index(ArticleType: 1))?.PagingInfo?.TotalItems;
            int? resAll = GetPagingInfoCount(target.Index(ArticleType: null))?.PagingInfo?.TotalItems;

            Assert.Equal(1, res1);
            Assert.Equal(5, resAll);
        }

        ArticleListViewModel GetPagingInfoCount(ViewResult viewResult) 
            => (viewResult?.ViewData?.Model as ArticleListViewModel);
    }
}
