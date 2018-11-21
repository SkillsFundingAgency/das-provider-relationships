using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal interface IDocumentClientFactory
    {
        Task<IDocumentClient> CreateDocumentClient();
    }
}