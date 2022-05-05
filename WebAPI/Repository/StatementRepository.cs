using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Helpers.Interfaces;
using WebAPI.IRepository;
using WebAPI.Models;
using System.IO;
using System.Linq;

namespace WebAPI.Repository
{
    public class StatementRepository : IStatementRepository
    {
        private readonly DataContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public StatementRepository(DataContext context, IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<Statement> AddStatementAsync(Statement addStatement)
        {
            var _statement = new Statement();
            _statement.Title = addStatement.Title;
            _statement.Description = addStatement.Description;  
            _statement.ImageName = addStatement.ImageName;
            _statement.PhoneNumber = addStatement.PhoneNumber;
            addStatement.ImageName = await SaveImage(addStatement.ImageFile);

            _unitOfWork.Add(_statement);
            await _unitOfWork.CommitAsync();
            return _statement;
        }

        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        public async Task<Statement> DeletePersonAsync(int id)
        {
            var statement = await _context.Statements.FindAsync(id);
            if (statement == null)
            {
                return null;
            }
            DeleteImage(statement.ImageName);
            _context.Statements.Remove(statement);
            await _context.SaveChangesAsync();

            return statement;
        }

        public async Task<Statement> GetStatementAsync(int Id)
        {
            var obj = await _unitOfWork.Query<Statement>().Where(n => n.Id == Id).FirstOrDefaultAsync();
            return obj;
        }

        public async Task<Statement> UpdatePersonAsync(int id, Statement statement)
        {
            if (id != statement.Id)
            {
                return null;
            }

            if (statement.ImageFile != null)
            {
                DeleteImage(statement.ImageName);
                statement.ImageName = await SaveImage(statement.ImageFile);
            }
            _context.Entry(statement).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (StatementModelExist(id) != null)
                {
                    throw new Exception(ex.Message);
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
            return null;
        }

        private async Task<Statement> StatementModelExist(int id)
        {
            return await _unitOfWork.Query<Statement>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Statement>> GetAllStatementAsync()
        {
            return await _unitOfWork.Query<Statement>().Select(x => new Statement()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                PhoneNumber = x.PhoneNumber,
                ImageName = x.ImageName,
                ImageSrc = x.ImageSrc
            }).ToListAsync();
        }

        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }

        public Task<Statement> GetStatementImageAsync(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
