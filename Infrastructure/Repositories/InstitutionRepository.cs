using Application.Common.Models;
using Application.Handlers.Institutions.Dtos;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InstitutionRepository(IdentityDbContext dbContext) : IInstitutionRepository
{
    public async Task CreateInstitutionAsync(Institution institution, CancellationToken cancellationToken)
    {
        institution.Id = Guid.NewGuid();
        dbContext.Institutions.Add(institution);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<InstitutionDto>> GetInstitutionsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        return await dbContext.Institutions
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new InstitutionDto
            {
                Id = x.Id,
                Name = x.Name,
                ClientId = x.ClientId,
                ContactEmail = x.ContactEmail
            }).ToListAsync(cancellationToken);
    }
    
    public async Task<Institution?> GetInstitutionByIdAsync(Guid institutionId, CancellationToken cancellationToken)
    {
        return await dbContext.Institutions
            .Include(x => x.AccessRequests)
            .Include(x => x.AccessGrants)
            .FirstOrDefaultAsync(x => x.Id == institutionId, cancellationToken);
    }

    public async Task<Guid?> GetInstitutionIdByClientAsync(string clientId, CancellationToken cancellationToken)
    {
        return await dbContext.Institutions.Where(x => x.ClientId == clientId)
            .Select(x => (Guid?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<bool> ExistsAsync(Guid institutionId, CancellationToken cancellationToken)
    {
        return await dbContext.Institutions.AnyAsync(x => x.Id == institutionId, cancellationToken);
    }

    public async Task DeleteInstitutionAsync(Guid institutionId, CancellationToken cancellationToken)
    {
        var institution = await dbContext.Institutions
            .FirstOrDefaultAsync(x => x.Id == institutionId, cancellationToken);
        if (institution != null)
        {
            dbContext.Institutions.Remove(institution);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}