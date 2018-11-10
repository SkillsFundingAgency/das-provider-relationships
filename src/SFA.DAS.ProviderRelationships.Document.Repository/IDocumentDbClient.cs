
using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentDbClient    {
        string DatabaseName { get; }
        IDocumentClient DocumentClient { get; }
    }
}
