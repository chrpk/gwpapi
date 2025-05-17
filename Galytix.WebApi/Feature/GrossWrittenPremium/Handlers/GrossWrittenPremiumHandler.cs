using Galytix.WebApi.Domain.Entities;
using LiteDB.Async;
using MediatR;

namespace Galytix.WebApi.Feature.GrossWrittenPremium;

/// <summary>
/// GrossWrittenPremiumHandler
/// </summary>
public class GrossWrittenPremiumHandler : IRequestHandler<GetAverageRequest, Dictionary<string, decimal>>
{
    private readonly ILiteDatabaseAsync _db;
    private readonly ILogger<GrossWrittenPremiumHandler> _logger;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="db"></param>
    /// <param name="logger"></param>
    public GrossWrittenPremiumHandler(ILiteDatabaseAsync db,
        ILogger<GrossWrittenPremiumHandler> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<Dictionary<string, decimal>> Handle(GetAverageRequest request,
        CancellationToken cancellationToken)
    {
        var collection = _db.GetCollection<GrossWrittenPremiumRecord>();

        request.Lob = request.Lob
            .Select(l => l.ToLower())
            .ToArray();

        var query = collection.Query()
            .Where(x => x.Country.ToLower() == request.Country.ToLower());

        var lob = request.Lob
            .Select(l => l.ToLower())
            .ToArray();

        if (lob.Length > 0)
            // Filter by line of business
            query = query.Where(x => lob.Contains(x.LineOfBusiness.ToLower()));

        // Get the sum of the yearly values for each record
        var result = await query.OrderBy(x => x.LineOfBusiness)
            .ToArrayAsync();

        // Map result to a dictionary
        // Better to map it in the database, but for the sake of simplicity, I do it here.
        return result.ToDictionary(x => x.LineOfBusiness, x => x.Sum);
    }
}