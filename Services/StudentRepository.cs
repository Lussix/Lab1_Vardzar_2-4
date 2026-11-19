using System.Diagnostics;
using Lab1_Vardzar_2_4.Models;
using SQLite;

namespace Lab1_Vardzar_2_4.Services;

public class StudentRepository : IStudentRepository
{
    private readonly SQLiteAsyncConnection _db;
    private readonly ICryptoService _crypto;
    private bool _initialized;

    public StudentRepository(string dbPath, ICryptoService crypto)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _crypto = crypto;
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
        var students = await _db.Table<Student>().ToListAsync();

        foreach (var s in students)
            s.StudentCardNumber = await _crypto.DecryptAsync(s.StudentCardEncrypted);

        return students;
    }

    public async Task<Student?> GetByIdAsync(Guid id)
    {
        await InitAsync();
        var student = await _db.FindAsync<Student>(id);

        if (student is not null)
            student.StudentCardNumber = await _crypto.DecryptAsync(student.StudentCardEncrypted);

        return student;
    }

    public async Task<int> SaveAsync(Student student)
    {
        await InitAsync();

        student.StudentCardEncrypted = await _crypto.EncryptAsync(student.StudentCardNumber);
        if (!string.IsNullOrEmpty(student.StudentCardNumber))
            Debug.WriteLine($"[CRYPTO] номер квитка '{student.StudentCardNumber}' записано в базу як: {student.StudentCardEncrypted}");

        return await _db.InsertOrReplaceAsync(student);
    }

    public async Task<int> DeleteAsync(Student student)
    {
        await InitAsync();
        return await _db.DeleteAsync(student);
    }
}