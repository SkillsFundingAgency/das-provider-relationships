using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRelationships.UnitTests.Models;

public class AccountTests
{
    [Test]
    public void And_Provider_Already_Exists_Then_Throws_Exception()
    {
        //arrange
        //todo: remove this tech debt!!
        var magicallyRequiredByModels = new UnitOfWorkContext();//new'd here despite usage in models is static.. 
        var account = new Account(2, "hashedid", "pub", "name", DateTime.Now);
        var provider = EntityActivator.CreateInstance<Provider>().Set(p => p.Ukprn, 12345678).Set(p => p.Name, "Foo");
        account.AddProvider(
            provider,
            new User(Guid.NewGuid(), "email", "firstName", "lastName"), 
            Guid.NewGuid());

        //act
        Action act = () => account.AddProvider(
            provider,
            new User(Guid.NewGuid(), "email", "firstName", "lastName"),
            Guid.NewGuid());

        //assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Requires provider has not already been added");
    }
}