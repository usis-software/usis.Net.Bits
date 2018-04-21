About usis Push Notification Router
===================================

The usis Push Notification Router provides services to send push notifications to mobile devices without the need to deal with the complexity of the underlying technology and infrastructure.

usisPNRouter is Windows service application that can operate on-premises or in the cloud on a Windows server.

It provides an API as a web service for devices to register themselves to receive push notifications.

On the server side an Microsoft Management Console snap-in allows the administration of push notification channels. These channels are responsible to dispatch and forward notifications to the appropriate platform-specific notification service (i.e. APNs - Apple Push Notification service). The configuration of these channels include the necessary certificates to authenticate with the notification services. These certificates are stored in a local database on the server and are installed in the local certificate store.

The API to send notifications is a web service that can be easily used by any application back-end. It allows sending notification to a single device or broadcast notifications to all registered devices.

All notification are stored in a database and then send to the platform notification service. The API provides methods to retrieve the state of the notification that indicates whether the notification was sent or an error occurred.

Requirements
============

- Windows Server 2012 R2
- .NET Framework 4.6 or higher
- Supported database provider: Microsoft SQL Server or MySQL

Samples
=======

List devices registered for an APNs channel
-------------------------------------------

static void ListDevices()
{
    var baseAddress = new Uri("http://<server>/APNsRouter");

    var binding = new WebHttpBinding(WebHttpSecurityMode.TransportCredentialOnly);
    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

    var channelFactory = new WebChannelFactory<IAPNsRouter>(binding, baseAddress);

    channelFactory.Credentials.UserName.UserName = "<username>";
    channelFactory.Credentials.UserName.Password = "<password>";

    var client = channelFactory.CreateChannel();

    var result = client.ListDevices("<bundleId>", usis.PushNotification.Environment.Development);
    foreach (var device in result.ReturnValue)
    {
        Console.WriteLine(device);
    }
}

ToDo
====

- Shutdown on failed SnapIn connection (i.e. MySql .NET Provider not installed).
- PNRouter service with Receiver/Solution API to send notifications.
- SQLite database
- https://msdn.microsoft.com/en-us/library/windows/apps/hh913756.aspx

Done:
=====
- Uninstall, delete certificates
- APNsRouter API to get device list
- Close channel when new certificate is uploaded or installed.
- Timed background task to requeue failed notifications
- Exception in CreateApnsConfiguration when using a sandbox certificate for a production channel.
- Authentication for APNsRouter
- Uninstall certificate only if it is the last in certificate store (thumbprint)
