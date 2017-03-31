// In The Hand - .NET Components for Mobility
//
// InTheHand.Configuration.ConfigurationManager
// 
// Copyright (c) 2003-2014 In The Hand Ltd, All rights reserved.

using System.Collections.Specialized;
using System.IO;

namespace InTheHand.Configuration
{
    /// <summary>
    /// Provides access to configuration files for client applications.
    /// </summary>
    /// <remarks>Equivalent to System.Configuration.ConfigurationManager</remarks>
    /// <example>The following code example shows how to use the ConfigurationManager class to access the appSettings configuration section.
    /// <code lang="vbnet">
    /// Imports System
    /// Imports System.Collections.Specialized
    /// Imports System.Collections.ObjectModel
    /// Imports System.Collections
    /// Imports System.Text
    /// Imports System.Configuration
    /// 
    /// Class UsingConfigurationManager
    /// 
    ///     ' Show how to use AppSettings.
    ///     Shared Sub DisplayAppSettings() 
    ///     
    ///         ' Get the AppSettings collection.
    ///         Dim appSettings As NameValueCollection = ConfigurationManager.AppSettings
    ///         
    ///         Dim keys As String() = appSettings.AllKeys
    /// 
    ///         Debug.WriteLine()
    ///         Debug.WriteLine("Application appSettings:")
    /// 
    ///         ' Loop to get key/value pairs.
    ///         Dim i As Integer
    ///         For i = 0 To appSettings.Count
    /// 
    ///             Debug.WriteLine("#{0} Name: {1} Value: {2}", i, _
    ///                 keys(i), appSettings(i))
    ///         Next i
    ///         
    ///     End Sub 'DisplayAppSettings
    /// 
    ///     Shared Sub Main(ByVal args() As String) 
    /// 
    ///         ' Show how to use AppSettings.
    ///         DisplayAppSettings()
    /// 
    ///     End Sub 'Main 
    ///     
    /// End Class 'UsingConfigurationManager
    /// </code>
    /// <code lang="cs">
    /// using System;
    /// using System.Collections.Specialized;
    /// using System.Collections.ObjectModel;
    /// using System.Collections;
    /// using System.Text;
    /// using System.Configuration;
    /// 
    /// namespace InTheHand.Samples
    /// {
    ///     class UsingConfigurationManager
    ///     {
    ///         // Show how to use AppSettings.
    ///         static void DisplayAppSettings()
    ///         {
    ///             // Get the AppSettings collection.
    ///             NameValueCollection appSettings = ConfigurationManager.AppSettings;
    ///             
    ///             string[] keys = appSettings.AllKeys;
    ///             
    ///             Debug.WriteLine();
    ///             Debug.WriteLine("Application appSettings:");
    ///             
    ///             // Loop to get key/value pairs.
    ///             for (int i = 0; i &lt; appSettings.Count; i++)
    ///                 Debug.WriteLine("#{0} Name: {1} Value: {2}", i, keys[i], appSettings[i]);
    ///         }
    ///         
    ///         static void Main(string[] args)
    ///         {
    ///             // Show how to use AppSettings.
    ///             DisplayAppSettings();
    ///         }
    ///     }
    /// }
    /// </code></example>
    public static class ConfigurationManager
    {
        private static ReadOnlyNameValueCollection appSettings;

        /// <summary>
        /// Gets the <b>appSettings</b> section data for the current application's default configuration.
        /// </summary>
        /// <value>Returns a <see cref="NameValueCollection"/> object that contains the contents of the <b>appSettings</b> section for the current application's configuration.</value>
        /// <example>The following code example shows how to use the AppSettings property.
        /// <code lang="vbnet">
        /// ' Show how to use AppSettings.
        /// Shared Sub DisplayAppSettings() 
        ///     
        ///     ' Get the AppSettings collection.
        ///     Dim appSettings As NameValueCollection = ConfigurationManager.AppSettings
        ///         
        ///     Dim keys As String() = appSettings.AllKeys
        /// 
        ///     Debug.WriteLine()
        ///     Debug.WriteLine("Application appSettings:")
        /// 
        ///     ' Loop to get key/value pairs.
        ///     Dim i As Integer
        ///     For i = 0 To appSettings.Count
        /// 
        ///         Debug.WriteLine("#{0} Name: {1} Value: {2}", i, keys(i), appSettings(i))
        ///     Next i
        ///         
        /// End Sub 'DisplayAppSettings
        /// </code>
        /// <code lang="cs">
        /// // Show how to use AppSettings.
        /// static void DisplayAppSettings()
        /// {
        ///     // Get the AppSettings collection.
        ///     NameValueCollection appSettings = ConfigurationManager.AppSettings;
        ///             
        ///     string[] keys = appSettings.AllKeys;
        ///             
        ///     Debug.WriteLine();
        ///     Debug.WriteLine("Application appSettings:");
        ///             
        ///     // Loop to get key/value pairs.
        ///     for (int i = 0; i &lt; appSettings.Count; i++)
        ///         Debug.WriteLine("#{0} Name: {1} Value: {2}", i, keys[i], appSettings[i]);
        /// }</code></example>
        public static NameValueCollection AppSettings
        {
            get
            {
                if (appSettings == null)
                {
                    lock (typeof(ConfigurationManager))
                    {
                        appSettings = new ReadOnlyNameValueCollection();

                        string filename = InTheHand.Reflection.AssemblyInTheHand.GetModuleFileName() + ".config";
                        if (!System.IO.File.Exists(filename))
                        {
                            filename = Path.Combine(EnvironmentInTheHand.CurrentDirectory, "app.config");
                        }
                        if (System.IO.File.Exists(filename))
                        {
                            Stream fs = File.OpenRead(filename);
                            System.Xml.XmlReader xr = System.Xml.XmlReader.Create(fs);

                            if (xr != null)
                            {
                                if (xr.ReadToDescendant("configuration"))
                                {
                                    if (xr.ReadToDescendant("appSettings"))
                                    {
                                        bool moreContent = xr.ReadToDescendant("add");
                                        while (moreContent)
                                        {

                                            string key = xr.GetAttribute("key");
                                            string value = xr.GetAttribute("value");
                                            if (key != null && value != null)
                                            {
                                                appSettings.Add(key, value);
                                            }
                                            moreContent = xr.ReadToNextSibling("add");
                                        }
                                    }
                                }
                                xr.Close();
                            }
                            if(fs!=null)
                            {
                                fs.Close();
                                fs = null;
                            }
                        }

                        appSettings.IsReadOnly = true;
                    }
                }

                return appSettings;
            }
        }
    }

    internal sealed class ReadOnlyNameValueCollection : NameValueCollection
    {
        internal new bool IsReadOnly
        {
            set{base.IsReadOnly = value;}
        }
    }
}
