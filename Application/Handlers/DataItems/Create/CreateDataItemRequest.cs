using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.DataItems.Create;

public class CreateDataItemRequest : IRequest<int>
{
    [JsonIgnore]
    public string DataOwnerUserId { get; set; } = null!;
    
    [Required]
    public string Key { get; init; } = null!;
    
    [Required]
    public string Value { get; init; } = null!;
    
    public DataCategory Category { get; init; }
    
}