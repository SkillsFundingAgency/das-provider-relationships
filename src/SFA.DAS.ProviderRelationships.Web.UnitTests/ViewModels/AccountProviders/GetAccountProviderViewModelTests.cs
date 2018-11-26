using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.ViewModels.AccountProviders
{
    [TestFixture]
    [Parallelizable]
    public class GetAccountProviderViewModelTests : FluentTest<GetAccountProviderViewModelTestsFixture>
    {
        [Test]
        public void GetOperationsByAccountLegalEntityId_WhenOperationsExist_ThenShouldReturnOperations()
        {
            Run(f => f.GetOperationsByAccountLegalEntityId(), (f, r) => r.Should().NotBeNull().And.BeEquivalentTo(new HashSet<Operation> { Operation.CreateCohort })); 
        }
    }

    public class GetAccountProviderViewModelTestsFixture
    {
        public AccountLegalEntityBasicDto AccountLegalEntity { get; set; }
        public List<AccountLegalEntityBasicDto> AccountLegalEntities { get; set; }
        public AccountProviderDto AccountProvider { get; set; }
        public GetAccountProviderViewModel ViewModel { get; set; }

        public GetAccountProviderViewModelTestsFixture()
        {
            AccountLegalEntity = new AccountLegalEntityBasicDto { Id = 1 };
            
            AccountLegalEntities = new List<AccountLegalEntityBasicDto>
            {
                AccountLegalEntity,
                new AccountLegalEntityBasicDto { Id = 2 },
                new AccountLegalEntityBasicDto { Id = 3 }
            };
            
            AccountProvider = new AccountProviderDto
            {
                AccountProviderLegalEntities = new List<AccountProviderLegalEntityDto>
                {
                    new AccountProviderLegalEntityDto
                    {
                        AccountLegalEntityId = AccountLegalEntity.Id,
                        Permissions = new List<PermissionDto>
                        {
                            new PermissionDto
                            {
                                Operation = Operation.CreateCohort
                            }
                        }
                    },
                    new AccountProviderLegalEntityDto
                    {
                        AccountLegalEntityId = 4,
                        Permissions = new List<PermissionDto>()
                    }
                }
            };
            
            ViewModel = new GetAccountProviderViewModel
            {
                AccountProvider = AccountProvider,
                AccountLegalEntities = AccountLegalEntities
            };
        }

        public HashSet<Operation> GetOperationsByAccountLegalEntityId()
        {
            return ViewModel.GetOperationsByAccountLegalEntityId(AccountLegalEntity.Id);
        }
    }
}