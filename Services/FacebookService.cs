using System.Text.Json;
using CMS.ViewModels.Facebook;

namespace CMS.Services;

public class FacebookService
{
    private readonly HttpClient _httpClient;

    public FacebookService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<FacebookLike>> GetUserLikesAsync(string accessToken)
    {
        string url = $"https://graph.facebook.com/v17.0/me/likes?fields=id,name,created_time&access_token={accessToken}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonDocument.Parse(json).RootElement.GetProperty("data");

        var likes = new List<FacebookLike>();
        foreach (var item in data.EnumerateArray())
        {
            likes.Add(new FacebookLike
            {
                Id = item.GetProperty("id").GetString(),
                Name = item.GetProperty("name").GetString(),
                CreatedTime = item.GetProperty("created_time").GetString()
            });
        }

        return likes;
    }
}
