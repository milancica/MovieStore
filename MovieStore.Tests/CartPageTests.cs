using System.Linq;
using MovieStore.Models;
using MovieStore.Pages;
using MovieStore.Repository;
using Moq;
using Xunit;

namespace MovieStore.Tests
{
    public class CartPageTests
    {
        [Fact]
        public void Can_Load_Cart()
        {
            Article b1 = new Article { ArticleId = 1, Name = "B1" };
            Article b2 = new Article { ArticleId = 2, Name = "B2" };

            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Articles).Returns((new Article[] { b1, b2 })
                .AsQueryable());

            Cart testCart = new Cart();
            testCart.AddItem(b1, 2);
            testCart.AddItem(b2, 1);

            CartModel cartModel = new CartModel(mock.Object, testCart);
            cartModel.OnGet("myUrl");

            Assert.Equal(2, cartModel.Cart.Lines.Count());
            Assert.Equal("myUrl", cartModel.ReturnUrl);
        }

        [Fact]
        public void Can_Update_Cart()
        {
            Article b1 = new Article { ArticleId = 1, Name = "B1" };

            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.Articles).Returns((new Article[] { b1 })
                .AsQueryable());

            Cart testCart = new Cart();
            CartModel cartModel = new CartModel(mock.Object, testCart);

            cartModel.OnPost(1, "myUrl");

            Assert.Single(testCart.Lines);
            Assert.Equal("B1", testCart.Lines.First().Article.Name);
            Assert.Equal(1, testCart.Lines.First().Quantity);
        }
    }
}
