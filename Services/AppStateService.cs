namespace Lab1_Vardzar_2_4.Services;

public class AppStateService : IAppStateService
{
    public string? CurrentStudentName { get; set; }
    public DateTime SessionStarted { get; } = DateTime.Now;

    public Guid? DraftStudentId { get; set; }
    public double? DraftAverageScore { get; set; }

    public void Save()
    {
        if (DraftStudentId is Guid id && DraftAverageScore is double score)
        {
            Preferences.Set("draft_id", id.ToString());
            Preferences.Set("draft_score", score);
        }
        else
        {
            Preferences.Remove("draft_id");
            Preferences.Remove("draft_score");
        }
    }

    public void Restore()
    {
        var idText = Preferences.Get("draft_id", "");
        if (Guid.TryParse(idText, out var id))
        {
            DraftStudentId = id;
            DraftAverageScore = Preferences.Get("draft_score", 0.0);
        }
    }
}