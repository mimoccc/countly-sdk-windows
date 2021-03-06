﻿/*
Copyright (c) 2012, 2013, 2014 Countly

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using CountlySDK.Helpers;
using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.IO.IsolatedStorage;
using System.Windows;

namespace CountlySDK.Entitites
{
    /// <summary>
    /// This class provides several static methods to retrieve information about the current device and operating environment.
    /// </summary>
    internal static class Device
    {
        private static string deviceId;
        /// <summary>
        /// Returns the unique device identificator
        /// </summary>
        public static string DeviceId
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(deviceId))
                    {
                        return OpenUDID.value;
                    }
                    else
                    {
                        return deviceId;
                    }

                }
                catch
                {
                    return "";
                }
            }
            set
            {
                deviceId = value;
            }
        }

        /// <summary>
        /// Returns the display name of the current operating system
        /// </summary>
        public static string OS
        {
            get
            {
                return "Windows Phone";
            }
        }

        /// <summary>
        /// Returns the current operating system version as a displayable string
        /// </summary>
        public static string OSVersion
        {
            get
            {
                return Environment.OSVersion.Version.ToString();
            }
        }

        /// <summary>
        /// Returns the current device manufacturer
        /// </summary>
        public static string Manufacturer
        {
            get
            {
                return DeviceStatus.DeviceManufacturer;
            }
        }
        

        /// <summary>
        /// Returns the current device model
        /// </summary>
        public static string DeviceName
        {
            get
            {
                return PhoneNameHelper.Resolve(DeviceStatus.DeviceManufacturer, DeviceStatus.DeviceName).FullCanonicalName;
            }
        }

        /// <summary>
        /// Returns application version from WMAppManifest.xml
        /// </summary>
        public static string AppVersion
        {
            get
            {
                try
                {
                    var xmlReaderSettings = new System.Xml.XmlReaderSettings
                    {
                        XmlResolver = new System.Xml.XmlXapResolver()
                    };

                    using (var xmlReader = System.Xml.XmlReader.Create("WMAppManifest.xml", xmlReaderSettings))
                    {
                        xmlReader.ReadToDescendant("App");

                        return xmlReader.GetAttribute("Version");
                    }
                }
                catch
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// Returns device resolution in <width_px>x<height_px> format
        /// </summary>
        public static string Resolution
        {
            get
            {
                if (Environment.OSVersion.Version.Major < 8.0)
                {
                    return "480x800";
                }

                object resolution;

                if (DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out resolution))
                {
                    Size screenResolution = (Size)resolution;

                    return (screenResolution.Width).ToString("F0") + "x" + (screenResolution.Height).ToString("F0");
                }
                else
                {    
                    int Scale = Application.Current.Host.Content.ScaleFactor;

                    switch (Scale)
                    {
                        case 100:
                            return "480x800";
                        case 150:
                            return "720x1280";
                        case 160:
                            return "768x1280";
                        default:
                            return "480x800";
                    }
                }
            }
        }

        /// <summary>
        /// Returns cellular mobile operator
        /// </summary>
        public static string Carrier
        {
            get
            {
                return DeviceNetworkInformation.CellularMobileOperator;
            }
        }

        /// <summary>
        /// Returns available RAM space
        /// </summary>
        public static long RamCurrent
        {
            get
            {
                return DeviceStatus.ApplicationCurrentMemoryUsage;
            }
        }

        /// <summary>
        /// Returns total RAM size
        /// </summary>
        public static long RamTotal
        {
            get
            {
                return DeviceStatus.DeviceTotalMemory;
            }
        }

        /// <summary>
        /// Returns current battery level from 0 to 100
        /// </summary>
        public static int Bat
        {
            get
            {
                return Windows.Phone.Devices.Power.Battery.GetDefault().RemainingChargePercent;
            }
        }

        /// <summary>
        /// Returns current device orientation
        /// </summary>
        public static string Orientation
        {
            get
            {
                return (Application.Current.Host.Content.ActualWidth > Application.Current.Host.Content.ActualHeight) ? "landscape" : "portrait";
            }
        }

        /// <summary>
        /// Returns current device connection to the internet
        /// </summary>
        public static bool Online
        {
            get
            {
                NetworkInterfaceType networkInterfaceType = NetworkInterface.NetworkInterfaceType;

                return (networkInterfaceType == NetworkInterfaceType.Wireless80211 || networkInterfaceType == NetworkInterfaceType.MobileBroadbandGsm || networkInterfaceType == NetworkInterfaceType.MobileBroadbandCdma);
            }
        }
    }
}
