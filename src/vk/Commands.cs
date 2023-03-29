using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Vulkan
{
    public static partial class VulkanNative
    {
        private static IntPtr s_nativeLibHandle;

        static VulkanNative()
        {
            s_nativeLibHandle = LoadNativeLibrary();
            LoadFunctionPointers();
        }

        private static IntPtr LoadFunctionPointer(string functionName) 
        {
            return NativeLibrary.GetExport(s_nativeLibHandle, functionName);
        }
        private static IntPtr LoadNativeLibrary()
        {
            return NativeLibrary.Load(GetVulkanName());
        }

        private static string GetVulkanName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "vulkan-1.dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (RuntimeInformation.OSDescription.Contains("Unix"))
                {
                    // Android
                return "libvulkan.so";
            }
                else
            {
                // Desktop Linux
                return "libvulkan.so.1";
            }
            }
#if NET5_0
            else if (OperatingSystem.IsAndroid())
            {
                return "libvulkan.so";
            }
#endif
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "libvulkan.dylib";
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        private static Exception CreateMissingFunctionException()
        {
            return new InvalidOperationException("The function does not exist or could not be loaded.");
        }
    }
    
}
