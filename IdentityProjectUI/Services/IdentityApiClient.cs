using IdentityProjectUI.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace IdentityProjectUI.Services;

public class IdentityApiClient(HttpClient httpClient)
{
    public Task<UserDto?> GetUserAsync(string userId, CancellationToken ct) =>
        httpClient.GetFromJsonAsync<UserDto>($"/api/User/{userId}", ct);

    public async Task<PagedResult<UserDto>?> GetGrantedUsersAsync(int pageSize, int pageNumber, string? search,
        CancellationToken ct)
    {
        var query = new Dictionary<string, string?>
        {
            ["pageSize"] = pageSize.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
        };

        if (!string.IsNullOrWhiteSpace(search))
            query["search"] = search;

        var url = QueryHelpers.AddQueryString("/api/User/granted", query);

        return await httpClient.GetFromJsonAsync<PagedResult<UserDto>>(url, ct);
    }
    
    public async Task<PagedResult<UserDto>?> GetUsersAsync(int pageSize, int pageNumber, string? search,
        CancellationToken ct)
    {
        var query = new Dictionary<string, string?>
        {
            ["pageSize"] = pageSize.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
        };

        if (!string.IsNullOrWhiteSpace(search))
            query["search"] = search;

        var url = QueryHelpers.AddQueryString("/api/User", query);

        return await httpClient.GetFromJsonAsync<PagedResult<UserDto>>(url, ct);
    }

    public async Task<PagedResult<AccessRequestDto>?> GetAccessRequestsAsync(int pageSize, int pageNumber,
        CancellationToken ct)
    {
        var url = QueryHelpers.AddQueryString("/api/access-requests/me", new Dictionary<string, string?>
        {
            ["pageSize"] = pageSize.ToString(),
            ["pageNumber"] = pageNumber.ToString()
        });

        return await httpClient.GetFromJsonAsync<PagedResult<AccessRequestDto>>(url, ct);
    }

