using Lab1_Vardzar_2_4.Models;

namespace Lab1_Vardzar_2_4.Services;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(Guid id);
    Task<int> SaveAsync(Student student);
    Task<int> DeleteAsync(Student student);
}