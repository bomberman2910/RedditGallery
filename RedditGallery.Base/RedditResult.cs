using System;
using System.Text.Json.Serialization;

// ReSharper disable UnusedMember.Global

namespace RedditGallery.Base
{

    public class RedditResult
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("data")]
        public RedditResultData Data { get; set; }
    }

    public class RedditResultData
    {
        [JsonPropertyName("after")]
        public string After { get; set; }

        [JsonPropertyName("dist")]
        public long Dist { get; set; }

        [JsonPropertyName("modhash")]
        public string Modhash { get; set; }

        [JsonPropertyName("geo_filter")]
        public string GeoFilter { get; set; }

        [JsonPropertyName("children")]
        public Child[] Children { get; set; }

        [JsonPropertyName("before")]
        public string Before { get; set; }
    }

    public class Child
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("data")]
        public ChildData Data { get; set; }
    }

    public class ChildData
    {
        [JsonPropertyName("approved_at_utc")]
        public object ApprovedAtUtc { get; set; }

        [JsonPropertyName("subreddit")]
        public string Subreddit { get; set; }

        [JsonPropertyName("selftext")]
        public string Selftext { get; set; }

        [JsonPropertyName("author_fullname")]
        public string AuthorFullname { get; set; }

        [JsonPropertyName("saved")]
        public bool Saved { get; set; }

        [JsonPropertyName("mod_reason_title")]
        public object ModReasonTitle { get; set; }

        [JsonPropertyName("gilded")]
        public long Gilded { get; set; }

        [JsonPropertyName("clicked")]
        public bool Clicked { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("link_flair_richtext")]
        public LinkFlairRichtext[] LinkFlairRichtext { get; set; }

        [JsonPropertyName("subreddit_name_prefixed")]
        public string SubredditNamePrefixed { get; set; }

        [JsonPropertyName("hidden")]
        public bool Hidden { get; set; }

        [JsonPropertyName("pwls")]
        public long? Pwls { get; set; }

        [JsonPropertyName("link_flair_css_class")]
        public string LinkFlairCssClass { get; set; }

        [JsonPropertyName("downs")]
        public long Downs { get; set; }

        [JsonPropertyName("thumbnail_height")]
        public long? ThumbnailHeight { get; set; }

        [JsonPropertyName("top_awarded_type")]
        public object TopAwardedType { get; set; }

        [JsonPropertyName("hide_score")]
        public bool HideScore { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("quarantine")]
        public bool Quarantine { get; set; }

        [JsonPropertyName("link_flair_text_color")]
        public string LinkFlairTextColor { get; set; }

        [JsonPropertyName("upvote_ratio")]
        public double UpvoteRatio { get; set; }

        [JsonPropertyName("author_flair_background_color")]
        public string AuthorFlairBackgroundColor { get; set; }

        [JsonPropertyName("ups")]
        public long Ups { get; set; }

        [JsonPropertyName("total_awards_received")]
        public long TotalAwardsReceived { get; set; }

        [JsonPropertyName("media_embed")]
        public MediaEmbed MediaEmbed { get; set; }

        [JsonPropertyName("thumbnail_width")]
        public long? ThumbnailWidth { get; set; }

        [JsonPropertyName("author_flair_template_id")]
        public Guid? AuthorFlairTemplateId { get; set; }

        [JsonPropertyName("is_original_content")]
        public bool IsOriginalContent { get; set; }

        [JsonPropertyName("user_reports")]
        public object[] UserReports { get; set; }

        [JsonPropertyName("secure_media")]
        public Media SecureMedia { get; set; }

        [JsonPropertyName("is_reddit_media_domain")]
        public bool IsRedditMediaDomain { get; set; }

        [JsonPropertyName("is_meta")]
        public bool IsMeta { get; set; }

        [JsonPropertyName("category")]
        public object Category { get; set; }

        [JsonPropertyName("secure_media_embed")]
        public MediaEmbed SecureMediaEmbed { get; set; }

        [JsonPropertyName("link_flair_text")]
        public string LinkFlairText { get; set; }

        [JsonPropertyName("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonPropertyName("score")]
        public long Score { get; set; }

        [JsonPropertyName("approved_by")]
        public object ApprovedBy { get; set; }

        [JsonPropertyName("is_created_from_ads_ui")]
        public bool IsCreatedFromAdsUi { get; set; }

        [JsonPropertyName("author_premium")]
        public bool AuthorPremium { get; set; }

        [JsonPropertyName("thumbnail")]
        public Uri Thumbnail { get; set; }

        [JsonPropertyName("edited")]
        public bool Edited { get; set; }

        [JsonPropertyName("author_flair_css_class")]
        public string AuthorFlairCssClass { get; set; }

        [JsonPropertyName("author_flair_richtext")]
        public AuthorFlairRichtext[] AuthorFlairRichtext { get; set; }

        [JsonPropertyName("gildings")]
        public Gildings Gildings { get; set; }

        [JsonPropertyName("post_hint")]
        public string PostHint { get; set; }

        [JsonPropertyName("content_categories")]
        public object ContentCategories { get; set; }

        [JsonPropertyName("is_self")]
        public bool IsSelf { get; set; }

        [JsonPropertyName("subreddit_type")]
        public string SubredditType { get; set; }

        [JsonPropertyName("created")]
        public double Created { get; set; }

        [JsonPropertyName("link_flair_type")]
        public string LinkFlairType { get; set; }

        [JsonPropertyName("wls")]
        public long Wls { get; set; }

        [JsonPropertyName("removed_by_category")]
        public object RemovedByCategory { get; set; }

        [JsonPropertyName("banned_by")]
        public object BannedBy { get; set; }

        [JsonPropertyName("author_flair_type")]
        public string AuthorFlairType { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("allow_live_comments")]
        public bool AllowLiveComments { get; set; }

        [JsonPropertyName("selftext_html")]
        public object SelftextHtml { get; set; }

        [JsonPropertyName("likes")]
        public object Likes { get; set; }

        [JsonPropertyName("suggested_sort")]
        public object SuggestedSort { get; set; }

        [JsonPropertyName("banned_at_utc")]
        public object BannedAtUtc { get; set; }

        [JsonPropertyName("url_overridden_by_dest")]
        public Uri UrlOverriddenByDest { get; set; }

        [JsonPropertyName("view_count")]
        public object ViewCount { get; set; }

        [JsonPropertyName("archived")]
        public bool Archived { get; set; }

        [JsonPropertyName("no_follow")]
        public bool NoFollow { get; set; }

        [JsonPropertyName("is_crosspostable")]
        public bool IsCrosspostable { get; set; }

        [JsonPropertyName("pinned")]
        public bool Pinned { get; set; }

        [JsonPropertyName("over_18")]
        public bool Over18 { get; set; }

        [JsonPropertyName("preview")]
        public Preview Preview { get; set; }

        [JsonPropertyName("all_awardings")]
        public AllAwarding[] AllAwardings { get; set; }

        [JsonPropertyName("awarders")]
        public object[] Awarders { get; set; }

        [JsonPropertyName("media_only")]
        public bool MediaOnly { get; set; }

        [JsonPropertyName("link_flair_template_id")]
        public Guid LinkFlairTemplateId { get; set; }

        [JsonPropertyName("can_gild")]
        public bool CanGild { get; set; }

        [JsonPropertyName("spoiler")]
        public bool Spoiler { get; set; }

        [JsonPropertyName("locked")]
        public bool Locked { get; set; }

        [JsonPropertyName("author_flair_text")]
        public string AuthorFlairText { get; set; }

        [JsonPropertyName("treatment_tags")]
        public object[] TreatmentTags { get; set; }

        [JsonPropertyName("visited")]
        public bool Visited { get; set; }

        [JsonPropertyName("removed_by")]
        public object RemovedBy { get; set; }

        [JsonPropertyName("mod_note")]
        public object ModNote { get; set; }

        [JsonPropertyName("distinguished")]
        public object Distinguished { get; set; }

        [JsonPropertyName("subreddit_id")]
        public string SubredditId { get; set; }

        [JsonPropertyName("author_is_blocked")]
        public bool AuthorIsBlocked { get; set; }

        [JsonPropertyName("mod_reason_by")]
        public object ModReasonBy { get; set; }

        [JsonPropertyName("num_reports")]
        public object NumReports { get; set; }

        [JsonPropertyName("removal_reason")]
        public object RemovalReason { get; set; }

        [JsonPropertyName("link_flair_background_color")]
        public string LinkFlairBackgroundColor { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("is_robot_indexable")]
        public bool IsRobotIndexable { get; set; }

        [JsonPropertyName("report_reasons")]
        public object ReportReasons { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("discussion_type")]
        public object DiscussionType { get; set; }

        [JsonPropertyName("num_comments")]
        public long NumComments { get; set; }

        [JsonPropertyName("send_replies")]
        public bool SendReplies { get; set; }

        [JsonPropertyName("whitelist_status")]
        public string WhitelistStatus { get; set; }

        [JsonPropertyName("contest_mode")]
        public bool ContestMode { get; set; }

        [JsonPropertyName("mod_reports")]
        public object[] ModReports { get; set; }

        [JsonPropertyName("author_patreon_flair")]
        public bool AuthorPatreonFlair { get; set; }

        [JsonPropertyName("author_flair_text_color")]
        public string AuthorFlairTextColor { get; set; }

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; }

        [JsonPropertyName("parent_whitelist_status")]
        public string ParentWhitelistStatus { get; set; }

        [JsonPropertyName("stickied")]
        public bool Stickied { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("subreddit_subscribers")]
        public long SubredditSubscribers { get; set; }

        [JsonPropertyName("created_utc")]
        public double CreatedUtc { get; set; }

        [JsonPropertyName("num_crossposts")]
        public long NumCrossposts { get; set; }

        [JsonPropertyName("media")]
        public Media Media { get; set; }

        [JsonPropertyName("is_video")]
        public bool IsVideo { get; set; }
    }

    public class AllAwarding
    {
        [JsonPropertyName("giver_coin_reward")]
        public object GiverCoinReward { get; set; }

        [JsonPropertyName("subreddit_id")]
        public object SubredditId { get; set; }

        [JsonPropertyName("is_new")]
        public bool IsNew { get; set; }

        [JsonPropertyName("days_of_drip_extension")]
        public object DaysOfDripExtension { get; set; }

        [JsonPropertyName("coin_price")]
        public long CoinPrice { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("penny_donate")]
        public object PennyDonate { get; set; }

        [JsonPropertyName("award_sub_type")]
        public string AwardSubType { get; set; }

        [JsonPropertyName("coin_reward")]
        public long CoinReward { get; set; }

        [JsonPropertyName("icon_url")]
        public Uri IconUrl { get; set; }

        [JsonPropertyName("days_of_premium")]
        public object DaysOfPremium { get; set; }

        [JsonPropertyName("tiers_by_required_awardings")]
        public object TiersByRequiredAwardings { get; set; }

        [JsonPropertyName("resized_icons")]
        public ResizedIcon[] ResizedIcons { get; set; }

        [JsonPropertyName("icon_width")]
        public long IconWidth { get; set; }

        [JsonPropertyName("static_icon_width")]
        public long StaticIconWidth { get; set; }

        [JsonPropertyName("start_date")]
        public object StartDate { get; set; }

        [JsonPropertyName("is_enabled")]
        public bool IsEnabled { get; set; }

        [JsonPropertyName("awardings_required_to_grant_benefits")]
        public object AwardingsRequiredToGrantBenefits { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("end_date")]
        public object EndDate { get; set; }

        [JsonPropertyName("subreddit_coin_reward")]
        public long SubredditCoinReward { get; set; }

        [JsonPropertyName("count")]
        public long Count { get; set; }

        [JsonPropertyName("static_icon_height")]
        public long StaticIconHeight { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("resized_static_icons")]
        public ResizedIcon[] ResizedStaticIcons { get; set; }

        [JsonPropertyName("icon_format")]
        public string IconFormat { get; set; }

        [JsonPropertyName("icon_height")]
        public long IconHeight { get; set; }

        [JsonPropertyName("penny_price")]
        public object PennyPrice { get; set; }

        [JsonPropertyName("award_type")]
        public string AwardType { get; set; }

        [JsonPropertyName("static_icon_url")]
        public Uri StaticIconUrl { get; set; }
    }

    public class ResizedIcon
    {
        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }
    }

    public class AuthorFlairRichtext
    {
        [JsonPropertyName("a")]
        public string A { get; set; }

        [JsonPropertyName("e")]
        public string E { get; set; }

        [JsonPropertyName("u")]
        public Uri U { get; set; }

        [JsonPropertyName("t")]
        public string T { get; set; }
    }

    public class Gildings
    {
        [JsonPropertyName("gid_1")]
        public long? Gid1 { get; set; }
    }

    public class LinkFlairRichtext
    {
        [JsonPropertyName("e")]
        public string E { get; set; }

        [JsonPropertyName("t")]
        public string T { get; set; }
    }

    public class Media
    {
        [JsonPropertyName("reddit_video")]
        public RedditVideo RedditVideo { get; set; }
    }

    public class RedditVideo
    {
        [JsonPropertyName("bitrate_kbps")]
        public long BitrateKbps { get; set; }

        [JsonPropertyName("fallback_url")]
        public Uri FallbackUrl { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("scrubber_media_url")]
        public Uri ScrubberMediaUrl { get; set; }

        [JsonPropertyName("dash_url")]
        public Uri DashUrl { get; set; }

        [JsonPropertyName("duration")]
        public long Duration { get; set; }

        [JsonPropertyName("hls_url")]
        public Uri HlsUrl { get; set; }

        [JsonPropertyName("is_gif")]
        public bool IsGif { get; set; }

        [JsonPropertyName("transcoding_status")]
        public string TranscodingStatus { get; set; }
    }

    public class MediaEmbed
    {
    }

    public class Preview
    {
        [JsonPropertyName("images")]
        public RedditImage[] Images { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }

    public class RedditImage
    {
        [JsonPropertyName("source")]
        public ResizedIcon Source { get; set; }

        [JsonPropertyName("resolutions")]
        public ResizedIcon[] Resolutions { get; set; }

        [JsonPropertyName("variants")]
        public Variants Variants { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class Variants
    {
        [JsonPropertyName("gif")]
        public Gif Gif { get; set; }

        [JsonPropertyName("mp4")]
        public Gif Mp4 { get; set; }
    }

    public class Gif
    {
        [JsonPropertyName("source")]
        public ResizedIcon Source { get; set; }

        [JsonPropertyName("resolutions")]
        public ResizedIcon[] Resolutions { get; set; }
    }
}
