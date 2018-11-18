**Release Notes**

# usis.Net.Bits

---

## Next Release

- `BackgroundCopyFile.TemporaryName` property added.
- `BackgroundCopyFile.ValidationState` property added.
- `BackgroundCopyFile.IsDownloadedFromPeer` property added.

---

## Version 2.5.0 - 2018-11-18

- `BackgroundCopyManager.LibraryVersion` property added.
- BITS 2.5 API implemented: see `BackgroundCopyJob.HttpOptions` property.

---

## Version 2.1.0 - 2018-10-27

- `BackgroundCopyFile.RemoteName` property setter added.
- `BackgroundCopyFile.RetrieveRanges` method added.

---

## Version 2.0.0 - 2018-10-21

- `BackgroundCopyJobFileAclOptions` enumeration added.
- `BackgroundCopyJob.FileAcl` property added.
- `BackgroundCopyJob.ReplaceRemotePrefix` method added.
- Fix for the exception that is thrown when calling `BackgroundCopyJob.SetCredentials` from a 32-bit application.
- `BackgroundCopyFileRange` class added.
- `BackgroundCopyJob.AddFile` overloads added.

---

## Version 1.1.2 - 2018-01-23

- .NET Framework 4.5
- Obsolete members removed.
- `BackgroundCopyManager.EnumerateJobs` override.

---

## Version 1.1.1 - 2018-01-20

- Corrected LCID-Fallback in `BackgroundCopyError.Description` and `BackgroundCopyError.ContextDescription` when MUI file was not found or loaded.
- Adding an event handler to a job without permission (access denied) does not throw an exception.

---

## Version 1.1.0 - 2017-11-04

- `BackgroundCopyJobType.UploadReply` added. 
- `BackgroundCopyNotifyCommandLine` class added.
- `BackgroundCopyJob.NotifyCommandLine` property added.
- `BackgroundCopyJob.ReplyFileName` property added.
- `BackgroundCopyJobReplyProgress` class added.
- `BackgroundCopyJob.RetrieveReplyProgress` method added.
- `BackgroundCopyJob.ReplyFileName` getter throws no exception for other job types than `UploadReply`.
- `BackgroundCopyJob.RetrieveReplyData` method added.
- `BackgroundCopyAuthenticationTarget` enumeration added.
- `BackgroundCopyAuthenticationScheme` enumeration added.
- Even dispose when methods returning `RPC_E_DISCONNECTED`.
- `BackgroundCopyJob.SetCredentials` method added.
- `BackgroundCopyJob.RemoveCredentials` method added.

---

## Version 1.0.2 - 2017-10-13

- `BackgroundCopyManager.GetJob(Guid, bool)` method overload added.
- `BackgroundCopyManager.Version` property added.

---

## Version 1.0.1 - 2017-10-10

- `BackgroundCopyJob.AddFiles` method added.
- `BackgroundCopyJobType.UploadReply` removed (not yet supported).
- `BackgroundCopyJobProxySettings` constructor added.
- `BackgroundCopyJobProxySettings` property setters removed.
- `BackgroundCopyJob.ProxySettings` property added.
- `BackgroundCopyJob.RetrieveProxySettings` deprecated.
- `BackgroundCopyJob.SetProxySettings` deprecated.

---

## Version 1.0.0 - 2017-10-07

This was the initial release.

---
