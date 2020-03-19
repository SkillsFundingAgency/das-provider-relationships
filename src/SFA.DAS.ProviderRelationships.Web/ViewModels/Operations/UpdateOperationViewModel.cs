using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Operations
{
    public class UpdateOperationViewModel
    {
        [Required]
        public long? AccountProviderId { get; set; }
        [Required]
        public long? AccountLegalEntityId { get; set; }
        [Required]
        public Operation Operation { get; set; }
        [Required]
        public bool? IsEnabled { get; set; }
        public string AccountLegalEntityName { get; set; }
        public string ProviderName { get; set; }
        public int AccountLegalEntitiesCount { get; set; }
        public bool IsEditMode { get; set; }
        public string BackLink { get; set; }
        public List<OperationViewModel> Operations { get; set; }

        public void Update(List<OperationViewModel> operations)
        {
            Merge(operations);
            foreach (var operation in Operations)
            {
                operation.IsEnabled = Operations.First(i => i.Value.Equals(operation.Value)).IsEnabled;
            }
        }

        public void Merge(List<OperationViewModel> operations)
        {
            Operations = Operations.Union(operations).ToList();
        }
    }
}