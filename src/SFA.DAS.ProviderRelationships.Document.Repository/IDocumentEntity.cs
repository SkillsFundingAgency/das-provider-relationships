using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentEntity
    {
        Guid? Id { get; }
        string ETag { get; }
    }
}
