using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.ReadStore.Mediator
{
    internal class Mediator : IMediator
    {
        private static readonly ConcurrentDictionary<Type, object> RequestHandlers = new ConcurrentDictionary<Type, object>();
        
        private readonly ServiceFactory _serviceFactory;

        public Mediator(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var requestType = request.GetType();
            
            var handler = (RequestHandler<TResponse>)RequestHandlers.GetOrAdd(requestType, t =>
            {
                var responseType = typeof(TResponse);
                var handlerOpenGenericType = typeof(RequestHandler<,>);
                var handlerClosedGenericType = handlerOpenGenericType.MakeGenericType(requestType, responseType);
                var handlerInstance = Activator.CreateInstance(handlerClosedGenericType);

                return handlerInstance;
            });

            return handler.Handle(request, cancellationToken, _serviceFactory);
        }
        
        private abstract class RequestHandler<TResponse>
        {
            public abstract Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken, ServiceFactory serviceFactory);
        }
    
        private class RequestHandler<TRequest, TResponse> : RequestHandler<TResponse> where TRequest : IRequest<TResponse>
        {
            public override Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken, ServiceFactory serviceFactory)
            {
                var handler = serviceFactory.GetInstance<IRequestHandler<TRequest, TResponse>>();
            
                return handler.Handle((TRequest)request, cancellationToken);
            }
        }
    }
}