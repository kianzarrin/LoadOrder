namespace LoadOrderTool.Util {
    using CO.PlatformServices;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using CO.IO;
    using System.Diagnostics;

    public static class SteamUtil {
        public static void ExecuteSteam(string args) {
            ContentUtil.Execute(DataLocation.SteamPath, DataLocation.SteamExe, args).WaitForExit();
            Thread.Sleep(10);
        }

        public static void ReDownload(IEnumerable<ulong> ids) {
            try {
                Log.Called(ids);
                foreach (var id in ids)
                    ExecuteSteam($"+workshop_download_item 255710 {id}");
                ExecuteSteam("steam://open/console");// so that user can see what is happening.
                ExecuteSteam("steam://open/downloads");
            } catch(Exception ex) { ex.Log(); }
        }


        public static PublishedFileDTO[] HttpResponse2DTOs(string httpResponse) {
            Log.Info("parsing response to json ...");
            dynamic json = JContainer.Parse(httpResponse);

            Log.Info($"result:{json.response.result}\nconverting json to DTO ... ");
            if (json.response.result == EResult.k_EResultOK) {
                JArray publishedfiledetails = json.response.publishedfiledetails;
                return publishedfiledetails
                    .Select(item => new PublishedFileDTO(item))
                    .Where(item => item.Result == EResult.k_EResultOK) // ignore deleted WS items
                    .ToArray();
            }
            return null;
        }

        public static string ExtractPersonaNameFromHTML(string html) {
            Log.Called(/*html*/);
            var pattern = "<span class=\"actual_persona_name\">([^<>]+)</span>";
            var match = Regex.Matches(html, "<span class=\"actual_persona_name\">([^<>]+)</span>").FirstOrDefault();
            if (match != null) {
                var ret = match.Groups[1].Value;
                ret.LogRet(match.Groups[0].Value);
                return ret;
            } else {
                throw new Exception(
                    $"No match found!\n" +
                    $"Pattern= {pattern}\n" +
                    $"html={html}");
            }
        }

        public static async IAsyncEnumerable<PublishedFileDTO[]> LoadDataAsyncInChunks(PublishedFileId[] ids, int chunkSize = 1000) {
            int i;
            for(i = 0; i + chunkSize < ids.Length; i += chunkSize) {
                var buffer = new PublishedFileId[chunkSize];
                Array.Copy(ids, i, buffer, 0, chunkSize);
                var data = await LoadDataAsync(buffer);
                yield return data;
            }
            int r = ids.Length - i;
            if(r > 0) {

                var buffer = new PublishedFileId[r];
                Array.Copy(ids, i, buffer, 0, r);
                var data = await LoadDataAsync(buffer);
                yield return data;
            }
        }

        public static async Task<PublishedFileDTO[]> LoadDataAsync(PublishedFileId[] ids) {
            using (var httpClient = new HttpClient()) {
                var url = @"https://api.steampowered.com/ISteamRemoteStorage/GetPublishedFileDetails/v1/";

                var dict = new Dictionary<string, string>();
                dict["itemcount"] = ids.Length.ToString();
                for (int i = 0; i < ids.Length; ++i) {
                    dict[$"publishedfileids[{i}]"] = ids[i].ToString();
                }
                var data = new FormUrlEncodedContent(dict);

                var httpResponse = await httpClient.PostAsync(url, data);

                if (httpResponse.IsSuccessStatusCode) {
                    string httpResponse2 = await httpResponse.Content.ReadAsStringAsync();
                    return await Task.Run(() => HttpResponse2DTOs(httpResponse2));
                }
                Log.Error("failed to get httpResponse");
                new Dictionary<object, object>();
                return null;
            }
        }


        public class HttpWrapper : IDisposable {
            public readonly int MaxConnections;
            public HttpClient HttpClient { get; private set; } = new HttpClient();
            public SemaphoreSlim HttpSem { get; private set; }
            public int FailureCount;

            public HttpWrapper(int connections = 6) {
                MaxConnections = connections;
                HttpSem = new SemaphoreSlim(MaxConnections);
                //ServicePointManager.FindServicePoint(new Uri("https://steamcommunity.com/profiles/")).ConnectionLimit = MaxConnections;
            }

            private bool disposedValue;

            protected virtual void Dispose(bool disposing) {
                if (!disposedValue) {
                    if (disposing) {
                        HttpClient.Dispose();
                        HttpSem.Dispose();
                    }

                    HttpClient = null;
                    HttpSem = null;

                    disposedValue = true;
                }
            }

            ~HttpWrapper() {
                Dispose(disposing: false);
            }

            public void Dispose() {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }

        public static async Task<string> GetPersonaNameAsync(this HttpWrapper httpWrapper, ulong personaID) {
            await httpWrapper.HttpSem.WaitAsync();
            try {
                if (httpWrapper.FailureCount >= httpWrapper.MaxConnections) return null;
                string url = $@"https://steamcommunity.com/profiles/{personaID}";
                var http = await httpWrapper.HttpClient.GetStringAsync(url);
                var ret = await Task.Run(() => ExtractPersonaNameFromHTML(http));
                httpWrapper.FailureCount = 0; // hope is restored!
                return ret;
            } catch (Exception ex) {
                httpWrapper.FailureCount++; // hope is fading!
                throw new Exception($"GetPersonaName({personaID}) failed", ex);
            } finally {
                httpWrapper.HttpSem.Release();
            }
        }

        public static string GetPersonaName(this HttpWrapper httpWrapper, ulong personaID) {
            if (httpWrapper.FailureCount >= httpWrapper.MaxConnections) return null;
            httpWrapper.HttpSem.Wait();
            try {
                string url = $@"https://steamcommunity.com/profiles/{personaID}";
                var http = httpWrapper.HttpClient.GetStringAsync(url).Result;
                var ret = ExtractPersonaNameFromHTML(http);
                httpWrapper.FailureCount = 0; // hope is restored!
                return ret;
            } catch (Exception ex) {
                httpWrapper.FailureCount++; // hope is fading!
                throw new Exception($"GetPersonaName({personaID}) failed", ex);
            } finally {
                httpWrapper.HttpSem.Release();
            }
        }

        public class PublishedFileDTO {
            public static DateTime kEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            public static DateTime ToUTCTime(ulong time) => kEpoch.AddSeconds(time);


            public EResult Result;
            public ulong PublishedFileID;
            public string Title;
            public ulong AuthorID;
            public DateTime UpdatedUTC;
            public ulong Size;
            public string PreviewURL;
            public string[] Tags;
            public PublishedFileDTO(dynamic publishedfiledetail) {
                Result = (EResult)publishedfiledetail.result;
                if (Result == EResult.k_EResultOK) {
                    Title = publishedfiledetail.title;
                    PublishedFileID = publishedfiledetail.publishedfileid;
                    Size = publishedfiledetail.file_size;
                    PreviewURL = publishedfiledetail.preview_url;
                    AuthorID = publishedfiledetail.creator;
                    UpdatedUTC = ToUTCTime((ulong)publishedfiledetail.time_updated);
                    Tags = (publishedfiledetail.tags as JArray)
                        ?.Select(item => (string)item["tag"])
                        ?.Where(item => !item.Contains("compatible", StringComparison.OrdinalIgnoreCase))
                        ?.ToArray();
                    //Log.Debug($"item[{PublishedFileID}]: Date Updated = {publishedfiledetail.time_updated}ticks =>  {Updated}");
                }
            }
        }

        public class LoggingHandler : DelegatingHandler {
            public static HttpClient CreateTracedHttpClient() => new HttpClient(new LoggingHandler(new HttpClientHandler()));
            public LoggingHandler(HttpMessageHandler innerHandler)
                : base(innerHandler) {
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
                Log.Debug("Request:");
                Console.WriteLine(request.ToString());
                if (request.Content != null) {
                    Console.WriteLine(await request.Content.ReadAsStringAsync());
                }
                Console.WriteLine();

                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

                Log.Debug("Response:");
                Console.WriteLine(response.ToString());
                if (response.Content != null) {
                    Log.Debug(await response.Content.ReadAsStringAsync());
                }
                Console.WriteLine();

                return response;
            }
        }



        // General result codes
        public enum EResult {
            k_EResultOK = 1,                            // success
            k_EResultFail = 2,                          // generic failure 
            k_EResultNoConnection = 3,                  // no/failed network connection
                                                        //	k_EResultNoConnectionRetry = 4,				// OBSOLETE - removed
            k_EResultInvalidPassword = 5,               // password/ticket is invalid
            k_EResultLoggedInElsewhere = 6,             // same user logged in elsewhere
            k_EResultInvalidProtocolVer = 7,            // protocol version is incorrect
            k_EResultInvalidParam = 8,                  // a parameter is incorrect
            k_EResultFileNotFound = 9,                  // file was not found
            k_EResultBusy = 10,                         // called method busy - action not taken
            k_EResultInvalidState = 11,                 // called object was in an invalid state
            k_EResultInvalidName = 12,                  // name is invalid
            k_EResultInvalidEmail = 13,                 // email is invalid
            k_EResultDuplicateName = 14,                // name is not unique
            k_EResultAccessDenied = 15,                 // access is denied
            k_EResultTimeout = 16,                      // operation timed out
            k_EResultBanned = 17,                       // VAC2 banned
            k_EResultAccountNotFound = 18,              // account not found
            k_EResultInvalidSteamID = 19,               // steamID is invalid
            k_EResultServiceUnavailable = 20,           // The requested service is currently unavailable
            k_EResultNotLoggedOn = 21,                  // The user is not logged on
            k_EResultPending = 22,                      // Request is pending (may be in process, or waiting on third party)
            k_EResultEncryptionFailure = 23,            // Encryption or Decryption failed
            k_EResultInsufficientPrivilege = 24,        // Insufficient privilege
            k_EResultLimitExceeded = 25,                // Too much of a good thing
            k_EResultRevoked = 26,                      // Access has been revoked (used for revoked guest passes)
            k_EResultExpired = 27,                      // License/Guest pass the user is trying to access is expired
            k_EResultAlreadyRedeemed = 28,              // Guest pass has already been redeemed by account, cannot be acked again
            k_EResultDuplicateRequest = 29,             // The request is a duplicate and the action has already occurred in the past, ignored this time
            k_EResultAlreadyOwned = 30,                 // All the games in this guest pass redemption request are already owned by the user
            k_EResultIPNotFound = 31,                   // IP address not found
            k_EResultPersistFailed = 32,                // failed to write change to the data store
            k_EResultLockingFailed = 33,                // failed to acquire access lock for this operation
            k_EResultLogonSessionReplaced = 34,
            k_EResultConnectFailed = 35,
            k_EResultHandshakeFailed = 36,
            k_EResultIOFailure = 37,
            k_EResultRemoteDisconnect = 38,
            k_EResultShoppingCartNotFound = 39,         // failed to find the shopping cart requested
            k_EResultBlocked = 40,                      // a user didn't allow it
            k_EResultIgnored = 41,                      // target is ignoring sender
            k_EResultNoMatch = 42,                      // nothing matching the request found
            k_EResultAccountDisabled = 43,
            k_EResultServiceReadOnly = 44,              // this service is not accepting content changes right now
            k_EResultAccountNotFeatured = 45,           // account doesn't have value, so this feature isn't available
            k_EResultAdministratorOK = 46,              // allowed to take this action, but only because requester is admin
            k_EResultContentVersion = 47,               // A Version mismatch in content transmitted within the Steam protocol.
            k_EResultTryAnotherCM = 48,                 // The current CM can't service the user making a request, user should try another.
            k_EResultPasswordRequiredToKickSession = 49,// You are already logged in elsewhere, this cached credential login has failed.
            k_EResultAlreadyLoggedInElsewhere = 50,     // You are already logged in elsewhere, you must wait
            k_EResultSuspended = 51,                    // Long running operation (content download) suspended/paused
            k_EResultCancelled = 52,                    // Operation canceled (typically by user: content download)
            k_EResultDataCorruption = 53,               // Operation canceled because data is ill formed or unrecoverable
            k_EResultDiskFull = 54,                     // Operation canceled - not enough disk space.
            k_EResultRemoteCallFailed = 55,             // an remote call or IPC call failed
            k_EResultPasswordUnset = 56,                // Password could not be verified as it's unset server side
            k_EResultExternalAccountUnlinked = 57,      // External account (PSN, Facebook...) is not linked to a Steam account
            k_EResultPSNTicketInvalid = 58,             // PSN ticket was invalid
            k_EResultExternalAccountAlreadyLinked = 59, // External account (PSN, Facebook...) is already linked to some other account, must explicitly request to replace/delete the link first
            k_EResultRemoteFileConflict = 60,           // The sync cannot resume due to a conflict between the local and remote files
            k_EResultIllegalPassword = 61,              // The requested new password is not legal
            k_EResultSameAsPreviousValue = 62,          // new value is the same as the old one ( secret question and answer )
            k_EResultAccountLogonDenied = 63,           // account login denied due to 2nd factor authentication failure
            k_EResultCannotUseOldPassword = 64,         // The requested new password is not legal
            k_EResultInvalidLoginAuthCode = 65,         // account login denied due to auth code invalid
            k_EResultAccountLogonDeniedNoMail = 66,     // account login denied due to 2nd factor auth failure - and no mail has been sent
            k_EResultHardwareNotCapableOfIPT = 67,      // 
            k_EResultIPTInitError = 68,                 // 
            k_EResultParentalControlRestricted = 69,    // operation failed due to parental control restrictions for current user
            k_EResultFacebookQueryError = 70,           // Facebook query returned an error
            k_EResultExpiredLoginAuthCode = 71,         // account login denied due to auth code expired
            k_EResultIPLoginRestrictionFailed = 72,
            k_EResultAccountLockedDown = 73,
            k_EResultAccountLogonDeniedVerifiedEmailRequired = 74,
            k_EResultNoMatchingURL = 75,
            k_EResultBadResponse = 76,                  // parse failure, missing field, etc.
            k_EResultRequirePasswordReEntry = 77,       // The user cannot complete the action until they re-enter their password
            k_EResultValueOutOfRange = 78,              // the value entered is outside the acceptable range
            k_EResultUnexpectedError = 79,              // something happened that we didn't expect to ever happen
            k_EResultDisabled = 80,                     // The requested service has been configured to be unavailable
            k_EResultInvalidCEGSubmission = 81,         // The set of files submitted to the CEG server are not valid !
            k_EResultRestrictedDevice = 82,             // The device being used is not allowed to perform this action
            k_EResultRegionLocked = 83,                 // The action could not be complete because it is region restricted
            k_EResultRateLimitExceeded = 84,            // Temporary rate limit exceeded, try again later, different from k_EResultLimitExceeded which may be permanent
            k_EResultAccountLoginDeniedNeedTwoFactor = 85,  // Need two-factor code to login
            k_EResultItemDeleted = 86,                  // The thing we're trying to access has been deleted
            k_EResultAccountLoginDeniedThrottle = 87,   // login attempt failed, try to throttle response to possible attacker
            k_EResultTwoFactorCodeMismatch = 88,        // two factor code mismatch
            k_EResultTwoFactorActivationCodeMismatch = 89,  // activation code for two-factor didn't match
            k_EResultAccountAssociatedToMultiplePartners = 90,  // account has been associated with multiple partners
            k_EResultNotModified = 91,                  // data not modified
            k_EResultNoMobileDevice = 92,               // the account does not have a mobile device associated with it
            k_EResultTimeNotSynced = 93,                // the time presented is out of range or tolerance
            k_EResultSmsCodeFailed = 94,                // SMS code failure (no match, none pending, etc.)
            k_EResultAccountLimitExceeded = 95,         // Too many accounts access this resource
            k_EResultAccountActivityLimitExceeded = 96, // Too many changes to this account
            k_EResultPhoneActivityLimitExceeded = 97,   // Too many changes to this phone
            k_EResultRefundToWallet = 98,               // Cannot refund to payment method, must use wallet
            k_EResultEmailSendFailure = 99,             // Cannot send an email
            k_EResultNotSettled = 100,                  // Can't perform operation till payment has settled
        }
    }
}
