using System;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace Pamaxie.Database.Extensions.Client.Extensions
{
    internal static class HttpResponseMessageExtension
    {
        internal static bool NotBadResponse(this HttpResponseMessage message)
        {
            switch (message.StatusCode)
            {
                case HttpStatusCode.Accepted:
                    throw new WebException("");
                case HttpStatusCode.Ambiguous:
                    throw new WebException("");
                case HttpStatusCode.Continue:
                    throw new WebException("");
                case HttpStatusCode.SwitchingProtocols:
                    throw new WebException("");
                case HttpStatusCode.Processing:
                    throw new WebException("");
                case HttpStatusCode.EarlyHints:
                    throw new WebException("");
                case HttpStatusCode.OK:
                    return true;
                case HttpStatusCode.Created:
                    return true;
                case HttpStatusCode.NonAuthoritativeInformation:
                    throw new WebException("");
                case HttpStatusCode.NoContent:
                    throw new WebException("");
                case HttpStatusCode.ResetContent:
                    throw new WebException("");
                case HttpStatusCode.PartialContent:
                    throw new WebException("");
                case HttpStatusCode.MultiStatus:
                    throw new WebException("");
                case HttpStatusCode.AlreadyReported:
                    throw new WebException("");
                case HttpStatusCode.IMUsed:
                    throw new WebException("");
                case HttpStatusCode.Moved:
                    throw new WebException("");
                case HttpStatusCode.Found:
                    throw new WebException("");
                case HttpStatusCode.RedirectMethod:
                    throw new WebException("");
                case HttpStatusCode.NotModified:
                    throw new WebException("");
                case HttpStatusCode.UseProxy:
                    throw new WebException("");
                case HttpStatusCode.Unused:
                    throw new WebException("");
                case HttpStatusCode.RedirectKeepVerb:
                    throw new WebException("");
                case HttpStatusCode.PermanentRedirect:
                    throw new WebException("");
                case HttpStatusCode.BadRequest:
                    throw new WebException("");
                case HttpStatusCode.Unauthorized:
                    throw new WebException(HttpStatusCode.Unauthorized.ToString());
                case HttpStatusCode.PaymentRequired:
                    throw new WebException("");
                case HttpStatusCode.Forbidden:
                    throw new WebException("");
                case HttpStatusCode.NotFound:
                    throw new WebException("");
                case HttpStatusCode.MethodNotAllowed:
                    throw new WebException("");
                case HttpStatusCode.NotAcceptable:
                    throw new WebException("");
                case HttpStatusCode.ProxyAuthenticationRequired:
                    throw new WebException("");
                case HttpStatusCode.RequestTimeout:
                    throw new WebException("");
                case HttpStatusCode.Conflict:
                    throw new WebException("");
                case HttpStatusCode.Gone:
                    throw new WebException("");
                case HttpStatusCode.LengthRequired:
                    throw new WebException("");
                case HttpStatusCode.PreconditionFailed:
                    throw new WebException("");
                case HttpStatusCode.RequestEntityTooLarge:
                    throw new WebException("");
                case HttpStatusCode.RequestUriTooLong:
                    throw new WebException("");
                case HttpStatusCode.UnsupportedMediaType:
                    throw new WebException("");
                case HttpStatusCode.RequestedRangeNotSatisfiable:
                    throw new WebException("");
                case HttpStatusCode.ExpectationFailed:
                    throw new WebException("");
                case HttpStatusCode.MisdirectedRequest:
                    throw new WebException("");
                case HttpStatusCode.UnprocessableEntity:
                    throw new WebException("");
                case HttpStatusCode.Locked:
                    throw new WebException("");
                case HttpStatusCode.FailedDependency:
                    throw new WebException("");
                case HttpStatusCode.UpgradeRequired:
                    throw new WebException("");
                case HttpStatusCode.PreconditionRequired:
                    throw new WebException("");
                case HttpStatusCode.TooManyRequests:
                    throw new WebException("");
                case HttpStatusCode.RequestHeaderFieldsTooLarge:
                    throw new WebException("");
                case HttpStatusCode.UnavailableForLegalReasons:
                    throw new WebException("");
                case HttpStatusCode.InternalServerError:
                    throw new WebException("");
                case HttpStatusCode.NotImplemented:
                    throw new WebException("");
                case HttpStatusCode.BadGateway:
                    throw new WebException("");
                case HttpStatusCode.ServiceUnavailable:
                    throw new WebException("");
                case HttpStatusCode.GatewayTimeout:
                    throw new WebException("");
                case HttpStatusCode.HttpVersionNotSupported:
                    throw new WebException("");
                case HttpStatusCode.VariantAlsoNegotiates:
                    throw new WebException("");
                case HttpStatusCode.InsufficientStorage:
                    throw new WebException("");
                case HttpStatusCode.LoopDetected:
                    throw new WebException("");
                case HttpStatusCode.NotExtended:
                    throw new WebException("");
                case HttpStatusCode.NetworkAuthenticationRequired:
                    throw new WebException("");
                default:
#pragma warning disable CA2208
                    throw new ArgumentOutOfRangeException();
#pragma warning restore CA2208
            }
        }
    }
}