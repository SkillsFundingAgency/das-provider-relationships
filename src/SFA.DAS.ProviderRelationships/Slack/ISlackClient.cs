using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Slack
{
    public interface ISlackClient
    {
        Task PostCreatedEmployerAccount(string name);
    }
}