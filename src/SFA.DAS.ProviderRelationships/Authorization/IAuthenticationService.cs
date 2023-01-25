using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Authorization
{
    public interface IAuthorizationService
    {
        void Authorize(params string[] options);
        Task AuthorizeAsync(params string[] options);
        AuthorizationResult GetAuthorizationResult(params string[] options);
        Task<AuthorizationResult> GetAuthorizationResultAsync(params string[] options);
        bool IsAuthorized(params string[] options);
        Task<bool> IsAuthorizedAsync(params string[] options);
    }

    public class AuthorizationResult
    {
        public bool IsAuthorized => _errors.Count == 0;
        public IEnumerable<AuthorizationError> Errors => _errors;

        private readonly List<AuthorizationError> _errors = new List<AuthorizationError>();

        public AuthorizationResult()
        {
        }

        public AuthorizationResult(AuthorizationError error)
        {
            _errors.Add(error);
        }

        public AuthorizationResult(IEnumerable<AuthorizationError> errors)
        {
            _errors.AddRange(errors);
        }

        public AuthorizationResult AddError(AuthorizationError error)
        {
            _errors.Add(error);

            return this;
        }

        public bool HasError<T>() where T : AuthorizationError
        {
            return _errors.OfType<T>().Any();
        }

        public override string ToString()
        {
            return $"{nameof(IsAuthorized)}: {IsAuthorized}, {nameof(Errors)}: {(_errors.Count > 0 ? string.Join(", ", _errors.Select(e => e.Message)) : "None")}";
        }
    }
    
    public abstract class AuthorizationError
    {
        public string Message { get; }

        protected AuthorizationError(string message)
        {
            Message = message;
        }
    }
}