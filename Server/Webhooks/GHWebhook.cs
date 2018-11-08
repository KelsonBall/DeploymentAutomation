using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using GithubWebhook.Events;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GithubWebhook
{
    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };
    }

    public class GhWebhook
    {
        [Obsolete("This can still be used, but it is preffered to pass in the HttpRequest instead.")]
        public GhWebhook(string strEvent, string signature, string delivery, string payloadText)
        {
            Event = strEvent;
            Signature = signature;
            Delivery = delivery;
            PayloadText = payloadText;

            PayloadObject = ConvertPayload();
        }

        public GhWebhook(HttpRequest hookIn)
        {
            hookIn.Headers.TryGetValue("X-GitHub-Event", out var strEvent);
            hookIn.Headers.TryGetValue("X-Hub-Signature", out var signature);
            hookIn.Headers.TryGetValue("X-GitHub-Delivery", out var delivery);
            hookIn.Headers.TryGetValue("Content-type", out var content);

            Event = strEvent;
            Signature = signature;
            Delivery = delivery;

            if (content != "application/json")
            {
                throw new Exception("Invalid content type. Expected application/json");
            }

            using (var reader = new StreamReader(hookIn.Body, Encoding.UTF8))
            {
                PayloadText = reader.ReadToEnd();
            }

            PayloadObject = ConvertPayload();
        }

        public string Delivery { get; }
        public string Event { get; }
        public object PayloadObject { get; }

        public string PayloadText { get; }
        public string Signature { get; }

        private static string ValidateSignature(string payload, string signatureWithPrefix, string secret)
        {
            if (!signatureWithPrefix.StartsWith("sha1=", StringComparison.OrdinalIgnoreCase))
                return "Invalid shaPrefix";

            var secretBytes = Encoding.UTF8.GetBytes(secret);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            using (var hmSha1 = new HMACSHA1(secretBytes))
            {
                var hash = hmSha1.ComputeHash(payloadBytes);

                return $"sha1={ToHexString(hash)}";
            }
        }

        private static string ToHexString(IReadOnlyCollection<byte> bytes)
        {
            var builder = new StringBuilder(bytes.Count * 2);
            foreach (var b in bytes) builder.AppendFormat("{0:x2}", b);

            return builder.ToString();
        }

        private object ConvertPayload()
        {
            switch (Event)
            {
                case PingEvent.EventString:
                    return PingEvent.FromJson(PayloadText);
                case CommitCommentEvent.EventString:
                    return CommitCommentEvent.FromJson(PayloadText);
                case CreateEvent.EventString:
                    return CreateEvent.FromJson(PayloadText);
                case DeleteEvent.EventString:
                    return DeleteEvent.FromJson(PayloadText);
                case DeploymentEvent.EventString:
                    return DeploymentEvent.FromJson(PayloadText);
                case DeploymentStatusEvent.EventString:
                    return DeploymentStatusEvent.FromJson(PayloadText);
                case ForkEvent.EventString:
                    return ForkEvent.FromJson(PayloadText);
                case GollumEvent.EventString:
                    return GollumEvent.FromJson(PayloadText);
                case InstallationEvent.EventString:
                    return InstallationEvent.FromJson(PayloadText);
                case InstallationRepositoriesEvent.EventString:
                    return InstallationRepositoriesEvent.FromJson(PayloadText);
                case IssueCommentEvent.EventString:
                    return IssueCommentEvent.FromJson(PayloadText);
                case IssuesEvent.EventString:
                    return IssuesEvent.FromJson(PayloadText);
                case LabelEvent.EventString:
                    return LabelEvent.FromJson(PayloadText);
                case MemberEvent.EventString:
                    return MemberEvent.FromJson(PayloadText);
                case MembershipEvent.EventString:
                    return MembershipEvent.FromJson(PayloadText);
                case MilestoneEvent.EventString:
                    return MilestoneEvent.FromJson(PayloadText);
                case OrganizationEvent.EventString:
                    return OrganizationEvent.FromJson(PayloadText);
                case OrgBlockEvent.EventString:
                    return OrgBlockEvent.FromJson(PayloadText);
                case PageBuildEvent.EventString:
                    return PageBuildEvent.FromJson(PayloadText);
                case ProjectCardEvent.EventString:
                    return ProjectCardEvent.FromJson(PayloadText);
                case ProjectColumnEvent.EventString:
                    return ProjectColumnEvent.FromJson(PayloadText);
                case ProjectEvent.EventString:
                    return ProjectEvent.FromJson(PayloadText);
                case PublicEvent.EventString:
                    return PublicEvent.FromJson(PayloadText);
                case PullRequestEvent.EventString:
                    return PullRequestEvent.FromJson(PayloadText);
                case PullRequestReviewEvent.EventString:
                    return PullRequestReviewEvent.FromJson(PayloadText);
                case PullRequestReviewCommentEvent.EventString:
                    return PullRequestReviewCommentEvent.FromJson(PayloadText);
                case PushEvent.EventString:
                    return PushEvent.FromJson(PayloadText);
                case ReleaseEvent.EventString:
                    return ReleaseEvent.FromJson(PayloadText);
                case RepositoryEvent.EventString:
                    return RepositoryEvent.FromJson(PayloadText);
                case StatusEvent.EventString:
                    return StatusEvent.FromJson(PayloadText);
                case WatchEvent.EventString:
                    return WatchEvent.FromJson(PayloadText);
                default:
                    throw new NotImplementedException(
                        $"Event Type: `{Event}` is not implemented. Want it added? Open an issue at https://github.com/promofaux/GithubWebhooks");
            }
        }


        public bool SignatureValid(string clientSecret)
        {
            return ValidateSignature(PayloadText, Signature, clientSecret) == Signature;
        }

        public string GetExpectedSignature(string clientSecret)
        {
            return ValidateSignature(PayloadText, Signature, clientSecret);
        }
    }
}