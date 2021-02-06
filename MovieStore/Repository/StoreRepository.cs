using System.Linq;
using MovieStore.Models;

namespace MovieStore.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private DatabaseContext _context;

        public StoreRepository(DatabaseContext ctx) => _context = ctx;

        public IQueryable<Article> Articles => _context.Articles;

        public IQueryable<ArticleType> ArticleTypes => _context.ArticleTypes;

        public void CreateArticle(Article b)
        {
            _context.Add(b);
            _context.SaveChanges();
        }

        public void CreateArticleType(ArticleType g)
        {
            _context.Add(g);
            _context.SaveChanges();
        }

        public void DeleteArticle(Article b)
        {
            _context.Remove(b);
            _context.SaveChanges();
        }

        public void DeleteArticleType(ArticleType g)
        {
            _context.Remove(g);
            _context.SaveChanges();
        }

        public void SaveArticle(Article b)
        {
            _context.SaveChanges();
        }

        public void SaveArticleType(ArticleType g)
        {
            _context.SaveChanges();
        }
    }
}
