﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderQueryReply>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IConfigurationProvider _configurationProvider;

        public GetProviderQueryHandler(Lazy<ProviderRelationshipsDbContext> db, IConfigurationProvider configurationProvider)
        {
            _db = db;
            _configurationProvider = configurationProvider;
        }

        public async Task<GetProviderQueryReply> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            var provider = await _db.Value.Providers
                .Where(p => p.Ukprn == request.Ukprn)
                .ProjectTo<ProviderDto>(_configurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return provider == null ? null : new GetProviderQueryReply(provider);
        }
    }
}