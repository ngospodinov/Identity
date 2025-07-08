using Application.Common.Models;
using Application.Handlers.Institutions.Dtos;
using MediatR;

namespace Application.Handlers.Institutions.Get.GetPaged;

public class GetPagedInstitutionsRequest(int pageSize = 10, int pageNumber = 1) : IRequest<PagedResult<InstitutionDto>>
{
    public int PageSize { get; set; } = pageSize;
    
    public int PageNumber { get; set; } = 1;
}