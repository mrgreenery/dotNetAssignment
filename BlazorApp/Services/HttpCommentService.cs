using System.Text.Json;
using ApiContract;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    public readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<CommentDto> AddComment(CreateCommentDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("comments", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdateComment(int id, UpdateCommentDto request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"comments/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }

    public async Task<CommentDto> GetCommentAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"comments/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<CommentDto>> GetCommentsAsync(int? userId, int? postId)
    {
        string url = "comments";
        if (userId.HasValue && postId.HasValue)
        {
            url = $"comments?userId={userId.Value}&postId={postId.Value}";
        }
        else if (userId.HasValue)
        {
            url = $"comments?userId={userId.Value}";
        }
        else if (postId.HasValue)
        {
           url =  $"comments?postId={postId.Value}";
        }
   
        HttpResponseMessage httpResponse = await client.GetAsync(url);
        string response  = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<CommentDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task DeleteCommentAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync($"comments/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }
}