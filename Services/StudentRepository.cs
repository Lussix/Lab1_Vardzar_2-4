using Lab1_Vardzar_2_4.Models;
using SQLite;

namespace Lab1_Vardzar_2_4.Services;

public class StudentRepository : IStudentRepository
{
    private readonly SQLiteAsyncConnection _db;
    private bool _initialized;

    public StudentRepository(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
    }

 
    private async Task InitAsync()
    {
        if (_initialized) return;
        await _db.CreateTableAsync<Student>();
        _initialized = true;
    }

    public async Task<List<Student>> GetAllAsync()
    {
        await InitAsync();
        return await _db.Table<Student>().ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(Guid id)
    {
        await InitAsync();
        return await _db.FindAsync<Student>(id);
    }

    public async Task<int> SaveAsync(Student student)
    {
        await InitAsync();
        return await _db.InsertOrReplaceAsync(student);
    }

    public async Task<int> DeleteAsync(Student student)
    {
        await InitAsync();
        return await _db.DeleteAsync(student);
    }
}