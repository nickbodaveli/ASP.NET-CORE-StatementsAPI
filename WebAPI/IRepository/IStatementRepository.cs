using WebAPI.Models;

namespace WebAPI.IRepository
{
    public interface IStatementRepository
    {
        Task<Statement> GetStatementAsync(int Id);
        Task<Statement> GetStatementImageAsync(int Id);
        Task<List<Statement>> GetAllStatementAsync();
        Task<Statement> AddStatementAsync(Statement addStatement);
        Task<Statement> UpdatePersonAsync(int id, Statement model);
        Task<Statement> DeletePersonAsync(int id);
        Task<string> SaveImage(IFormFile imageFile);
        void DeleteImage(string imageName);
    }
}
