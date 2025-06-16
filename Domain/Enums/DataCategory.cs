using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum DataCategory
{
    Academic = 1,
    Financial,
    Legal,
    Personal,
    Public,
}

public static class DataCategoryExtensions
{
    public static string ParseCategory(this DataCategory category)
        => category switch
        {
            DataCategory.Personal => "Personal",
            DataCategory.Public => "Public",
            DataCategory.Financial => "Financial",
            DataCategory.Academic => "Academic",
            DataCategory.Legal => "Legal" ,
            _ => throw new ValidationException($"Unknown category '{category}'.")
        };
}