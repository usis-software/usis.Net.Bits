Model with EF Code First
========================

1. Entity classes derived from usis.Framework.Entity.EntityBase class.
2. DBContext class derived from usis.Framework.Entity.DBContextBase class.
3. Add DbSet<TEntity> properties in DBContext.

Configure a WFC service - WcfServiceHostFactory
===============================================

  <system.serviceModel>
    <services>
      <service name="<service type>">
        <endpoint binding="basicHttpBinding" contract="<contract type (interface)>"/>
        <host>
          <baseAddresses>
            <add baseAddress="http://*/service-path"/>
          </baseAddresses>
        </host>
      </service>
    </services>
  </system.serviceModel>

