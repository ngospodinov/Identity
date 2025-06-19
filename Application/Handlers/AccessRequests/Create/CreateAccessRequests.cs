using Domain.Enums;
using MediatR;

namespace Application.Handlers.AccessRequests.Create;

public class CreateAccessRequests : IRequest
{
    public string RequesterUserId { get; set; }
    
    public string DataOwnerUserId { get; set; }
    
    public List<DataCategory> SelectedCategories { get; set; }
}