    public async Task<bool> AccessRequestDecisionAsync(AccessRequestDecision decision, CancellationToken ct)
    {
        using var resp = await httpClient.PostAsJsonAsync("/api/access-requests/decision", decision, ct);

        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> PostNewAccessRequestAsync(NewAccessRequestsDto accessRequests, CancellationToken ct)
    {
        using var resp = await httpClient.PostAsJsonAsync("/api/access-requests", accessRequests, ct);

        return resp.IsSuccessStatusCode;
    }

    public async Task<PagedResult<NameDto>?> GetMyNamesAsync(int pageSize, int pageNumber, CancellationToken ct)
    {
        var url = QueryHelpers.AddQueryString("/api/Name/me", new Dictionary<string, string?>
        {
            ["pageSize"] = pageSize.ToString(),
            ["pageNumber"] = pageNumber.ToString()
        });

        return await httpClient.GetFromJsonAsync<PagedResult<NameDto>>(url, ct);
    }

    public async Task<List<NameDto>?> GetMyNamesAsync(CancellationToken ct) =>
        await httpClient.GetFromJsonAsync<List<NameDto>>("/api/Name/me", ct);
    
    public async Task<NameDto?> GetNameAsync(int id, CancellationToken ct) =>
        await httpClient.GetFromJsonAsync<NameDto>($"/api/Name/me/{id}", ct);

    public async Task<NameDto?> GetNameAsync(string userId, string category, CancellationToken ct)
    {
        var url = QueryHelpers.AddQueryString($"/api/Name/{userId}", new Dictionary<string, string?>
        {
            ["category"] = category,
        });        
        
        return await httpClient.GetFromJsonAsync<NameDto>(url, ct);
    }

    public async Task<bool> EditNameAsync(NameDto name, CancellationToken ct)
    {
        using var resp = await httpClient.PostAsJsonAsync($"/api/Name/me/{name.Id}", name, ct);

        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> PostNameAsync(NameDto name, CancellationToken ct)
    {
        using var resp = await httpClient.PostAsJsonAsync("/api/Name/me", name, ct);

        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteNameAsync(int id, CancellationToken ct)
    {
        using var resp = await httpClient.DeleteAsync($"/api/Name/me/{id}", ct);

        return resp.IsSuccessStatusCode;
    }

    public async Task<PagedResult<DataItemDto>?> GetMyDataAsync(int pageSize, int pageNumber, CancellationToken ct)
    {
        var url = QueryHelpers.AddQueryString("/api/Data/me", new Dictionary<string, string?>
        {
            ["pageSize"] = pageSize.ToString(),
            ["pageNumber"] = pageNumber.ToString()
        });

        return await httpClient.GetFromJsonAsync<PagedResult<DataItemDto>>(url, ct);
    }

    public async Task<PagedResult<DataItemDto>?> GetDataItemsByCategoryAsync (int pageSize, int pageNumber, string? categoryFilter, CancellationToken ct)
    {
        var query = new Dictionary<string, string?>
        {
            ["pageSize"] = pageSize.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
        };

        if (!string.IsNullOrWhiteSpace(categoryFilter))
            query["categoryFilter"] = categoryFilter;

        var url = QueryHelpers.AddQueryString($"/api/Data/me", query);

        return await httpClient.GetFromJsonAsync<PagedResult<DataItemDto>>(url, ct);
    }

    public async Task<List<DataItemDto>> GetDataItemsForCategoryByUserAsync(string userId, int pageSize, int pageNumber,
        string categoryFilter, CancellationToken ct)
    {
        var query = new Dictionary<string, string?>
        {
            ["pageSize"] = pageSize.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
            ["categoryFilter"] = categoryFilter,
        };
        
        var url = QueryHelpers.AddQueryString($"/api/Data/{userId}", query);

        return await httpClient.GetFromJsonAsync<List<DataItemDto>>(url, ct);
    }
    
    public async Task<List<GrantedCategoryDto>?> GetGrantedCategoriesAsync(string userId, CancellationToken ct)
    {
        var url = $"/api/access-grants/{userId}";

        return await httpClient.GetFromJsonAsync<List<GrantedCategoryDto>>(url, ct);
    }

    public async Task<bool> PostDataItemAsync(DataItemDto data, CancellationToken ct)
     {
         using var resp = await httpClient.PostAsJsonAsync("/api/Data/me", data, ct);
         
         return resp.IsSuccessStatusCode;
     }
     
     public Task<DataItemDto?> GetDataItemAsync(int id, CancellationToken ct) =>
         httpClient.GetFromJsonAsync<DataItemDto>($"/api/Data/me/{id}", ct);


     public async Task<bool> EditDataItemAsync(DataItemDto data, CancellationToken ct)
     {
         using var resp = await httpClient.PutAsJsonAsync($"/api/Data/me/{data.Id}", data, ct);

         return resp.IsSuccessStatusCode;
     }
     
     public async Task<bool> DeleteDataItemAsync(int id, CancellationToken ct)
     {
         using var resp = await httpClient.DeleteAsync($"/api/Data/me/{id}", ct);
         
         return resp.IsSuccessStatusCode;
     }

     public async Task<PagedResult<AccessGrantDto>?> GetAccessGrantsAsync(int pageSize, int pageNumber,
         CancellationToken ct)
     {
         var query = new Dictionary<string, string?>
         {
             ["pageSize"] = pageSize.ToString(),
             ["pageNumber"] = pageNumber.ToString(),
         };
         
         var url = QueryHelpers.AddQueryString($"/api/access-grants/me", query);

         return await httpClient.GetFromJsonAsync<PagedResult<AccessGrantDto>>(url, ct);
     }

     public async Task<bool> RevokeAccessGrantAsync(int id, CancellationToken ct)
     {
         using var resp = await httpClient.DeleteAsync($"/api/access-grants/me/{id}", ct);
         
         return resp.IsSuccessStatusCode;
     }
}