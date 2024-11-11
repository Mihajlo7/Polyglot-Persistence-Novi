namespace SQLDataAccess
{
    public interface IGenericRepository
    {
        public Task InsertOne<T> (T entity);
        public Task InsertMany<T> (List<T> entity);
        public Task InsertBulk<T> (List<T> entity);
    }
}
