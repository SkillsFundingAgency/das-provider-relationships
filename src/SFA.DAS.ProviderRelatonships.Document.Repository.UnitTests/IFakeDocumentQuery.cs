using System.Linq;
using Microsoft.Azure.Documents.Linq;

namespace SFA.DAS.ProviderRelatonships.Document.Repository.UnitTests
{
    public interface IFakeDocumentQuery<T> : IDocumentQuery<T>, IOrderedQueryable<T>
    {
    }





}

