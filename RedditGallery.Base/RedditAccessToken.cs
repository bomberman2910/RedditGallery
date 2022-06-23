using System.Text.Json.Serialization;

namespace RedditGallery.Base;

public class RedditAccessToken
{
    public RedditAccessToken(string accessToken, int expiresIn, string scope, string tokenType)
    {
        AccessToken = accessToken;
        ExpiresIn = expiresIn;
        Scope = scope;
        TokenType = tokenType;
    }

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
}