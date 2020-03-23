namespace SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd.Dtos
{
    public class ProviderDto
    {
        public long Ukprn { get; set; }
        public string Name { get; set; }
        public string FormattedProviderSuggestion => $"{Name.ToUpper()} {Ukprn}";
    }
}