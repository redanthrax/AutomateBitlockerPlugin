using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GPOTesting {
    public class SuperImpersonate : IDisposable {
        #region WinAPI
        [DllImport("advapi32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        static extern int OpenProcessToken(
            System.IntPtr ProcessHandle, // handle to process
            int DesiredAccess, // desired access to process
            ref IntPtr TokenHandle // handle to open access token
        );

        [DllImport("kernel32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        public const int TOKEN_DUPLICATE = 0x0002;
        public const int TOKEN_QUERY = 0x0008;
        public const int TOKEN_IMPERSONATE = 0x0004;

        static IntPtr DupeToken(IntPtr token, int Level) {
            IntPtr dupeTokenHandle = IntPtr.Zero;
            bool retVal = DuplicateToken(token, Level, ref dupeTokenHandle);
            return dupeTokenHandle;
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean CreateProcessAsUser
        (
            IntPtr hToken,
            String lpApplicationName,
            String lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            Boolean bInheritHandles,
            Int32 dwCreationFlags,
            IntPtr lpEnvironment,
            String lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation
        );

        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO {
            public Int32 cb;
            public String lpReserved;
            public String lpDesktop;
            public String lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 dwProcessId;
            public Int32 dwThreadId;
        }
        #endregion

        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        IntPtr _hToken = IntPtr.Zero;
        IntPtr _dupeTokenHandle = IntPtr.Zero;
        WindowsImpersonationContext _impersonatedUser;

        /// <summary>
        /// Impersonate the user based on the Process ID
        /// </summary>
        /// <param name="ProcessID">Process ID of the user, cannot be 0</param>
        public SuperImpersonate(int ProcessID) {
            if (ProcessID == 0) {
                throw new Exception("Process cannot be 0!");
            }

            //get the process
            Process proc = Process.GetProcessById(ProcessID);
            if (OpenProcessToken(proc.Handle, TOKEN_QUERY | TOKEN_IMPERSONATE | TOKEN_DUPLICATE, ref _hToken) != 0) {
                //Create identity from the process token
                WindowsIdentity newId = new WindowsIdentity(_hToken);
                //set impersonation level to impersonate = 2
                const int SecurityImpersonation = 2;
                _dupeTokenHandle = DupeToken(_hToken, SecurityImpersonation);
                if (IntPtr.Zero == _dupeTokenHandle) {
                    string s = String.Format("Dup failed {0}, privilege not held",
                    Marshal.GetLastWin32Error());
                    throw new Exception(s);
                }

                //Do impersonation
                _impersonatedUser = newId.Impersonate();
            }
            else {
                string s = String.Format("OpenProcess Failed {0}, privilege not held", Marshal.GetLastWin32Error());
                throw new Exception(s);
            }
        }

        /// <summary>
        /// Impersonate the user based on the upn username@domain
        /// </summary>
        /// <param name="UPN">The full UPN of the user.</param>
        public SuperImpersonate(string UPN) {
            //Create identity from the UPN
            WindowsIdentity newId = new WindowsIdentity(UPN);
            //Do impersonation
            _impersonatedUser = newId.Impersonate();
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Disposing flag</param>
        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                handle.Dispose();
                //free other objects
                CloseHandle(_hToken);
                if (_impersonatedUser != null) {
                    _impersonatedUser.Dispose();
                }
            }

            disposed = true;
        }
    }
}
