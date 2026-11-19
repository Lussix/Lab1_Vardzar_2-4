namespace Lab1_Vardzar_2_4.Services;

public interface IAppStateService
{
    string? CurrentStudentName { get; set; }
    DateTime SessionStarted { get; }

    Guid? DraftStudentId { get; set; }
    double? DraftAverageScore { get; set; }

    void Save();
    void Restore();
}