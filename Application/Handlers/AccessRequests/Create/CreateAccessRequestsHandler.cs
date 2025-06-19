using Application.Exceptions;
using Application.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.AccessRequests.Create;

public class CreateAccessRequestsHandler(IUserRepository userRepository, IAccessRequestRepository accessRequestRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateAccessRequests>
{
    public async Task Handle(CreateAccessRequests request, CancellationToken ct)
    {
        var dataOwnerUserExists = await userRepository.ExistsAsync(request.DataOwnerUserId, ct);
        if (!dataOwnerUserExists)
        {
            throw new NotFoundException($"User with id {request.DataOwnerUserId} not found");
        }

        if (request.SelectedCategories.Count > 0)
        {
            var newAccessRequests = request.SelectedCategories.Select
            (x => new AccessRequest
            {
                DataOwnerUserId = request.DataOwnerUserId,
                RequesterUserId = request.RequesterUserId,
                RequestedCategory = x,
                RequestedAt = DateTime.UtcNow,
            });

            foreach (var accessRequest in newAccessRequests)
            {
                await accessRequestRepository.CreateAsync(accessRequest, ct);
            }
            
            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}