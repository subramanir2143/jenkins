using Cnx.EarningsAndDeductions.Domain.Exceptions;
using Cnx.EarningsAndDeductions.Infrastructure.EventStore;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly MongoEventStore _context;

        public RequestManager(IOptions<EDSettings> settings, IMediator mediator)
        {
            _context = new MongoEventStore(settings, mediator);
        }

        /// <summary>
        /// Save the Request Id 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new EarningsAndDeductionsDomainException($"Request with {id} already exists") :
                new ClientRequest()
                {
                    RequestId = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };

           await  _context.ClientRequests.InsertOneAsync(request);
        }

        /// <summary>
        /// Check whether the Request Id already exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ExistAsync(Guid id)
        {
            var filter = Builders<ClientRequest>.Filter.Eq("RequestId", id);
            var request = await _context.ClientRequests
                          .Find(filter).FirstOrDefaultAsync();

            return request != null;
        }
    }
}
