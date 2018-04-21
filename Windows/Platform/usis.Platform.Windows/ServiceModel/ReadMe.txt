Configure a WFC client
======================

Using BasicHttpBinding
----------------------

<?xml version="1.0" encoding="utf-8" ?>
<configuration>

  ...

  <system.serviceModel>
    <client>
      <endpoint contract="usis.Samples.IWcfService" address="http://localhost/WcfService" binding="basicHttpBinding"/>
    </client>
  </system.serviceModel>

</configuration>

Using NetTcpBinding
-------------------

<?xml version="1.0" encoding="utf-8" ?>
<configuration>

  ...

  <system.serviceModel>
    <client>
      <endpoint contract="usis.Samples.IWcfService" address="net.tcp://localhost:8080/FieldService" binding="netTcpBinding"/>
    </client>
  </system.serviceModel>

</configuration>
