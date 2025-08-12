using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.Application.Services;

public interface IPermitNumberService
{
    Task<string> GeneratePermitNumberAsync(PermitType permitType);
    Task<bool> IsPermitNumberUniqueAsync(string permitNumber);
}

public class PermitNumberService : IPermitNumberService
{
    private readonly AppDbContext _context;

    public PermitNumberService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> GeneratePermitNumberAsync(PermitType permitType)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"{permitType.Code}{year}";
        
        var lastPermit = await _context.Permits
            .Where(p => p.PermitNumber.StartsWith(prefix))
            .OrderByDescending(p => p.PermitNumber)
            .FirstOrDefaultAsync();

        int nextSequence = 1;
        if (lastPermit != null)
        {
            var lastNumber = lastPermit.PermitNumber.Substring(prefix.Length);
            if (int.TryParse(lastNumber, out int lastSequence))
            {
                nextSequence = lastSequence + 1;
            }
        }

        var permitNumber = $"{prefix}{nextSequence:D6}";
        
        // Ensure uniqueness
        while (await _context.Permits.AnyAsync(p => p.PermitNumber == permitNumber))
        {
            nextSequence++;
            permitNumber = $"{prefix}{nextSequence:D6}";
        }

        return permitNumber;
    }

    public async Task<bool> IsPermitNumberUniqueAsync(string permitNumber)
    {
        return !await _context.Permits.AnyAsync(p => p.PermitNumber == permitNumber);
    }
}
