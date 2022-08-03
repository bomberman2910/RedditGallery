using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RedditGallery.Base;

public class RedditApiController
{
    private readonly Credentials credentials;
    private const string CredentialsFileName = "credentials.config";
    private const string UserAgent = "windows:redgallery:v0.0.1 (by /u/kokoroatariganai)";
    private AccessToken currentAccessToken;

    public string CurrentSubRedditLink { get; set; } = string.Empty;
    public PostCategory CurrentPostCategory { get; set; } = PostCategory.New;

    public RedditApiController()
    {
        var credentialsFileExists = File.Exists(CredentialsFileName);
        if (!credentialsFileExists)
        {
            credentials = new Credentials(string.Empty, string.Empty);
            var newCredentialsString = JsonSerializer.Serialize(credentials, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(CredentialsFileName, newCredentialsString);
            return;
        }

        var credentialFileContent = File.ReadAllText(CredentialsFileName);
        var readCredentials = JsonSerializer.Deserialize<Credentials>(credentialFileContent);
        if (readCredentials is null || readCredentials.IsIncomplete())
            throw new InvalidCredentialException($"The credentials in file {CredentialsFileName} seem to be corrupt.");
        credentials = readCredentials;
        GetAccessToken();
    }

    private string GetAccessToken()
    {
        const string accessTokenUrl = "https://www.reddit.com/api/v1/access_token";

        if(credentials.IsIncomplete())
            throw new InvalidCredentialException($"Credentials seem to be corrupt.");

        if (currentAccessToken is not null && !currentAccessToken.IsExpired())
            return currentAccessToken.Token;

        var requestContent = new FormUrlEncodedContent(new KeyValuePair<string, string>[] { new("grant_type", "client_credentials") });
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials.GetBase64String());
        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
        var response = client.PostAsync(accessTokenUrl, requestContent).Result;
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Error occured while getting access token (StatusCode: {(int)response.StatusCode} {response.StatusCode})");
        var result = response.Content.ReadAsStringAsync().Result;
        var accessToken = JsonSerializer.Deserialize<RedditAccessToken>(result);
        if (accessToken is null)
            throw new Exception("Received invalid access token from server");
        currentAccessToken = new AccessToken(accessToken.AccessToken, DateTime.Now.Ticks, accessToken.ExpiresIn);
        return accessToken.AccessToken;
    }

    public async Task<Post> GetPostAsync(string before = null, string after = null)
    {
        const string baseUrl = "https://oauth.reddit.com/";
        var parameters = new List<string>
        {
            "show=all",
            "raw_json=1",
            "limit=1"
        };
        if (before != null)
            parameters.Add($"before={before}");
        if (after != null)
            parameters.Add($"after={after}");

        var accessToken = GetAccessToken();

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", UserAgent);
        var url = $"{baseUrl}{CurrentSubRedditLink}/{CurrentPostCategory.ToString().ToLower()}?{string.Join('&', parameters)}";
        var response = await client.GetAsync(url);
        if(!response.IsSuccessStatusCode)
            throw new Exception($"Error occured while getting post (StatusCode: {(int)response.StatusCode} {response.StatusCode})");
        var contents = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<RedditResult>(contents, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault });
        if (result is null || result.Data.Children.Length == 0)
            return null;
        var postData = result.Data.Children[^1].Data;
        var postAfter = result.Data.After;
        var postBefore = result.Data.Before;
        var postType = postData.IsSelf ? "text" : postData.PostHint ?? (postData.IsRedditMediaDomain ? postData.IsVideo ? "hosted:video" : "image" : string.Empty);
        var mediaLink = postType switch
        {
            "image" => postData.Url.AbsoluteUri,
            "hosted:video" => postData.Media.RedditVideo.FallbackUrl.AbsoluteUri,
            _ => string.Empty
        };
        return new Post
        {
            Type = postType,
            Title = postData.Title,
            Name = postData.Name,
            Link = postData.Permalink,
            MediaLink = mediaLink,
            Before = postBefore,
            After = postAfter
        };
    }
}

internal class AccessToken
{
    private const int ReissuanceThreshold = 1000;

    public AccessToken(string token, long issuedAt, long expiresIn)
    {
        Token = token;
        IssuedAt = issuedAt;
        ExpiresIn = expiresIn;
    }

    public string Token { get; }
    public long IssuedAt { get; }
    public long ExpiresIn { get; }

    public bool IsExpired()
    {
        return IssuedAt + ExpiresIn - DateTime.Now.Ticks > ReissuanceThreshold;
    }
}

public enum PostCategory
{
    Hot,New,Top
}

internal class Credentials
{
    private string base64String;

    public Credentials(string clientToken, string clientSecret)
    {
        ClientToken = clientToken;
        ClientSecret = clientSecret;
        base64String = string.Empty;
    }

    public string ClientToken { get; }
    public string ClientSecret { get; }

    public bool IsIncomplete()
    {
        return string.IsNullOrEmpty(ClientToken) || string.IsNullOrEmpty(ClientSecret);
    }

    public string GetBase64String()
    {
        if(!string.IsNullOrEmpty(base64String))
            return base64String;

        var byteArray = new UTF8Encoding().GetBytes($"{ClientToken}:{ClientSecret}");
        base64String = Convert.ToBase64String(byteArray);
        return base64String;
    }
}