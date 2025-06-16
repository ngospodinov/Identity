using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Common;

public static class StringExtensions
{
    public static DataCategory ParseDataCategory(this string data)
        => data?.Trim() switch
        {
            "Public" => DataCategory.Public,
            "Financial" => DataCategory.Financial,
            "Academic" => DataCategory.Academic,
            "Legal" => DataCategory.Legal,
            _ => throw new ValidationException($"Unknown category '{data}'.")
        };
    
    public static RequestStatus ParseRequestStatus(this string status)
        => status?.Trim() switch
        {
            "Public" => RequestStatus.Pending,
            "Approved" => RequestStatus.Approved,
            "Denied" => RequestStatus.Denied,
            _ => throw new ValidationException($"Unknown request status '{status}'.")
        };

}