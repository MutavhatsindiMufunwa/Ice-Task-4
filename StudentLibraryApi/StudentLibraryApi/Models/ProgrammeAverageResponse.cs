namespace StudentLibraryApi.Models
{
    public sealed record ProgrammeAverageResponse(string Programme, decimal Average, string FormattedAverage)
    {
    }
}
