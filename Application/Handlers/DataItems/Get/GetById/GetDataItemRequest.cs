using Application.Handlers.Users.Dtos;
using MediatR;

namespace Application.Handlers.DataItems.Get.GetById;

public class GetDataItemRequest(int id, string userId) : IRequest<UserDataItemDto?>
{
    public int Id { get; set; } = id;

    public string UserId { get; set; } = userId;
}