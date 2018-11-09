using System;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocument
    {
        Guid Id { get; set; }
        string ETag { get; set; }
    }
}
