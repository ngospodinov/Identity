using Application.Common.Models;
using Application.Handlers.Institutions.Dtos;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.Institutions.Get.GetPaged;

public class GetPagedInstitutionsHandler(IInstitutionRepository institutionRepository) : IRequestHandler<GetPagedInstitutionsRequest, PagedResult<InstitutionDto>>
{
    public async Task<PagedResult<InstitutionDto>> Handle(GetPagedInstitutionsRequest request,
        CancellationToken cancellationToken)
    {
        var institutions = await institutionRepository.GetInstitutionsAsync(request.PageNumber, request.PageSize, cancellationToken);

        return new PagedResult<InstitutionDto>()
        {
            Items = institutions,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = institutions.Count,
        };
    }
}