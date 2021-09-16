using Microsoft.Win32;
using System.Management;
using iTin.Core.Hardware.Common;
using iTin.Hardware.Specification;
using iTin.Hardware.Specification.Dmi;
using iTin.Hardware.Specification.Dmi.Property;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System;


namespace HWID_Checker
{
    class Programm
    {
        static void Main(string[] args)
        {
            int width = 90;
            int height = 30;
            Console.SetWindowSize(width, height);
            Console.WriteLine("                                         HWID Checker\n\n");

            getWindows();
            getCPU();
            getMOBO();
            getDrives();

            Console.ReadKey();
        }

        public static void getWindows()
        {
            Console.WriteLine("######################################### Windows ########################################");
            try
            {
                string reqURL = "SOFTWARE\\Microsoft\\SQMClient";
                RegistryKey key = Registry.LocalMachine.OpenSubKey(reqURL);
                if (key != null)
                {
                    Object a = key.GetValue("MachineId");
                    if (a != null)
                    {
                        string MachineId = new string(a as String);
                        Console.WriteLine("#    Machine ID:                       " + MachineId + "            #");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {
                string reqURL = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion";
                RegistryKey key = Registry.LocalMachine.OpenSubKey(reqURL);
                if (key != null)
                {
                    Object b = key.GetValue("ProductId");
                    if (b != null)
                    {
                        string ProductId = new string(b as String);
                        Console.WriteLine("#    Product ID:                       " + ProductId + "                           #");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {
                string reqURL = "SYSTEM\\CurrentControlSet\\Control\\IDConfigDB\\Hardware Profiles\\0001";
                RegistryKey key = Registry.LocalMachine.OpenSubKey(reqURL);
                if (key != null)
                {
                    Object h = key.GetValue("HwProfileGuid");
                    if (h != null)
                    {
                        string HwProfileGuid = new string(h as String);
                        Console.WriteLine("#    HwProfile GUID:                   " + HwProfileGuid + "            #");
                    } 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {
                string reqURL = "SOFTWARE\\Microsoft\\Cryptography";
                RegistryKey key = Registry.LocalMachine.OpenSubKey(reqURL);
                if (key != null)
                {
                    Object i = key.GetValue("MachineGuid");
                    if (i != null)
                    {
                        string MachineGuid = new string(i as String);
                        Console.WriteLine("#    Machine GUID:                     " + MachineGuid + "              #");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



            Console.WriteLine("##########################################################################################");
        }

        public static void getCPU()
        {
            Console.WriteLine("\n########################################### CPU ##########################################");
            try
            {
                ManagementClass managClass = new ManagementClass("win32_processor");
                ManagementObjectCollection managCollec = managClass.GetInstances();

                foreach (ManagementObject managObj in managCollec)
                {
                    var Cpu = managObj.Properties["Name"].Value.ToString();
                    Console.WriteLine("#    CPU Name:                         " + Cpu + "         #");

                    var CpuInfo = managObj.Properties["processorID"].Value.ToString();
                    Console.WriteLine("#    CPU ID:                           " + CpuInfo + "                                  #");
                    break;
                }
                Console.WriteLine("##########################################################################################");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void getMOBO()
        {
            Console.WriteLine("\n####################################### MOTHERBOARD ######################################");
            try
            {
                DmiStructureCollection structures_2 = DMI.CreateInstance().Structures;
                QueryPropertyResult Product = structures_2.GetProperty(DmiProperty.BaseBoard.Product);
                if (Product.Success)
                {
                    Console.WriteLine("#    Motherboard Model:                " + Product.Value.Value + "                              #");
                }

                DmiStructureCollection structures = DMI.CreateInstance().Structures;
                QueryPropertyResult UUID = structures.GetProperty(DmiProperty.System.UUID);
                if (UUID.Success)
                {
                    Console.WriteLine("#    Motherboard UUID:                 " + UUID.Value.Value + "            #");
                }

                DmiStructureCollection structures_1 = DMI.CreateInstance().Structures;
                QueryPropertyResult Serial = structures_1.GetProperty(DmiProperty.BaseBoard.SerialNumber);
                if (Serial.Success)
                {
                    Console.WriteLine("#    Motherboard Serialnumber:         " + Serial.Value.Value + "                                   #");
                }

                try
                {
                    string macAddresses = string.Empty;
                    foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (nic.OperationalStatus == OperationalStatus.Up)
                        {
                            macAddresses += nic.GetPhysicalAddress().ToString();
                            break;
                        }
                    }
                    Console.WriteLine("#    MAC Adress:                       " + macAddresses + "                                      #");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Console.WriteLine("##########################################################################################");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static List<string> list = new List<string>();
        public static List<string> list_1 = new List<string>();

        public static void getDrives()
        {
            Console.WriteLine("\n######################################### DRIVES #########################################");
            try
            {
                ManagementObjectSearcher moSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

                foreach (ManagementObject wmi_HD in moSearcher.Get())
                {
                    string hd = wmi_HD["SerialNumber"].ToString();
                    string model = wmi_HD["model"].ToString();
                    list.Add(hd);
                    list_1.Add(model);
                }
                Console.WriteLine("#    " + list_1[0] + ":                    " + list[0] + "                                      #");
                Console.WriteLine("#    " + list_1[1] + ":           " + list[1] + "                                      #");
                Console.WriteLine("#    " + list_1[2] + ":          " + list[2] + "                              #");
                Console.WriteLine("#    " + list_1[3] + ":          " + list[3] + "                              #");
                Console.WriteLine("##########################################################################################");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}