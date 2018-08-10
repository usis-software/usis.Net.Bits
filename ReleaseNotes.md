﻿##### Release Notes

### usis.Net.Bits

---

#### Version 1.1.2 - 2018-01-23

- .NET Framework 4.5
- Obsolete members removed.
- BackgroundCopyManager.EnumerateJobs override.

---

#### Version 1.1.1 - 2018-01-20

- Correct LCID-Fallback in BackgroundCopyError.Description and BackgroundCopyError.ContextDescription when MUI file not found or nor loaded.
- Adding an event handler to a job without permission (access denied) does not throw an exception.

---

#### Version 1.1.0 - 2017-11-04

- BackgroundCopyNotifyCommandLine class added.
- BackgroundCopyJob.NotifyCommandLine property added.
- BackgroundCopyJob.ReplyFileName property added.
- BackgroundCopyJobReplyProgress class added.
- BackgroundCopyJob.RetrieveReplyProgress method added.
- BackgroundCopyJob.ReplyFileName getter throws no exception for other job types that UploadReply.
- BackgroundCopyJob.RetrieveReplyData method added.
- BackgroundCopyAuthenticationTarget enumeration added.
- BackgroundCopyAuthenticationScheme enumeration added.
- Even dispose when methods returning RPC_E_DISCONNECTED.
- BackgroundCopyJob.SetCredentials method added.
- BackgroundCopyJob.RemoveCredentials method added.

---

#### Version 1.0.2 - 2017-10-13

- BackgroundCopyManager.GetJob(Guid, bool) method overload added.
- BackgroundCopyManager.Version property added.

---

#### Version 1.0.1 - 2017-10-10

- BackgroundCopyJob.AddFiles method added.
- BackgroundCopyJobType.UploadReply removed (not yet supported).
- BackgroundCopyJobProxySettings constructor added.
- BackgroundCopyJobProxySettings property setters removed.
- BackgroundCopyJob.ProxySettings property added.
- BackgroundCopyJob.RetrieveProxySettings deprecated.
- BackgroundCopyJob.SetProxySettings deprecated.

---

#### Version 1.0.0 - 2017-10-07

This was the initial release.

---