using System.Text.Json.Serialization;

namespace SetTheDate.Models
{
    public class WaSenderApiModel
    {

    }
    public class WebhookPayload
    {
        [JsonPropertyName("data")]
        public WebhookData? Data { get; set; }
    }

    public class WebhookData
    {
        [JsonPropertyName("messages")]
        public MessageData? Messages { get; set; }
    }

    public class MessageData
    {
        [JsonPropertyName("key")]
        public MessageKey? Key { get; set; }

        [JsonPropertyName("messageBody")]
        public string? MessageBody { get; set; }

        [JsonPropertyName("message")]
        public Message? Message { get; set; }

        [JsonPropertyName("remoteJid")]
        public string? RemoteJid { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }

    public class MessageKey
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("fromMe")]
        public bool FromMe { get; set; }

        [JsonPropertyName("remoteJid")]
        public string RemoteJid { get; set; } = string.Empty;

        [JsonPropertyName("senderPn")]
        public string? SenderPn { get; set; }

        [JsonPropertyName("cleanedSenderPn")]
        public string? CleanedSenderPn { get; set; }

        [JsonPropertyName("cleanedParticipantPn")]
        public string? CleanedParticipantPn { get; set; }

        [JsonPropertyName("senderLid")]
        public string? SenderLid { get; set; }

        [JsonPropertyName("addressingMode")]
        public string? AddressingMode { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("conversation")]
        public string? Conversation { get; set; }

        [JsonPropertyName("messageContextInfo")]
        public MessageContextInfo? MessageContextInfo { get; set; }
    }

    public class MessageContextInfo
    {
        [JsonPropertyName("deviceListMetadata")]
        public DeviceListMetadata? DeviceListMetadata { get; set; }

        [JsonPropertyName("deviceListMetadataVersion")]
        public int DeviceListMetadataVersion { get; set; }

        // You can add other fields if needed
    }

    public class DeviceListMetadata
    {
        [JsonPropertyName("senderKeyHash")]
        public string? SenderKeyHash { get; set; }

        [JsonPropertyName("senderTimestamp")]
        public string? SenderTimestamp { get; set; }

        [JsonPropertyName("senderAccountType")]
        public string? SenderAccountType { get; set; }

        [JsonPropertyName("receiverAccountType")]
        public string? ReceiverAccountType { get; set; }

        [JsonPropertyName("recipientKeyHash")]
        public string? RecipientKeyHash { get; set; }

        [JsonPropertyName("recipientTimestamp")]
        public string? RecipientTimestamp { get; set; }
    }

}
