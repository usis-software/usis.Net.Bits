using System;
using usis.Windows.Framework;

namespace usis
{
    public class TestSnapIn : SnapIn
    {
        protected override void OnConnected(EventArgs e)
        {
            WindowsApplication.Properties["usis.Framework.SettingsPath"] = "Software\\usis\\GenericApp\\1.0\\Settings";
            WindowsApplication.MainWindow = new Windows.FrameWindow();
            //WindowsApplication.MainWindow.Show();

            base.OnConnected(e);
        }
    }

    #region old

    //internal class TestApplication : Application
    //{
    //    protected override void OnStartup(StartupEventArgs e)
    //    {
    //        base.OnStartup(e);
    //    }

    //    #region configuration

    //    //internal static ApplicationConfiguration LoadSolutionConfiguration()
    //    //{
    //    //    string xml = BuildConfigurationXmlString();
    //    //    ConfigurationRoot configurationRoot = LoadConfigurationFromString(xml);
    //    //    if (configurationRoot != null)
    //    //    {
    //    //        return configurationRoot.Applications.First();
    //    //    }
    //    //    return null;
    //    //}

    //    //internal static string BuildConfigurationXmlString()
    //    //{
    //    //    string xml;
    //    //    using (var writer = new StringWriter(CultureInfo.InvariantCulture))
    //    //    {
    //    //        var settings = new XmlWriterSettings();
    //    //        settings.Indent = true;
    //    //        settings.NewLineOnAttributes = true;
    //    //        var xmlWriter = XmlWriter.Create(writer, settings);
    //    //        var serializer = new XmlSerializer(typeof(ConfigurationRoot));
    //    //        serializer.Serialize(xmlWriter, BuildConfiguration());
    //    //        xml = writer.ToString();
    //    //        Debug.Print(xml);
    //    //    }
    //    //    return xml;
    //    //}

    //    //internal static ConfigurationRoot LoadConfigurationFromString(string xml)
    //    //{
    //    //    using (var reader = new StringReader(xml))
    //    //    {
    //    //        var serializer = new XmlSerializer(typeof(ConfigurationRoot));
    //    //        return serializer.Deserialize(reader) as ConfigurationRoot;
    //    //    }
    //    //}

    //    //internal static ConfigurationRoot BuildConfiguration()
    //    //{
    //    //    var application = new ConfigurationRoot();
    //    //    var solution = new ApplicationConfiguration()
    //    //    {
    //    //        //SettingsPath = "Software\\usis\\GenericApp\\1.0\\Settings"
    //    //    };
    //    //    var snapIn = new SnapInConfiguration()
    //    //    {
    //    //        TypeName = typeof(TestSnapIn).AssemblyQualifiedName
    //    //    };

    //    //    solution.SnapIns.Add(snapIn);
    //    //    application.Applications.Add(solution);

    //    //    return application;
    //    //}

    //    #endregion
    //}

    #endregion
}
