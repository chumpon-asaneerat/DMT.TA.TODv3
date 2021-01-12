#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

#endregion

namespace DMT.Smartcard
{
    #region Win32Interop and related enum/structures/class

    public enum RAWINPUTDEVICETYPE
    {
        RIM_TYPEMOUSE = 0,
        RIM_TYPEKEYBOARD = 1,
        RIM_TYPEHID = 2
    }

    public struct RAWINPUTDEVICELIST
    {
        public IntPtr hDevice;
        public RAWINPUTDEVICETYPE dwType;
    }

    public struct RID_DEVICE_INFO_MOUSE
    {
        public uint dwId;
        public uint dwNumberOfButtons;
        public uint dwSampleRate;
        public bool fHasHorizontalWheel;
    }

    public struct RID_DEVICE_INFO_KEYBOARD
    {
        public uint dwType;
        public uint dwSubType;
        public uint dwKeyboardMode;
        public uint dwNumberOfFunctionKeys;
        public uint dwNumberOfIndicators;
        public uint dwNumberOfKeysTotal;
    }

    public struct RID_DEVICE_INFO_HID
    {
        public uint dwVendorId;
        public uint dwProductId;
        public uint dwVersionNumber;
        public ushort usUsagePage;
        public ushort usUsage;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct RID_DEVICE_INFO
    {
        [FieldOffset(0)]
        public uint cbSize;
        [FieldOffset(4)]
        public RAWINPUTDEVICETYPE dwType;
        [FieldOffset(8)]
        public RID_DEVICE_INFO_MOUSE mouse;
        [FieldOffset(8)]
        public RID_DEVICE_INFO_KEYBOARD keyboard;
        [FieldOffset(8)]
        public RID_DEVICE_INFO_HID hid;
    }

    public class Win32Exception : Exception
    {
        public int ErrorCode { get; private set; }
        public Win32Exception(string message, int code) : base(message)
        {
            ErrorCode = code;
        }
    }

    internal class Win32Interop
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr LoadLibraryExW(string path, IntPtr fileHandle, int dwFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern int FormatMessageW(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, IntPtr lpBuffer, int nSize, IntPtr args);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr mem);

        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern uint GetRawInputDeviceList(IntPtr pRawInputDeviceList,
                IntPtr puiNumDevices,
                uint cbSize);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint GetRawInputDeviceInfoW(IntPtr hDevice,
                uint uiCommand,
                IntPtr pData,
                IntPtr pcbSize);

        public static void GetLastErrorAndThrow()
        {
            var err = Marshal.GetLastWin32Error();
            if (err != 0)
            {
                var message = FormatErrorMessage(err);
                throw new Win32Exception(message, err);
            }
        }

        public static string FormatErrorMessage(int code)
        {
            IntPtr dataPtr = Marshal.AllocHGlobal(IntPtr.Size);

            try
            {
                var len = FormatMessageW(0x00000100 | 0x00001000 | 0x00000200, IntPtr.Zero, code, (0x01 << 10) | (0x00), dataPtr, 0, IntPtr.Zero);
                // NET 4.5.1
                //var textPtr = Marshal.PtrToStructure<IntPtr>(dataPtr);                
                // NET 4.5
                var textPtr = (IntPtr)Marshal.PtrToStructure(dataPtr, typeof(IntPtr));

                if (len != 0 && textPtr != IntPtr.Zero)
                {
                    var retval = Marshal.PtrToStringUni(textPtr, len);

                    LocalFree(textPtr);

                    return retval;
                }

                return "Unknown message";
            }
            finally
            {
                Marshal.Release(dataPtr);
            }
        }
    }

    #endregion
}

namespace DMT.Smartcard
{
    #region ThreadSafeQueue<T>

    internal class ThreadSafeQueue<T> : IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
    {
        private Queue<T> _queue;

        public ThreadSafeQueue()
        {
            _queue = new Queue<T>();
        }

        public ThreadSafeQueue(int capacity)
        {
            _queue = new Queue<T>(capacity);
        }

        public ThreadSafeQueue(IEnumerable<T> collection)
        {
            _queue = new Queue<T>(collection);
        }

        public int Count
        {
            get
            {
                lock (_queue)
                    return _queue.Count;
            }
        }

        public bool IsSynchronized => ((ICollection)_queue).IsSynchronized;

        public object SyncRoot => ((ICollection)_queue).SyncRoot;

        public void Clear()
        {
            lock (_queue)
                _queue.Clear();
        }

        public bool Contains(T item)
        {
            lock (_queue)
                return _queue.Contains(item);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)_queue).CopyTo(array, index);
        }

        public T Dequeue()
        {
            lock (_queue)
                return _queue.Dequeue();
        }

        public void Enqueue(T item)
        {
            lock (_queue)
            {
                _queue.Enqueue(item);
                System.Threading.Monitor.Pulse(_queue);
            }
        }

        public void WaitForEnqueue()
        {
            lock (_queue)
            {
                if (Count == 0)
                    System.Threading.Monitor.Wait(_queue);
            }
        }

        public void WaitForEnqueue(int timeout)
        {
            lock (_queue)
            {
                System.Threading.Monitor.Wait(_queue, timeout);
            }
        }

        public T[] FlushToArray()
        {
            lock (_queue)
            {
                var res = _queue.ToArray();
                _queue.Clear();
                return res;
            }
        }

        public void Notify()
        {
            lock (_queue)
            {
                System.Threading.Monitor.Pulse(_queue);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_queue).GetEnumerator();
        }

        public T Peek()
        {
            lock (_queue)
                return _queue.Peek();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)_queue).GetEnumerator();
        }
    }

    #endregion

    #region DispathcedThread

    internal class DispathcedThread : IDisposable
    {
        private ThreadSafeQueue<Task> _tasksQueue;

        public Thread Thread { get; private set; }

        private CancellationTokenSource _tokenSource;

        public DispathcedThread()
        {
            _tokenSource = new CancellationTokenSource();
            _tasksQueue = new ThreadSafeQueue<Task>();
            Thread = new Thread(ProcThread);
            Thread.Start(_tokenSource.Token);
        }

        private void ProcThread(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            while (!token.IsCancellationRequested)
            {
                while (_tasksQueue.Count > 0)
                {
                    var task = _tasksQueue.Dequeue();

                    try
                    {
                        task.RunSynchronously();
                    }
                    catch (Exception)
                    {
                    }
                }

                _tasksQueue.WaitForEnqueue(1000);
            }
        }

        public void Invoke(Action action)
        {
            Invoke(new Task(action));
        }

        public void Invoke(Task t)
        {
            try
            {
                _tasksQueue.Enqueue(t);
                t.Wait();
                if (t.IsFaulted)
                    throw t.Exception;
            }
            catch (AggregateException ex)
            {
                throw ex.GetBaseException();
            }
        }

        public Task InvokeAsync(Action action)
        {
            return InvokeAsync(new Task(action));
        }

        public Task InvokeAsync(Task t)
        {
            _tasksQueue.Enqueue(t);
            return t;
        }

        public T Invoke<T>(Func<T> action)
        {
            return Invoke(new Task<T>(action));
        }

        public T Invoke<T>(Task<T> t)
        {
            try
            {
                _tasksQueue.Enqueue(t);
                t.Wait();
                if (t.IsFaulted)
                    throw t.Exception;
                return t.Result;
            }
            catch (AggregateException ex)
            {
                throw ex.GetBaseException();
            }
        }

        public Task<T> InvokeAsync<T>(Func<T> action)
        {
            return InvokeAsync(new Task<T>(action));
        }

        public Task<T> InvokeAsync<T>(Task<T> t)
        {
            _tasksQueue.Enqueue(t);
            return t;
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _tokenSource.Cancel();

                    Thread.Join(10000);
                    try
                    {
                        Thread.Abort();
                    }
                    catch (ThreadAbortException)
                    {
                        Thread.ResetAbort();
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this);
        }

        #endregion

    }

    #endregion

    #region UnmanagedFunctionNameAttribute

    internal class UnmanagedFunctionNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public UnmanagedFunctionNameAttribute(string name)
        {
            Name = name;
        }
    }

    #endregion

    #region SDKDelegates

    unsafe internal class SDKDelegates
    {
        private const CallingConvention DelegatesCallingConversion = CallingConvention.StdCall;

        [UnmanagedFunctionName("lib_ver")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int LibVer(/*uint*/IntPtr Ver);

        [UnmanagedFunctionName("des_encrypt")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int des_encrypt(/*byte*/IntPtr szOut, /*byte*/IntPtr szIn, uint inlen, /*byte*/IntPtr key, uint keylen);

        [UnmanagedFunctionName("des_decrypt")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int des_decrypt(/*byte*/IntPtr szOut, /*byte*/IntPtr szIn, uint inlen, /*byte*/IntPtr key, uint keylen);

        [UnmanagedFunctionName("rf_init_usb")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate IntPtr rf_init_usb(int HIDNum);

        [UnmanagedFunctionName("rf_get_device_name")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_get_device_name(int HIDNum, /*char*/IntPtr buf, int sz);

        [UnmanagedFunctionName("rf_init_device_number")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_init_device_number(ushort icdev);

        [UnmanagedFunctionName("rf_get_device_number")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_get_device_number(/*int*/IntPtr Icdev);

        [UnmanagedFunctionName("rf_get_model")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_get_model(ushort icdev, /*byte*/IntPtr pVersion, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("rf_get_snr")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_get_snr(ushort icdev, /*byte*/IntPtr Snr);

        [UnmanagedFunctionName("rf_beep")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_beep(ushort icdev, byte msec);

        [UnmanagedFunctionName("rf_init_sam")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_init_sam(ushort icdev, byte bound);

        [UnmanagedFunctionName("rf_sam_rst")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_sam_rst(ushort icdev, /*byte*/IntPtr pData, /*byte*/IntPtr pMsgLg);

        [UnmanagedFunctionName("rf_sam_cos")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_sam_cos(ushort icdev, /*byte*/IntPtr command, byte cmdLen, /*byte*/IntPtr pData, /*byte*/IntPtr Length);

        [UnmanagedFunctionName("rf_init_type")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_init_type(ushort icdev, byte type);

        [UnmanagedFunctionName("rf_antenna_sta")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_antenna_sta(ushort icdev, byte mode);

        [UnmanagedFunctionName("rf_request")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_request(ushort icdev, byte mode, /*ushort*/IntPtr TagType);
        //public delegate int rf_request(ushort icdev, byte mode, ushort TagType);

        [UnmanagedFunctionName("rf_anticoll")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_anticoll(ushort icdev, byte bcnt, /*byte*/IntPtr pSnr, /*byte*/IntPtr pRLength);

        [UnmanagedFunctionName("rf_select")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_select(ushort icdev, /*byte*/IntPtr pSnr, byte srcLen, /*byte*/IntPtr Size);

        [UnmanagedFunctionName("rf_halt")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_halt();

        [UnmanagedFunctionName("rf_download_key")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_download_key(ushort icdev, byte mode, /*byte*/IntPtr key);

        [UnmanagedFunctionName("rf_M1_authentication1")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_M1_authentication1(ushort icdev, byte mode, byte secnr);

        [UnmanagedFunctionName("rf_M1_authentication2")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_M1_authentication2(ushort icdev, byte mode, byte secnr, /*byte*/IntPtr key);

        [UnmanagedFunctionName("rf_M1_read")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_M1_read(ushort icdev, byte adr, /*byte*/IntPtr pData, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("rf_M1_write")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_M1_write(ushort icdev, byte adr, /*byte*/IntPtr data);

        [UnmanagedFunctionName("rf_M1_initval")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_M1_initval(ushort icdev, byte adr, long value);

        [UnmanagedFunctionName("rf_M1_readval")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_M1_readval(ushort icdev, byte adr, /*long*/IntPtr pValue);

        [UnmanagedFunctionName("rf_M1_decrement")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_M1_decrement(ushort icdev, byte adr, long value);

        [UnmanagedFunctionName("rf_M1_increment")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_M1_increment(ushort icdev, byte adr, long value);

        [UnmanagedFunctionName("rf_M1_restore")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_M1_restore(ushort icdev, byte adr);

        [UnmanagedFunctionName("rf_M1_transfer")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_M1_transfer(ushort icdev, byte adr);

        [UnmanagedFunctionName("rf_typea_rst")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_typea_rst(ushort icdev, byte model, /*byte*/IntPtr pData, /*byte*/IntPtr pMsgLg);

        [UnmanagedFunctionName("rf_cos_command")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_cos_command(ushort icdev, /*byte*/IntPtr command, byte cmdLen, /*byte*/IntPtr pData, /*byte*/IntPtr pMsgLg);

        [UnmanagedFunctionName("rf_atqb")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_atqb(ushort icdev, byte model, /*byte*/IntPtr pData, /*byte*/IntPtr pMsgLg);

        [UnmanagedFunctionName("rf_attrib")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_attrib(ushort icdev, ulong PUPI, byte CID);

        [UnmanagedFunctionName("rf_typeb_cos")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_typeb_cos(ushort icdev, byte CID, /*byte*/IntPtr command, byte cmdLen, /*byte*/IntPtr pData, /*byte*/IntPtr pMsgLg);

        [UnmanagedFunctionName("rf_hltb")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_hltb(ushort icdev, ulong PUPI);

        [UnmanagedFunctionName("rf_at020_check")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_at020_check(ushort icdev, /*byte*/IntPtr key);

        [UnmanagedFunctionName("rf_at020_read")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_at020_read(ushort icdev, byte adr, /*byte*/IntPtr pData, /*byte*/IntPtr pMsgLen);

        [UnmanagedFunctionName("rf_at020_write")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_at020_write(ushort icdev, byte adr, /*byte*/IntPtr data);

        [UnmanagedFunctionName("rf_at020_lock")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_at020_lock(ushort icdev, /*byte*/IntPtr data);

        [UnmanagedFunctionName("rf_at020_count")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_at020_count(ushort icdev, /*byte*/IntPtr data);

        [UnmanagedFunctionName("rf_at020_deselect")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_at020_deselect(ushort icdev);

        [UnmanagedFunctionName("rf_light")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_light(ushort icdev, byte color);

        [UnmanagedFunctionName("rf_cl_deselect")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_cl_deselect(ushort icdev);

        [UnmanagedFunctionName("rf_free")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate void rf_free(/*void*/IntPtr pData);

        [UnmanagedFunctionName("rf_ClosePort")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_ClosePort(IntPtr m_hFileHandle);

        [UnmanagedFunctionName("rf_GetErrorMessage")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_GetErrorMessage();

        [UnmanagedFunctionName("rf_st_select")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_st_select(ushort icdev, /*byte*/IntPtr pChip_ID);

        [UnmanagedFunctionName("rf_st_completion")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_st_completion(ushort icdev);

        [UnmanagedFunctionName("rf_sr176_readblock")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_sr176_readblock(ushort icdev, byte block, /*byte*/IntPtr pData, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("rf_sr176_writeblock")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_sr176_writeblock(ushort icdev, byte block, /*byte*/IntPtr data);

        [UnmanagedFunctionName("rf_sr176_protectblock")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_sr176_protectblock(ushort icdev, byte lockreg);

        [UnmanagedFunctionName("rf_srix4k_readblock")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_srix4k_readblock(ushort icdev, byte block, /*byte*/IntPtr pData, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("rf_srix4k_writeblock")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_srix4k_writeblock(ushort icdev, byte block, /*byte*/IntPtr pData);

        [UnmanagedFunctionName("rf_srix4k_authenticate")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_srix4k_authenticate(ushort icdev, /*byte*/IntPtr pRND, /*byte*/IntPtr pSIG, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("rf_srix4k_getuid")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_srix4k_getuid(ushort icdev, /*byte*/IntPtr Puid, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("rf_srix4k_writelockreg")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_srix4k_writelockreg(ushort icdev, byte lockreg);

        [UnmanagedFunctionName("rf_ul_select")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_ul_select(ushort icdev, /*byte*/IntPtr pSnr, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("rf_ul_write")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_ul_write(ushort icdev, byte page, /*byte*/IntPtr data);

        [UnmanagedFunctionName("ISO15693_Inventory")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Inventory(ushort icdev, /*byte*/IntPtr pData, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("ISO15693_Stay_Quiet")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Stay_Quiet(ushort icdev, /*byte*/IntPtr UID);

        [UnmanagedFunctionName("ISO15693_Select")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Select(ushort icdev, /*byte*/IntPtr UID);

        [UnmanagedFunctionName("ISO15693_Reset_To_Ready")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Reset_To_Ready(ushort icdev, byte model, /*byte*/IntPtr UID);

        [UnmanagedFunctionName("ISO15693_Read")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Read(ushort icdev, byte model, /*byte*/IntPtr UID,
            byte block, byte number, /*byte*/IntPtr Pdata, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("ISO15693_Write")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Write(ushort icdev, byte model, /*byte*/IntPtr UID,
            byte block, /*byte*/IntPtr data);

        [UnmanagedFunctionName("ISO15693_Lock_Block")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Lock_Block(ushort icdev, byte model, /*byte*/IntPtr UID, byte block);

        [UnmanagedFunctionName("ISO15693_Write_AFI")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Write_AFI(ushort icdev, byte model, /*byte*/IntPtr UID, byte AFI);

        [UnmanagedFunctionName("ISO15693_Lock_AFI")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Lock_AFI(ushort icdev, byte model, /*byte*/IntPtr UID);

        [UnmanagedFunctionName("ISO15693_Write_DSFID")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Write_DSFID(ushort icdev, byte model, /*byte*/IntPtr UID, byte DSFID);

        [UnmanagedFunctionName("ISO15693_Lock_DSFID")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Lock_DSFID(ushort icdev, byte model, /*byte*/IntPtr UID);

        [UnmanagedFunctionName("ISO15693_Get_System_Information")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Get_System_Information(ushort icdev, byte model,
            /*byte*/IntPtr UID, /*byte*/IntPtr Pdata, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("ISO15693_Get_Block_Security")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Get_Block_Security(ushort icdev, byte model,
            /*byte*/IntPtr UID, byte block, byte number, /*byte*/IntPtr Pdata,
            /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("smartcard_rst")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int smartcard_rst(ushort icdev, /*byte*/IntPtr Data, /*byte*/IntPtr MsgLg);

        [UnmanagedFunctionName("smartcard_cos")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int smartcard_cos(ushort icdev, /*byte*/IntPtr command, byte cmdLen, /*byte*/IntPtr Data, /*byte*/IntPtr MsgLg);

        [UnmanagedFunctionName("AT24_read")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int AT24_read(ushort icdev, ushort offset, ushort len, /*byte*/IntPtr data_buffer);

        [UnmanagedFunctionName("AT24_write")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int AT24_write(ushort icdev, uint offset, ushort len, /*byte*/IntPtr data_buffer);

        [UnmanagedFunctionName("AT1608_Rst")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int AT1608_Rst(ushort icdev, /*byte*/IntPtr pData);

        [UnmanagedFunctionName("AT1608_Authenticate ")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int AT1608_Authenticate(ushort icdev, /*byte*/IntPtr pQ0, /*byte*/IntPtr pQ1);

        [UnmanagedFunctionName("AT1608_VerifyPassint")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int AT1608_VerifyPassint(ushort icdev, byte index, /*byte*/IntPtr pPassint);

        [UnmanagedFunctionName("AT1608_Read")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int AT1608_Read(ushort icdev, byte addr, ushort len, byte zone, /*byte*/IntPtr pData);

        [UnmanagedFunctionName("AT1608_Write")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int AT1608_Write(ushort icdev, byte addr, byte zone, /*byte*/IntPtr pData, ushort len);

        [UnmanagedFunctionName("AT1608_ReadFuse1 ")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int AT1608_ReadFuse1(ushort icdev, /*byte*/IntPtr pFuse);

        [UnmanagedFunctionName("AT1608_WriteFuse1")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int AT1608_WriteFuse1(ushort icdev);

        [UnmanagedFunctionName("OpenCard ")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int OpenCard(ushort icdev);

        [UnmanagedFunctionName("CloseCard ")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int CloseCard(ushort icdev);

        [UnmanagedFunctionName("rf_Shc1102_Auth")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_Shc1102_Auth(ushort icdev, /*byte*/IntPtr pPassint);

        [UnmanagedFunctionName("rf_Shc1102_Read) (ushort icdev, byte block, /*byte*/IntPtr pData, /*byte*/IntPtr pLen")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_Shc1102_Read(ushort icdev, byte block, /*byte*/IntPtr pData, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("rf_Shc1102_Write) (ushort icdev, byte block, /*byte*/IntPtr pData")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_Shc1102_Write(ushort icdev, byte block, /*byte*/IntPtr pData);

        [UnmanagedFunctionName("ReadTime")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ReadTime(ushort icdev, /*byte*/IntPtr pData, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("WriteTime")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int WriteTime(ushort icdev, /*byte*/IntPtr pData);

        [UnmanagedFunctionName("DisPlayTime")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int DisPlayTime(ushort icdev, byte bShowFlag);

        [UnmanagedFunctionName("DisPlayString")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int DisPlayString(ushort icdev, /*byte*/IntPtr pData, byte Len);

        [UnmanagedFunctionName("ISO15693_Inventorys")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int ISO15693_Inventorys(ushort icdev, /*byte*/IntPtr pData, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("Srf55vp_Read")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int Srf55vp_Read(ushort icdev, /*byte*/IntPtr pUID, byte page, /*byte*/IntPtr pData, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("Srf55vp_Write")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int Srf55vp_Write(ushort icdev, /*byte*/IntPtr pUID, byte page, /*byte*/IntPtr pData);

        [UnmanagedFunctionName("Srf55vp_WriteByte")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int Srf55vp_WriteByte(ushort icdev, /*byte*/IntPtr pUID, byte page, byte byteaddr, byte data);

        [UnmanagedFunctionName("Srf55vp_Write_Reread")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int Srf55vp_Write_Reread(ushort icdev, /*byte*/IntPtr pUID, byte page, /*byte*/IntPtr pWdata, /*byte*/IntPtr pRdata, /*byte*/IntPtr pLen);

        [UnmanagedFunctionName("rf_DESFire_rst")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_DESFire_rst(ushort icdev, byte model,
            /*byte*/IntPtr pData, /*byte*/IntPtr pMsgLg);

        [UnmanagedFunctionName("rf_transceive")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_transceive(ushort icdev, byte crcon,
            /*byte*/IntPtr pTxData, byte sendLen,
            /*byte*/IntPtr pRxData, /*byte*/IntPtr pMsgLg);


        [UnmanagedFunctionName("rf_transceive1")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_transceive1(ushort icdev, /*byte*/IntPtr pTxData,
            byte sendLen, /*byte*/IntPtr pRxData, /*byte*/IntPtr pMsgLg);

        [UnmanagedFunctionName("rf_receive")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_receive(ushort icdev, /*byte*/IntPtr pRxData, /*byte*/IntPtr pMsgLg);

        [UnmanagedFunctionName("rf_thr1064_read")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_thr1064_read(ushort icdev, byte page,
            /*byte*/IntPtr pData, /*byte*/IntPtr pMsgLen);

        [UnmanagedFunctionName("rf_thr1064_write")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_thr1064_write(ushort icdev, byte page, /*byte*/IntPtr pData, byte MsgLen);

        [UnmanagedFunctionName("rf_thr1064_check")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_thr1064_check(ushort icdev, /*byte*/IntPtr pKey);

        [UnmanagedFunctionName("SRF55V_Inventorys")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int SRF55V_Inventorys(ushort icdev, byte AFI, /*byte*/IntPtr pData, /*byte*/IntPtr pLen);

        /*******************************************************/
        //Parameter: icdev:   [IN]        Communication device identifier
        //           model:   [IN]        it0 = Select_flag, bit1 = Addres_flag, bit2 = Option_flag
        //           pUID:    [IN]  8 bytes UID
        //           block:   [IN]  Block address
        //           pData:   [OUT] Response data from tag
        //           pLen:    [OUT] Length of response data
        /*******************************************************/
        /*
        int (WINAPI IntPtr SRI64_Read)(ushort icdev, 
                                            byte  model,
                                            byte  *PUID,
                                            byte  block,
                                            byte  *pData,
                                            byte  *pLen);
        */
        /*******************************************************/
        //Parameter: icdev:  [IN] Communication device identifier
        //           model:  [IN] bit0 = Select_flag, bit1 = Addres_flag, bit2 = Option_flag
        //           pUID:   [IN] 8 bytes UID
        //           block:  [IN] Block address
        //           pData:  [IN] Written data, 1 byte
        /*******************************************************/
        [UnmanagedFunctionName("SRI64_Write")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int SRI64_Write(ushort icdev, byte model,
            /*byte*/IntPtr pUID, byte block, byte wData);

        /*******************************************************/
        //icdev: [IN]        Communication device identifier
        //pKey:  [IN]       Key, 16 bytes
        //???룺	 0x0240
        /*******************************************************/
        [UnmanagedFunctionName("rf_UC_authentication")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_UC_authentication(ushort icdev, /*byte*/IntPtr pKey);

        /*******************************************************/
        //Parameter: icdev:  [IN]        Communication device identifier
        //           pKey:   [IN]        Key, 16 bytes
        //???룺	 0x0242
        /*******************************************************/
        [UnmanagedFunctionName("rf_UC_changekey")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_UC_changekey(ushort icdev, /*byte*/IntPtr pKey);

        /*******************************************************/
        //Parameter: icdev:  [IN]        Communication device identifier
        //           pData:  [IN]        written data
        //???룺	 0x010D
        /*******************************************************/
        [UnmanagedFunctionName("rf_NFC_switch")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_NFC_switch(ushort icdev, /*byte*/IntPtr pData);

        /*******************************************************/
        //Parameter: icdev:  [IN]        Communication device identifier
        //           pData:  [IN]        written data
        //           length: [IN]
        //???룺	 0x010E
        /*******************************************************/
        [UnmanagedFunctionName("rf_NDEF")]
        [UnmanagedFunctionPointer(DelegatesCallingConversion)]
        public delegate int rf_NDEF(ushort icdev, /*byte*/IntPtr pData, ushort length);
    }

    #endregion

    #region UnmanagedFunctionInfo (struct)

    internal struct UnmanagedFunctionInfo
    {
        public IntPtr Pointer;
        public Delegate FunctionDelegate;
    }

    #endregion

    #region FunctionPointersContainer

    internal class FunctionPointersContainer
    {
        public Hashtable FunctionPointers { get; private set; }
        public IntPtr ModulePtr { get; private set; }

        public FunctionPointersContainer(IntPtr hModule)
        {
            FunctionPointers = new Hashtable();
            ModulePtr = hModule;
        }

        public UnmanagedFunctionInfo GetFunctionAddress<T>(string name) where T : Delegate
        {
            if (FunctionPointers.ContainsKey(name))
                return (UnmanagedFunctionInfo)FunctionPointers[name];

            var res = Win32Interop.GetProcAddress(ModulePtr, name);

            if (res == IntPtr.Zero)
            {
                Win32Interop.GetLastErrorAndThrow();
            }

            var info = new UnmanagedFunctionInfo
            {
                // .NET 4.5.1
                //FunctionDelegate = Marshal.GetDelegateForFunctionPointer<T>(res),
                // .NET 4.5
                FunctionDelegate = Marshal.GetDelegateForFunctionPointer(res, typeof(T)),
                Pointer = res
            };

            FunctionPointers.Add(name, info);

            return info;
        }

        public UnmanagedFunctionInfo GetFunctionAddress<T>() where T : Delegate
        {
            var type = typeof(T);
            var attr = type.GetCustomAttributes(typeof(UnmanagedFunctionNameAttribute), true);
            if (attr.Length == 0)
                throw new InvalidOperationException("UnmanagedFunctionNameAttribute is not defined");
            var name = ((UnmanagedFunctionNameAttribute)attr[0]).Name;

            return GetFunctionAddress<T>(name);
        }

        public T GetFunctionDelegate<T>() where T : Delegate
        {
            var info = GetFunctionAddress<T>();
            return (T)info.FunctionDelegate;
        }
    }

    #endregion

    #region SL600SDK

    #region SL600Exception

    public class SL600Exception : Exception
    {
        public SL600Exception() : base() { }
        public SL600Exception(string msg) : base(msg) { }
    }

    #endregion

    #region SL600SDK - Main

    public partial class SL600SDK : IDisposable
    {
        internal const int MAX_RF_BUFFER = 1024;
        internal const int MAX_RF_STR_SIZE = 255;

        public static readonly byte[] DefaultKey = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

        internal DispathcedThread _thread;
        internal FunctionPointersContainer _functions;
        internal SL600SDK(DispathcedThread thread, FunctionPointersContainer functions)
        {
            _thread = thread;
            _functions = functions;
        }

        public uint[] GetVersion()
        {
            ThrowIfDisposed();

            return _thread.Invoke(() =>
            {
                var func = _functions.GetFunctionDelegate<SDKDelegates.LibVer>();

                uint[] data = new uint[4];

                ThrowIfNotSucceded(func(Marshal.UnsafeAddrOfPinnedArrayElement(data, 0)));

                return data;
            });
        }

        public Task<uint[]> GetVersionAsync()
        {
            ThrowIfDisposed();

            return _thread.InvokeAsync(() =>
            {
                var func = _functions.GetFunctionDelegate<SDKDelegates.LibVer>();

                uint[] data = new uint[4];

                ThrowIfNotSucceded(func(Marshal.UnsafeAddrOfPinnedArrayElement(data, 0)));

                return data;
            });
        }

        private void ThrowIfNotSucceded(int res)
        {
            if (res != 0)
            {
                throw new SL600Exception();
            }
        }

        #region IDisposable Support

        public event EventHandler<EventArgs> Disposed;
        private bool disposedValue = false;

        private void ThrowIfDisposed()
        {
            if (disposedValue)
                throw new ObjectDisposedException("SL600SDK");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                _thread = null;
                _functions = null;

                disposedValue = true;

                Disposed?.Invoke(this, new EventArgs());
            }
        }

        public void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this);
        }

        #endregion
    }

    #endregion

    #region SL600SDK - HID

    public partial class SL600SDK
    {
        public RAWINPUTDEVICELIST[] GetAllHIDDevices()
        {
            // .NET 4.5.1
            //IntPtr pnumdevices = Marshal.AllocHGlobal(Marshal.SizeOf<uint>());
            // .NET 4.5
            IntPtr pnumdevices = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));

            try
            {
                // .NET 4.5.1
                /*
                uint retval = Win32Interop.GetRawInputDeviceList(IntPtr.Zero, pnumdevices,
                   (uint)Marshal.SizeOf<RAWINPUTDEVICELIST>());
                */
                // .NET 4.5
                uint retval = Win32Interop.GetRawInputDeviceList(IntPtr.Zero, pnumdevices,
                    (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));

                if (retval == (uint)0xFFFFFFFF)
                {
                    Win32Interop.GetLastErrorAndThrow();
                }

                // .NET 4.5.1
                //var devicesNum = Marshal.PtrToStructure<uint>(pnumdevices);
                // .NET 4.5
                var devicesNum = (uint)Marshal.PtrToStructure(pnumdevices, typeof(uint));


                var devices = new RAWINPUTDEVICELIST[devicesNum];

                // .NET 4.5.1
                /*
                retval = Win32Interop.GetRawInputDeviceList(
                    Marshal.UnsafeAddrOfPinnedArrayElement(devices, 0),
                    pnumdevices,
                   (uint)Marshal.SizeOf<RAWINPUTDEVICELIST>());
                */
                // .NET 4.5
                retval = Win32Interop.GetRawInputDeviceList(
                    Marshal.UnsafeAddrOfPinnedArrayElement(devices, 0),
                    pnumdevices,
                   (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST)));

                if (retval == (uint)0xFFFFFFFF)
                {
                    Win32Interop.GetLastErrorAndThrow();
                }

                return devices;
            }
            finally
            {
                Marshal.FreeHGlobal(pnumdevices);
            }
        }

        public RID_DEVICE_INFO GetHIDDeviceInfo(RAWINPUTDEVICELIST device)
        {
            // .NET 4.5.1
            //var size = Marshal.SizeOf<RID_DEVICE_INFO>();
            // .NET 4.5
            var size = Marshal.SizeOf(typeof(RID_DEVICE_INFO));

            var infoStructPtr = Marshal.AllocHGlobal(size);
            // .NET 4.5.1
            //var pSize = Marshal.AllocHGlobal(Marshal.SizeOf<uint>());
            // .NET 4.5
            var pSize = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));

            Marshal.StructureToPtr(size, pSize, false);

            try
            {
                var retval = Win32Interop.GetRawInputDeviceInfoW(device.hDevice, 0x2000000b, infoStructPtr, pSize);
                if (retval == 0xFFFFFFFF)
                {
                    Win32Interop.GetLastErrorAndThrow();
                }
                // .NET 4.5.1
                //return Marshal.PtrToStructure<RID_DEVICE_INFO>(infoStructPtr);
                // .NET 4.5
                return (RID_DEVICE_INFO)Marshal.PtrToStructure(infoStructPtr, typeof(RID_DEVICE_INFO));
            }
            finally
            {
                Marshal.FreeHGlobal(pSize);
                Marshal.FreeHGlobal(infoStructPtr);
            }
        }

        public string GetHIDDeviceName(RAWINPUTDEVICELIST device)
        {
            var strptr = Marshal.AllocHGlobal(255);
            // .NET 4.5.1
            //var pSize = Marshal.AllocHGlobal(Marshal.SizeOf<uint>());
            // .NET 4.5
            var pSize = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
            Marshal.StructureToPtr((uint)255, pSize, false);

            try
            {
                var retval = Win32Interop.GetRawInputDeviceInfoW(device.hDevice, 0x20000007, strptr, pSize);
                if (retval == 0xFFFFFFFF)
                {
                    Win32Interop.GetLastErrorAndThrow();
                }

                return Marshal.PtrToStringUni(strptr);
            }
            finally
            {
                Marshal.FreeHGlobal(pSize);
                Marshal.FreeHGlobal(strptr);
            }
        }
    }

    #endregion

    #region SL600SDK - RF

    public partial class SL600SDK
    {
        private byte[] GetByteDataFromAction(Action<IntPtr, IntPtr> action)
        {
            var dataPtr = Marshal.AllocHGlobal(MAX_RF_BUFFER);
            // .NET 4.5.1
            //var lenPtr = Marshal.AllocHGlobal(Marshal.SizeOf<int>());
            //Marshal.Copy(new byte[] { 0, 0, 0, 0 }, 0, lenPtr, Marshal.SizeOf<int>());
            // .NET 4.5
            var lenPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            Marshal.Copy(new byte[] { 0, 0, 0, 0 }, 0, lenPtr, Marshal.SizeOf(typeof(int)));


            try
            {
                action(dataPtr, lenPtr);
                // .NET 4.5.1
                //var len = Marshal.PtrToStructure<int>(lenPtr);
                // .NET 4.5
                var len = (int)Marshal.PtrToStructure(lenPtr, typeof(int));

                //byte p1 = (byte)(len & 0xFF);
                //byte p2 = (byte)((len >> 8) & 0xFF);
                //byte p3 = (byte)((len >> 16) & 0xFF);
                //byte p4 = (byte)((len >> 24) & 0xFF);

                if (len > MAX_RF_BUFFER)
                    throw new SL600Exception("Buffer size > MAX_RF_BUFFER");

                var ret = new byte[len];
                Marshal.Copy(dataPtr, ret, 0, (int)len);
                return ret;
            }
            finally
            {
                Marshal.FreeHGlobal(dataPtr);
                Marshal.FreeHGlobal(lenPtr);
            }
        }

        public IntPtr RFInitUSB(int HIDNum)
        {
            ThrowIfDisposed();
            return _thread.Invoke(() => _functions.GetFunctionDelegate<SDKDelegates.rf_init_usb>()(HIDNum));
        }

        public void RFClosePort(IntPtr handle)
        {
            ThrowIfDisposed();
            _thread.Invoke(() => ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_ClosePort>()(handle)));
        }

        public Task<IntPtr> RFInitUSBAsync(int HIDNum)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => _functions.GetFunctionDelegate<SDKDelegates.rf_init_usb>()(HIDNum));
        }

        private string RFGetDeviceNameInternal(int HIDNum)
        {
            IntPtr stringPtr = Marshal.AllocHGlobal(MAX_RF_STR_SIZE);

            try
            {
                ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_get_device_name>()
                    (HIDNum, stringPtr, MAX_RF_STR_SIZE));

                return Marshal.PtrToStringAnsi(stringPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(stringPtr);
            }
        }

        private Dictionary<int, Tuple<string, IntPtr>> RFGetDevicesNamesInternal(int maxNum)
        {
            IntPtr stringPtr = Marshal.AllocHGlobal(MAX_RF_STR_SIZE);
            var res = new Dictionary<int, Tuple<string, IntPtr>>();
            try
            {
                var func = _functions.GetFunctionDelegate<SDKDelegates.rf_get_device_name>();
                var init = _functions.GetFunctionDelegate<SDKDelegates.rf_init_usb>();
                for (int i = 0; i < maxNum; i++)
                {
                    IntPtr handle;
                    if ((handle = init(i)) != IntPtr.Zero - 1)
                        if (func(i, stringPtr, MAX_RF_STR_SIZE) == 0)
                        {
                            res.Add(i, new Tuple<string, IntPtr>(Marshal.PtrToStringAnsi(stringPtr), handle));
                        }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(stringPtr);
            }

            return res;
        }

        public Dictionary<int, Tuple<string, IntPtr>> RFGetDevicesNames(int maxNum = 32)
        {
            ThrowIfDisposed();
            return _thread.Invoke(() => RFGetDevicesNamesInternal(maxNum));
        }
        public Task<Dictionary<int, Tuple<string, IntPtr>>> RFGetDevicesNamesAsync(int maxNum = 32)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => RFGetDevicesNamesInternal(maxNum));
        }

        public string RFGetDeviceName(int HIDNum)
        {
            ThrowIfDisposed();
            return _thread.Invoke(() => RFGetDeviceNameInternal(HIDNum));
        }
        public Task<string> RFGetDeviceNameAsync(int HIDNum)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => RFGetDeviceNameInternal(HIDNum));
        }

        public void RFInitDeviceNumber(ushort icdev)
        {
            ThrowIfDisposed();
            _thread.Invoke(() => ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_init_device_number>()(icdev)));
        }

        public Task RFInitDeviceNumberAsync(ushort icdev)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_init_device_number>()(icdev)));
        }

        private int[] RFGetDeviceNumberInternal()
        {
            var res = new int[1];
            ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_get_device_number>()
                (Marshal.UnsafeAddrOfPinnedArrayElement(res, 0)));
            return res;
        }

        public int[] RFGetDeviceNumber()
        {
            ThrowIfDisposed();
            return _thread.Invoke(() => RFGetDeviceNumberInternal());
        }
        public Task<int[]> RFGetDeviceNumberAsync()
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => RFGetDeviceNumberInternal());
        }

        private byte[] RFGetSNRInternal(ushort icdev)
        {
            var ptr = Marshal.AllocHGlobal(MAX_RF_BUFFER);

            try
            {
                ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_get_snr>()(icdev, ptr));
                var res = new byte[MAX_RF_BUFFER];

                Marshal.Copy(ptr, res, 0, MAX_RF_BUFFER);

                return res;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public byte[] RFGetSNR(ushort icdev)
        {
            ThrowIfDisposed();
            return _thread.Invoke(() => RFGetSNRInternal(icdev));
        }
        public Task<byte[]> RFGetSNRAsync(ushort icdev)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => RFGetSNRInternal(icdev));
        }

        public void RFBeep(ushort icdev, byte msec)
        {
            ThrowIfDisposed();
            _thread.Invoke(() => ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_beep>()
                (icdev, msec)));
        }
        public Task RFBeepAsync(ushort icdev, byte msec)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_beep>()
                (icdev, msec)));
        }

        public void RFInitSAM(ushort icdev, byte bound)
        {
            ThrowIfDisposed();
            _thread.Invoke(() => ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_init_sam>()
                (icdev, bound)));
        }
        public Task RFInitSAMAsync(ushort icdev, byte bound)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_init_sam>()
                (icdev, bound)));
        }

        private byte[] RFSAMResetInternal(ushort icdev)
        {
            return GetByteDataFromAction((dataPtr, lenPtr) =>
                        ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_sam_rst>()
                        (icdev, dataPtr, lenPtr)));
        }

        public byte[] RFSAMReset(ushort icdev)
        {
            ThrowIfDisposed();
            return _thread.Invoke(() => RFSAMResetInternal(icdev));
        }
        public Task<byte[]> RFSAMResetAsync(ushort icdev)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => RFSAMResetInternal(icdev));
        }

        private byte[] RFSAMCOSInternal(ushort icdev, byte[] command)
        {
            return GetByteDataFromAction((dataPtr, lenPtr) =>
                        ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_sam_cos>()
                                            (icdev,
                                            Marshal.UnsafeAddrOfPinnedArrayElement(command, 0),
                                            (byte)command.Length,
                                            dataPtr,
                                            lenPtr)));
        }

        public byte[] RFSAMCOS(ushort icdev, byte[] command)
        {
            ThrowIfDisposed();
            return _thread.Invoke(() => RFSAMCOSInternal(icdev, command));
        }

        public Task<byte[]> RFSAMCOSAsync(ushort icdev, byte[] command)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => RFSAMCOSInternal(icdev, command));
        }

        private byte[] RFCOSCommandInternal(ushort icdev, byte[] command)
        {
            return GetByteDataFromAction((dataPtr, lenPtr) =>
                        ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_cos_command>()
                                            (icdev,
                                            Marshal.UnsafeAddrOfPinnedArrayElement(command, 0),
                                            (byte)command.Length,
                                            dataPtr,
                                            lenPtr)));
        }

        public byte[] RFCOSCommand(ushort icdev, byte[] command)
        {
            ThrowIfDisposed();
            return _thread.Invoke(() => RFCOSCommandInternal(icdev, command));
        }

        public Task<byte[]> RFCOSCommandAsync(ushort icdev, byte[] command)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => RFCOSCommandInternal(icdev, command));
        }

        public void RFInitType(ushort icdev, byte type)
        {
            ThrowIfDisposed();
            _thread.Invoke(() => _functions.GetFunctionDelegate<SDKDelegates.rf_init_type>()(icdev, type));
        }

        public Task RFInitTypeAsync(ushort icdev, byte type)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => _functions.GetFunctionDelegate<SDKDelegates.rf_init_type>()(icdev, type));
        }

        public void RFSetAntennaMode(ushort icdev, byte mode)
        {
            ThrowIfDisposed();
            _thread.Invoke(() => _functions.GetFunctionDelegate<SDKDelegates.rf_antenna_sta>()(icdev, mode));
        }
        public Task RFSetAntennaModeAsync(ushort icdev, byte mode)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => _functions.GetFunctionDelegate<SDKDelegates.rf_antenna_sta>()(icdev, mode));
        }

        public void RFSetAntennaMode(ushort icdev, bool mode)
        {
            ThrowIfDisposed();
            _thread.Invoke(() => _functions.GetFunctionDelegate<SDKDelegates.rf_antenna_sta>()(icdev, mode ? (byte)1 : (byte)0));
        }
        public Task RFSetAntennaModeAsync(ushort icdev, bool mode)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => _functions.GetFunctionDelegate<SDKDelegates.rf_antenna_sta>()(icdev, mode ? (byte)1 : (byte)0));
        }

        private byte[] RFResetTypeAInternal(ushort icdev, byte model)
        {
            return GetByteDataFromAction((dataPtr, lenPtr) =>
                        ThrowIfNotSucceded(_functions.GetFunctionDelegate<SDKDelegates.rf_typea_rst>()
                                            (icdev,
                                            model,
                                            dataPtr,
                                            lenPtr)));
        }
        public byte[] RFResetTypeA(ushort icdev, byte model)
        {
            ThrowIfDisposed();
            return _thread.Invoke(() => RFResetTypeAInternal(icdev, model));
        }
        public Task<byte[]> RFResetTypeAAsync(ushort icdev, byte model)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => RFResetTypeAInternal(icdev, model));
        }

        // mode: 0x26 - READ_STD (false)
        // mode: 0x52 - READ_ALL (true)
        public int RFRequest(ushort icdev, bool mode, IntPtr type)
        {
            ThrowIfDisposed();
            return _thread.Invoke(() =>
                _functions.GetFunctionDelegate<SDKDelegates.rf_request>()
                (
                    icdev,
                    mode ? (byte)0x52 : (byte)0x26,
                    type
                ));
        }

        public int RFAntiColl(ushort icdev, IntPtr pSnr, IntPtr pLen)
        {
            byte bcnt = 0x04; // must be 4.
            // pSnr: Unique Serial Number.
            // pLen: Size of response data.
            ThrowIfDisposed();
            return _thread.Invoke(() =>
                _functions.GetFunctionDelegate<SDKDelegates.rf_anticoll>()
                (
                    icdev,
                    bcnt,
                    pSnr,
                    pLen
                ));
        }

        public int RFSelect(ushort icdev, IntPtr pSnr, byte srcLen, IntPtr size)
        {
            ThrowIfDisposed();
            return _thread.Invoke(() =>
                _functions.GetFunctionDelegate<SDKDelegates.rf_select>()
                (
                    icdev,
                    pSnr,
                    srcLen,
                    size
                ));
        }

        public int RFM1Authentication2(ushort icdev, byte mode, byte secnr, IntPtr key)
        {
            ThrowIfDisposed();
            return _thread.Invoke(() =>
                _functions.GetFunctionDelegate<SDKDelegates.rf_M1_authentication2>()
                (
                    icdev,
                    mode,
                    secnr,
                    key
                ));
        }

        public int RFM1Read(ushort icdev, byte addr, IntPtr data, IntPtr pLen)
        {
            ThrowIfDisposed();
            return _thread.Invoke(() =>
                _functions.GetFunctionDelegate<SDKDelegates.rf_M1_read>()
                (
                    icdev,
                    addr,
                    data,
                    pLen
                ));
        }
    }

    #endregion

    #region SL600SDK - Encryption

    public partial class SL600SDK
    {
        private void DESEncryptInternal(byte[] szOut, byte[] szIn, byte[] key)
        {
            var func = _functions.GetFunctionDelegate<SDKDelegates.des_encrypt>();

            ThrowIfNotSucceded(func(
                Marshal.UnsafeAddrOfPinnedArrayElement(szOut, 0),
                Marshal.UnsafeAddrOfPinnedArrayElement(szIn, 0),
                (uint)szIn.Length,
                Marshal.UnsafeAddrOfPinnedArrayElement(key, 0),
                (uint)key.Length)
                );
        }
        public void DESEncrypt(byte[] szOut, byte[] szIn, byte[] key)
        {
            ThrowIfDisposed();
            _thread.Invoke(() => DESEncryptInternal(szOut, szIn, key));
        }
        public Task DESEncryptAsync(byte[] szOut, byte[] szIn, byte[] key)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => DESEncryptInternal(szOut, szIn, key));
        }

        private void DESDecryptInternal(byte[] szOut, byte[] szIn, byte[] key)
        {
            var func = _functions.GetFunctionDelegate<SDKDelegates.des_decrypt>();

            ThrowIfNotSucceded(func(
                Marshal.UnsafeAddrOfPinnedArrayElement(szOut, 0),
                Marshal.UnsafeAddrOfPinnedArrayElement(szIn, 0),
                (uint)szIn.Length,
                Marshal.UnsafeAddrOfPinnedArrayElement(key, 0),
                (uint)key.Length)
                );
        }
        public void DESDecrypt(byte[] szOut, byte[] szIn, byte[] key)
        {
            ThrowIfDisposed();
            _thread.Invoke(() => DESDecryptInternal(szOut, szIn, key));
        }
        public Task DESDecryptAsync(byte[] szOut, byte[] szIn, byte[] key)
        {
            ThrowIfDisposed();
            return _thread.InvokeAsync(() => DESDecryptInternal(szOut, szIn, key));
        }
    }

    #endregion

    #endregion

    #region SL600Device

    public class SL600Device
    {
        public enum DeviceType
        {
            ISO14443A = 0x41,
            ISO14443B = 0x42,
            AT88RF020 = 0x72,
            ISO15693 = 0x31
        }

        public SL600SDK SDK { get; private set; }
        public ushort DeviceID { get; private set; }
        internal SL600Device(SL600SDK sdk, ushort icdev)
        {
            SDK = sdk;
            DeviceID = icdev;
        }

        public void SetType(DeviceType type)
        {
            SDK.RFInitType(DeviceID, (byte)type);
        }

        public Task SetTypeAsync(DeviceType type)
        {
            return SDK.RFInitTypeAsync(DeviceID, (byte)type);
        }
    }

    #endregion

    #region SL600SDKFactory

    public class SL600SDKFactory
    {
        private static SL600SDKFactory _factory;

        private object _syncObj = new object();

        private DispathcedThread _thread;
        private FunctionPointersContainer _functions;
        private IntPtr _hModule;
        
        private string _pathToDll;
        private int _refCounter;

        internal SL600SDKFactory(string path)
        {
            _pathToDll = path;
            _refCounter = 0;
        }

        public SL600SDK CreateInstance()
        {
            lock (_syncObj)
            {
                if (_hModule == IntPtr.Zero)
                {
                    _hModule = Win32Interop.LoadLibraryExW(_pathToDll, IntPtr.Zero, 0x00001000);
                    if (_hModule == IntPtr.Zero)
                    {
                        // TODO: Smartcard error should occur here.
                        // Nedd to break and see how to handle its by application.
                        Win32Interop.GetLastErrorAndThrow();
                    }
                    else
                    {
                        // If above error occur enable below code to check
                        // when exception raise how application handle if in 
                        // global exception handler.
                        //Win32Interop.GetLastErrorAndThrow();
                    }
                }

                if (_thread == null)
                {
                    _thread = new DispathcedThread();
                }

                if (_functions == null)
                {
                    _functions = new FunctionPointersContainer(_hModule);
                }

                var inst = new SL600SDK(_thread, _functions);
                inst.Disposed += Inst_Disposed;
                Interlocked.Increment(ref _refCounter);

                return inst;
            }
        }

        public void Release()
        {
            lock (_syncObj)
            {
                if (null != _thread) _thread.Dispose();
                _thread = null;
                _functions = null;

                if (_hModule != IntPtr.Zero)
                {
                    Win32Interop.FreeLibrary(_hModule);
                }
                _hModule = IntPtr.Zero;

                // reset static variables
                _factory = null;
            }
        }

        private void Inst_Disposed(object sender, EventArgs e)
        {
            ((SL600SDK)sender).Disposed -= Inst_Disposed;
            var val = Interlocked.Decrement(ref _refCounter);

            if (val == 0)
            {
                Release();
            }
        }

        public static SL600SDKFactory CreateFactory(string path)
        {
            if (_factory == null)
            {
                _factory = new SL600SDKFactory(path);
            }

            return _factory;
        }
    }

    #endregion

    #region TLVReader relateed enum/struct/classes

    #region TLVClasses enum

    public enum TLVClasses
    {
        UNIVERSAL = 0,
        APPLICATION = 1,
        CONTEXT_SPECIFIC = 2,
        PRIVATE = 3
    }

    #endregion

    #region TLVTag (struct)

    public struct TLVTag
    {
        public uint Value;

        public static implicit operator uint(TLVTag tag) { return tag.Value; }
        public TLVTag(uint value)
        {
            Value = value;
        }

        public TLVClasses GetClass()
        {
            return (TLVClasses)((Value >> 6) & 0x03);
        }


        public bool IsConstructed()
        {
            return ((Value >> 5) & 0x01) == 1 ? true : false;
        }

        public byte GetTagValue()
        {
            return (byte)(Value & 0b00011111);
        }

        public override string ToString()
        {
            return "TLV Tag: " + Value.ToString("X2");
        }
    }

    #endregion

    #region TLVObject

    public class TLVObject
    {
        public TLVTag Tag { get; private set; }
        public byte[] Value { get; private set; }
        public bool IsEMV { get; set; }

        public TLVObject(TLVTag tag, byte[] value)
        {
            Tag = tag;
            Value = value;
        }

        public List<TLVObject> ReadInnerTlvTags()
        {
            using (var ms = new MemoryStream(Value))
            using (var reader = new TLVReader(ms) { RemoveEMVPadding = IsEMV })
            {
                return reader.ReadTags();
            }
        }

        public string DumpInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("T   L   V");

            DumpInfo(0, sb);

            return sb.ToString();
        }

        private void DumpInfo(int offset, StringBuilder sb)
        {
            for (int i = 0; i < offset; i++)
                sb.Append("         ");

            sb.AppendFormat("{0:X2}   {1:X2}", Tag.Value, Value.Length);
            if (Tag.IsConstructed())
            {
                sb.AppendLine();
                foreach (var t in ReadInnerTlvTags())
                {
                    t.DumpInfo(offset + 1, sb);
                }
            }
            else
            {
                sb.AppendLine("   " + string.Join(" ", Value.Select(x => x.ToString("X2"))));
            }
        }

        public override string ToString()
        {
            return string.Format("TLV Tag:0x{0:X2} Length:0x{1:X2} dec:{1}", Tag.Value, Value.Length);
        }
    }

    #endregion

    #region TLVReader

    public class TLVReader : IDisposable
    {
        public bool RemoveEMVPadding { get; set; }
        public Stream SourceStream { get; private set; }

        public uint TotalLength { get; set; }
        public TLVReader(Stream stream)
        {
            SourceStream = stream;
        }

        public List<TLVObject> ReadTags()
        {
            var res = new List<TLVObject>();

            while (true)
            {
                var tag = ReadNextTag();
                if (tag == null)
                    break;

                var length = ReadLength();
                if (length == 0)
                    continue;
                if (length == null)
                    throw new Exception("Indefinite length not supported");

                var buf = new byte[length.Value];
                var readIndex = 0;
                while (true)
                {
                    var lengthRead = SourceStream.Read(buf, readIndex, length.Value - readIndex);

                    if (lengthRead == 0)
                        throw new Exception("Unexpected end of stream while reading data");

                    readIndex += lengthRead;

                    if (length == readIndex)
                        break; // all data read
                }
                res.Add(new TLVObject(new TLVTag(tag.Value), buf) { IsEMV = RemoveEMVPadding });
            }

            return res;
        }

        private uint? ReadNextTag()
        {
            uint tagValue = 0;
            int byteCount = 0;

            while (true)
            {
                if (byteCount >= 4)
                    throw new Exception(string.Format("Tag is more than 4 bytes long. Partial tag value: 0x{0:x8}", tagValue));

                var thisByte = SourceStream.ReadByte();
                if (thisByte == -1)
                {
                    if (byteCount > 0)
                        throw new Exception(string.Format("Unexpected end of tag data. Partial tag value: 0x{0:x8}", tagValue));

                    return null; // end of stream, no tag found
                }

                // NOTE: check 0xff usage!!!
                if (RemoveEMVPadding && byteCount == 0 && (thisByte == 0x00 || thisByte == 0xff))
                    continue; // skip if not already reading tag (EMV allows 0x00 or 0xff padding between TLV entries)

                byteCount++;
                tagValue <<= 8;
                tagValue |= (uint)thisByte;

                if (byteCount == 1 && (thisByte & 0x1f) != 0x1f)
                    return tagValue; // no more data (tag number is 0 to 30 inclusive, so only one octet used)

                if (byteCount != 1 && (thisByte & 0x80) == 0)
                    return tagValue; // no more data (bit 8 is not set for last octet)
            }

        }

        public Tuple<TLVTag, int> PeekTagAndLength()
        {
            while (true)
            {
                var tag = ReadNextTag();
                if (tag == null)
                    break;

                var length = ReadLength();
                if (length == 0)
                    continue;
                if (length == null)
                    throw new Exception("Indefinite length not supported");


                return new Tuple<TLVTag, int>(new TLVTag(tag.Value), length.Value);
            }
            return null;
        }

        private int? ReadLength()
        {
            var lenByte = SourceStream.ReadByte();

            if (lenByte == -1)
                throw new IOException("Unexpected end of stream");

            if ((lenByte & 0x80) == 0)
                return lenByte;

            int length = 0;
            int lengthBytes = lenByte & 0x7F;

            if (lengthBytes == 0)
                return null;

            if (lengthBytes > 4)
                throw new IOException("Invalid length value");

            for (int i = 0; i < lengthBytes; i++)
            {
                lenByte = SourceStream.ReadByte();
                if (lenByte == -1)
                    throw new IOException("Unexpected end of stream");

                length <<= 8;
                length |= (int)lenByte;
            }

            return length;
        }

        public void Dispose()
        {
            ((IDisposable)SourceStream).Dispose();
        }
    }

    #endregion

    #endregion

    #region APDUMessage related interface/class

    #region IAPDUMessage

    public interface IAPDUMessage
    {
        byte Class { get; set; }
        byte Instruction { get; set; }
        byte Param1 { get; set; }
        byte Param2 { get; set; }
        byte[] Data { get; set; }
        int? ExpectedResponseLength { get; set; }
        void WriteRawData(Stream str);
    }

    #endregion

    #region APDUMessage

    public class APDUMessage : IAPDUMessage
    {
        public byte Class { get; set; }
        public byte Instruction { get; set; }
        public byte Param1 { get; set; }
        public byte Param2 { get; set; }
        public byte[] Data { get; set; }
        public int? ExpectedResponseLength { get; set; }


        public void WriteRawData(Stream str)
        {
            str.WriteByte(Class);
            str.WriteByte(Instruction);
            str.WriteByte(Param1);
            str.WriteByte(Param2);

            if (Data != null && Data.Length != 0)
            {
                var lcBytes = GetLengthData(Data.Length);

                str.Write(lcBytes, 0, lcBytes.Length);
                str.Write(Data, 0, Data.Length);
            }

            if (ExpectedResponseLength.HasValue)
            {
                var leBytes = GetLengthData(ExpectedResponseLength.Value);
                str.Write(leBytes, 0, leBytes.Length);
            }

        }

        private byte[] GetLengthData(int len)
        {
            byte[] bytes = null;

            if (len <= 256)
            {
                bytes = new byte[1];
                if (len == 256)
                    bytes[0] = 0x00;
                else
                    bytes[0] = (byte)len;
            }
            else if (len <= 65536)
            {
                bytes = new byte[3];
                bytes[0] = 0x00;

                if (len == 65536)
                {
                    bytes[1] = 0x00;
                    bytes[2] = 0x00;
                }
                else
                {
                    bytes[2] = (byte)(len & 0xFF);
                    bytes[1] = (byte)((len >> 8) & 0xFF);
                }
            }

            return bytes;
        }
    }

    #endregion

    #endregion

    #region SmartCardReader related interfaces/classes

    #region ISmartCardReader

    public interface ISmartCardReader
    {
        TLVObject SendAPDU(IAPDUMessage message, out ushort sw12);
        TLVObject SendAPDU(IAPDUMessage message);
        byte[] Reset();
        bool IsCardExist();
    }

    #endregion

    #region ISmartCardReaderExtensions (class)

    public static class ISmartCardReaderExtensions
    {
        public static Task<bool> IsCardExistAsync(this ISmartCardReader reader)
        {
            return Task.Run(() => reader.IsCardExist());
        }
    }

    #endregion

    #region SmartCardReaderException

    public class SmartCardReaderException : Exception
    {
        public ushort SW12 { get; set; }
        public SmartCardReaderException(string message) : base(message) { }
        public SmartCardReaderException(ushort sw12) { SW12 = sw12; }
        public SmartCardReaderException(string message, ushort sw12) : base(message) { SW12 = sw12; }
    }

    #endregion

    #region SmartCardReader

    public abstract class SmartCardReader : ISmartCardReader
    {
        public bool IsEmv { get; set; }

        public abstract bool IsCardExist();
        public abstract byte[] Reset();

        public TLVObject SendAPDU(IAPDUMessage message, out ushort sw12)
        {
            using (var ms = new MemoryStream())
            {
                message.WriteRawData(ms);
                var dataToSend = ms.ToArray();

#if DEBUG
                System.Diagnostics.Debug.WriteLine(
                    string.Format("C-APDU => {0}", string.Join(" ", dataToSend.Select(x => x.ToString("X2")))));
#endif

                var resp = SendCommand(dataToSend);

#if DEBUG
                System.Diagnostics.Debug.WriteLine(
                    string.Format("R-APDU <= {0}", string.Join(" ", resp.Select(x => x.ToString("X2")))));
#endif

                if (resp == null || resp.Length < 2)
                    throw new InvalidOperationException("APDU Response is null or empty");

                sw12 = (ushort)((ushort)(resp[resp.Length - 2] << 8) | (ushort)(resp[resp.Length - 1]));
                if (resp.Length == 2)
                {
                    return null;
                }

                using (var respMs = new MemoryStream(resp))
                using (var reader = new TLVReader(respMs) { RemoveEMVPadding = IsEmv })
                {
                    var res = reader.PeekTagAndLength();
                    if (res.Item2 > resp.Length - 2)
                    {
                        System.Diagnostics.Debug.WriteLine("Too short result, readjusting length");
                        message.ExpectedResponseLength = res.Item2 + (int)respMs.Position;

                        return SendAPDU(message, out sw12);
                    }
                    respMs.Position = 0;
                    return reader.ReadTags().First();
                }
            }
        }

        public TLVObject SendAPDU(IAPDUMessage message)
        {
            ushort sw12 = 0;
            var retVal = SendAPDU(message, out sw12);
            if (sw12 == 0x9000)
                return retVal;
            else if (sw12 == 0x6700 && !message.ExpectedResponseLength.HasValue) //Wrong length maybe need to try set expected lenght
            {
                message.ExpectedResponseLength = 0xF;
                return SendAPDU(message);
            }
            else if (sw12 == 0x6700 && message.ExpectedResponseLength.HasValue && message.ExpectedResponseLength.Value < 0xFFFFFF)
            {
                message.ExpectedResponseLength = (message.ExpectedResponseLength << 4) & 0xF;
                return SendAPDU(message);
            }
            else
                throw new SmartCardReaderException($"SW Status is not OK ({sw12.ToString("X2")})", sw12);
        }

        protected abstract byte[] SendCommand(byte[] command);
    }

    #endregion

    #endregion

    #region Sl600SmartCardReader

    public class Sl600SmartCardReader : SmartCardReader, IDisposable
    {
        public SL600SDK SDK { get; private set; }
        public ushort ICDev { get; private set; }

        public IntPtr Handle { get; private set; }

        public bool Connected { get; private set; }
        public string ConnectStatus { get; private set; }

        public Sl600SmartCardReader(SL600SDK sdk, int icdev)
        {
            SDK = sdk;
            ICDev = (ushort)icdev;
            Handle = (null != SDK) ? SDK.RFInitUSB(icdev) : IntPtr.Zero;
            if (Handle == IntPtr.Zero)
            {
                Connected = false;
                //Console.WriteLine("Port Handle is zero.");
                ConnectStatus = "Port Handle is zero.";
            }
            if (Handle == new IntPtr(-1))
            {
                Connected = false;
                //Console.WriteLine("Port in used by another process.");
                ConnectStatus = "Port in used by another process.";
            }
            Connected = true;
            ConnectStatus = "Port opened.";
        }
        public override byte[] Reset()
        {
            if (null != SDK)
            {
                SDK.RFSetAntennaMode(ICDev, false);
                Sleep(50);
            }
            if (null != SDK)
            {
                SDK.RFInitType(ICDev, (byte)'A');
                Sleep(50);
            }
            if (null != SDK)
            {
                SDK.RFSetAntennaMode(ICDev, true);
                Sleep(50);
            }
            return (null != SDK) ? SDK.RFResetTypeA(ICDev, 0) : null;
        }

        protected override byte[] SendCommand(byte[] command)
        {
            return (null != SDK) ? SDK.RFCOSCommand(ICDev, command) : null;
        }

        public void DoEvents()
        {
            try
            {
                System.Windows.Forms.Application.DoEvents();
            }
            catch { }
        }

        public void Sleep(int ms)
        {
            if (ms > 0)
            {
                //Thread.Sleep(ms);
                DoEvents();
            }
        }

        public override bool IsCardExist()
        {
            try
            {
                return 0 == SDK._thread.Invoke(() =>
                {
                    var dataPtr = Marshal.AllocHGlobal(SL600SDK.MAX_RF_BUFFER);
                    // .NET 4.5.1
                    //var lenPtr = Marshal.AllocHGlobal(Marshal.SizeOf<int>());
                    //Marshal.Copy(new byte[] { 0, 0, 0, 0 }, 0, lenPtr, Marshal.SizeOf<int>());
                    // .NET 4.5
                    var lenPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
                    Marshal.Copy(new byte[] { 0, 0, 0, 0 }, 0, lenPtr, Marshal.SizeOf(typeof(int)));

                    try
                    {
                        int ret = (null != SDK) ? SDK._functions.GetFunctionDelegate<SDKDelegates.rf_typea_rst>()(ICDev, 0, dataPtr, lenPtr) : -1;
                        return ret;
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(dataPtr);
                        Marshal.FreeHGlobal(lenPtr);
                    }
                });
            }
            catch (SL600Exception)
            {
                return false;
            }
        }

        private int RFRequest(bool STDMode = false)
        {
            int status = 0;

            // tagType(mode): 0x26 - READ_STD (false)
            // tagType(mode): 0x52 - READ_ALL (true)
            var tagType = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ushort)));

            if (STDMode)
                Marshal.Copy(new byte[] { 0, 0, 0, 0x26 }, 0, tagType, Marshal.SizeOf(typeof(ushort)));
            else Marshal.Copy(new byte[] { 0, 0, 0, 0x52 }, 0, tagType, Marshal.SizeOf(typeof(ushort)));
            try
            {
                status = (null != SDK) ? SDK.RFRequest(ICDev, false, tagType) : -1;
                if (status != 0)
                {
                    //Console.WriteLine("RFRequest failed.");
                }
                Sleep(50);
            }
            finally
            {
                Marshal.FreeHGlobal(tagType);
            }
            return status;
        }

        private int RFAntiCollAndSelect(out byte[] buffers)
        {
            int status = 0;
            buffers = null;
            // for RFAntiColl/RFSelect
            var serialNoPtr = Marshal.AllocHGlobal(SL600SDK.MAX_RF_BUFFER);
            var serialNoLenPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)));
            Marshal.Copy(new byte[] { 0 }, 0, serialNoLenPtr, Marshal.SizeOf(typeof(byte)));
            // for RFSelect
            var psizePtr = Marshal.AllocHGlobal(SL600SDK.MAX_RF_BUFFER);
            try
            {
                status = (null != SDK) ? SDK.RFAntiColl(ICDev, serialNoPtr, serialNoLenPtr) : -1;
                if (status != 0)
                {
                    //Console.WriteLine("RFAntiColl failed.");
                }
                Sleep(50);
                if (status == 0)
                {
                    // adjust size to actual size.
                    byte snrSize = Marshal.ReadByte(serialNoLenPtr, 0);
                    status = (null != SDK) ? SDK.RFSelect(ICDev, serialNoPtr, snrSize, psizePtr) : -1;
                    if (status != 0)
                    {
                        //Console.WriteLine("RFSelect failed.");
                    }
                    if (status == 0)
                    {
                        buffers = new byte[snrSize];
                        Marshal.Copy(serialNoPtr, buffers, 0, snrSize);
                    }
                    Sleep(50);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(psizePtr);
                Marshal.FreeHGlobal(serialNoPtr);
                Marshal.FreeHGlobal(serialNoLenPtr);
            }
            return status;
        }

        private int RFM1Authentication2(byte[] key, byte mode = 0x61)
        {
            int status = 0;
            int keySize = 6; // key size = 6 byte.

            if (null == key || key.Length != keySize)
            {
                //Console.WriteLine("Invalid Key Length.");
                return -1;
            }
            //byte mode = 0x61; // KeyA
            //byte mode = 0x62; // KeyB
            byte block_abs = 0;
            var secureKeyPtr = Marshal.AllocHGlobal(keySize);
            Marshal.Copy(key, 0, secureKeyPtr, keySize);

            try
            {
                status = (null != SDK) ? SDK.RFM1Authentication2(ICDev, mode, block_abs, secureKeyPtr) : -1;
                if (status != 0)
                {
                    Console.WriteLine("RFM1Authentication2 failed.");
                }
                Sleep(50);
            }
            finally
            {
                Marshal.FreeHGlobal(secureKeyPtr);
            }
            return status;
        }

        private int RFM1Read(out byte[] buffers, byte blockNo = 0)
        {
            int status = 0;

            var dataPtr = Marshal.AllocHGlobal(SL600SDK.MAX_RF_BUFFER);
            var dataLenPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)));
            Marshal.Copy(new byte[] { 0xFF }, 0, dataLenPtr, Marshal.SizeOf(typeof(byte)));

            try
            {
                status = (null != SDK) ? SDK.RFM1Read(ICDev, blockNo, dataPtr, dataLenPtr) : -1;
                Sleep(50);
                if (status != 0)
                {
                    //Console.WriteLine("RFM1Read failed.");
                    buffers = null;
                }
                else
                {
                    byte dataSize = Marshal.ReadByte(dataLenPtr, 0);
                    //Console.WriteLine("Data Size: {0}", dataSize);
                    buffers = new byte[dataSize];
                    Marshal.Copy(dataPtr, buffers, 0, dataSize);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(dataLenPtr);
                Marshal.FreeHGlobal(dataPtr);
            }
            return status;
        }

        private string BufferToString(byte[] buffers)
        {
            string result = string.Empty;
            if (null == buffers || buffers.Length <= 0) return result;
            try
            {
                StringBuilder sb = new StringBuilder(buffers.Length * 2);
                foreach (byte b in buffers)
                {
                    sb.AppendFormat("{0:X2} ", b);
                }
                result = sb.ToString().Trim();
            }
            finally
            {
            }
            return result;
        }

        public class ReadCardSerialResult
        {
            public int status { get; set; }

            public byte[] RawSerialNo { get; set; }
            public string SerialNo { get; set; }
        }

        public class ReadCardBlockResult
        {
            public int status { get; set; }

            public byte[] RawBlock0 { get; set; }
            public string Block0 { get; set; }
            public byte[] RawBlock1 { get; set; }
            public string Block1 { get; set; }
            public byte[] RawBlock2 { get; set; }
            public string Block2 { get; set; }
            public byte[] RawBlock3 { get; set; }
            public string Block3 { get; set; }
        }

        public ReadCardSerialResult ReadCardSerial(bool STDMode = true)
        {
            ReadCardSerialResult result = new ReadCardSerialResult();
            try
            {
                int status = 0;
                try
                {
                    if (null != SDK) SDK.RFSetAntennaMode(ICDev, false);
                    Sleep(50);

                    if (null != SDK) SDK.RFInitType(ICDev, (byte)'A');
                    Sleep(50);

                    if (null != SDK) SDK.RFSetAntennaMode(ICDev, true);
                    Sleep(50);

                    status = RFRequest(STDMode);

                    byte[] buffers = null;
                    if (status == 0) status = RFAntiCollAndSelect(out buffers);
                    if (buffers != null)
                    {
                        result.RawSerialNo = buffers;
                        result.SerialNo = BufferToString(buffers);
                    }

                    result.status = status;
                }
                finally
                {

                }
            }
            catch (SL600Exception)
            {
                result.status = -1;
            }
            return result;
        }

        public ReadCardBlockResult ReadCardBlock(byte[] key = null,
            bool STDMode = true, bool KeyA = true)
        {
            ReadCardBlockResult result = new ReadCardBlockResult();
            try
            {
                int status = 0;
                try
                {
                    byte[] buffers = null;

                    // mode = 0x61 (KeyA), mode = 0x62 (KeyB)
                    byte mode = (KeyA) ? (byte)0x61 : (byte)0x62;
                    if (status == 0) status = RFM1Authentication2(key, mode);

                    if (status == 0)
                    {
                        status = RFM1Read(out buffers, 0);
                        result.RawBlock0 = buffers;
                        result.Block0 = BufferToString(buffers);
                    }
                    if (status == 0)
                    {
                        status = RFM1Read(out buffers, 1);
                        result.RawBlock1 = buffers;
                        result.Block1 = BufferToString(buffers);
                    }
                    if (status == 0)
                    {
                        status = RFM1Read(out buffers, 2);
                        result.RawBlock2 = buffers;
                        result.Block2 = BufferToString(buffers);
                    }
                    if (status == 0)
                    {
                        status = RFM1Read(out buffers, 3);
                        result.RawBlock3 = buffers;
                        result.Block3 = BufferToString(buffers);
                    }

                    result.status = status;
                }
                finally
                {

                }
            }
            catch (SL600Exception)
            {
                result.status = -1;
            }
            return result;
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Handle != IntPtr.Zero)
                    {
                        if (null != SDK)
                        {
                            SDK.RFClosePort(Handle);

                            Connected = false;
                            ConnectStatus = "Port closed.";
                        }
                    }
                    Handle = IntPtr.Zero;
                }
                if (null != SDK)
                {
                    //SDK = null;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this);
        }

        #endregion
    }

    #endregion
}

namespace DMT.Smartcard
{
    #region DelegateExtensionMethods

    /// <summary>
    /// Delegate extension methods (Internal for Singelton class).
    /// </summary>
    internal static class SingeltonDelegateExtensionMethods
    {
        /// <summary>
        /// Extension method which marshals events back onto the main thread
        /// </summary>
        /// <param name="multicast">The MulticastDelegate instance.</param>
        /// <param name="sender">The sender instance.</param>
        /// <param name="args">The arguments for delegate.</param>
        public static void Raise(this MulticastDelegate multicast, object sender, EventArgs args)
        {
            foreach (Delegate del in multicast.GetInvocationList())
            {
                // Try for WPF first
                DispatcherObject dispatcherTarget = del.Target as DispatcherObject;
                if (dispatcherTarget != null && !dispatcherTarget.Dispatcher.CheckAccess())
                {
                    // WPF target which requires marshaling
                    dispatcherTarget.Dispatcher.BeginInvoke(del, sender, args);
                }
                else
                {
                    // Maybe its WinForms?
                    ISynchronizeInvoke syncTarget = del.Target as ISynchronizeInvoke;
                    if (syncTarget != null && syncTarget.InvokeRequired)
                    {
                        // WinForms target which requires marshaling
                        syncTarget.BeginInvoke(del, new object[] { sender, args });
                    }
                    else
                    {
                        // Just do it.
                        del.DynamicInvoke(sender, args);
                    }
                }
            }
        }
        /// <summary>
        /// Extension method which marshals actions back onto the main thread
        /// </summary>
        /// <typeparam name="T">The target class.</typeparam>
        /// <param name="action">The action delegate.</param>
        /// <param name="args">The arguments for delegate.</param>
        public static void Raise<T>(this Action<T> action, T args)
        {
            // Try for WPF first
            DispatcherObject dispatcherTarget = action.Target as DispatcherObject;
            if (dispatcherTarget != null && !dispatcherTarget.Dispatcher.CheckAccess())
            {
                // WPF target which requires marshaling
                dispatcherTarget.Dispatcher.BeginInvoke(action, args);
            }
            else
            {
                // Maybe its WinForms?
                ISynchronizeInvoke syncTarget = action.Target as ISynchronizeInvoke;
                if (syncTarget != null && syncTarget.InvokeRequired)
                {
                    // WinForms target which requires marshaling
                    syncTarget.BeginInvoke(action, new object[] { args });
                }
                else
                {
                    // Just do it.
                    action.DynamicInvoke(args);
                }
            }
        }
    }

    #endregion

    #region NTheadSingelton<T> class

    /// <summary>
    /// The NTheadSingelton<T> class
    /// </summary>
    /// <typeparam name="T">The target class.</typeparam>
    public abstract class NTheadSingelton<T> where T : NTheadSingelton<T>
    {
        #region Internal Variables

        private Thread _th;
        private bool _running = false;
        private bool _isExit = false;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected NTheadSingelton()
        {
            if (null != System.Windows.Application.Current)
            {
                System.Windows.Application.Current.Exit += Current_Exit;
                System.Windows.Application.Current.SessionEnding += Current_SessionEnding;
            }
            if (null != System.Windows.Forms.Form.ActiveForm)
            {
                System.Windows.Forms.Application.ThreadExit += Application_ThreadExit;
                System.Windows.Forms.Application.ApplicationExit += Application_ApplicationExit;
            }
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NTheadSingelton()
        {
            Shutdown();
            if (null != System.Windows.Forms.Form.ActiveForm)
            {
                System.Windows.Forms.Application.ThreadExit -= Application_ThreadExit;
                System.Windows.Forms.Application.ApplicationExit -= Application_ApplicationExit;
            }
            if (null != System.Windows.Application.Current)
            {
                System.Windows.Application.Current.Exit -= Current_Exit;
                System.Windows.Application.Current.SessionEnding -= Current_SessionEnding;
            }
        }

        #endregion

        #region Protected Variables

        /// <summary>
        /// Protected access thread name.
        /// </summary>
        protected string ThreadName = "";
        /// <summary>
        /// Gets is application in exit state.
        /// </summary>
        protected bool IsExit { get { return _isExit; } }

        #endregion

        #region Private Methods


        private void Application_ThreadExit(object sender, EventArgs e)
        {
            //Console.WriteLine("WindowForm:Application.ThreadExit");
            _isExit = true;
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            //Console.WriteLine("WindowForm:Application.Exit");
            _isExit = true;
        }

        private void Current_SessionEnding(object sender, System.Windows.SessionEndingCancelEventArgs e)
        {
            //Console.WriteLine("WPF:Current.SessionEnding");
            _isExit = true;
        }

        private void Current_Exit(object sender, System.Windows.ExitEventArgs e)
        {
            //Console.WriteLine("WPF:Current.Exit");
            _isExit = true;
        }

        private void Processing()
        {
            while (null != _th && _running && !_isExit)
            {
                OnProcessing();
            }
            Shutdown();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// OnProcessing virtual method.
        /// </summary>
        protected virtual void OnProcessing() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start service.
        /// </summary>
        public void Start()
        {
            if (null == _th)
            {
                _th = new Thread(this.Processing);
                _th.Priority = ThreadPriority.BelowNormal;
                _th.Name = ThreadName;
                _th.IsBackground = true;

                _running = true;

                _th.Start();
            }
        }
        /// <summary>
        /// Shutdown service.
        /// </summary>
        public void Shutdown()
        {
            Console.WriteLine("Shutdown");
            _running = false;
            if (null != _th)
            {
                try
                {
                    _th.Abort();
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
                _th = null;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets is server is running.
        /// </summary>
        public bool IsRunning { get { return (null != _th && _running); } }

        #endregion

        #region Singelton

        /// <summary>
        /// Static Readonly property.
        /// </summary>
        private static readonly Lazy<T> Lazy =
            new Lazy<T>(() => {
                T ret = default(T);
                lock (typeof(T))
                {
                    ret = Activator.CreateInstance(typeof(T), true) as T;
                }
                return ret;
            });
        /// <summary>
        /// Gets singleton instance.
        /// </summary>
        public static T Instance => Lazy.Value;

        #endregion
    }

    #endregion

    #region EventHandlers and EventArgs.

    public delegate void M1CardReadSerialEventHandler(object sender, M1CardReadSerialEventArgs e);

    public class M1CardReadSerialEventArgs
    {
        #region Constructor

        internal M1CardReadSerialEventArgs(Sl600SmartCardReader.ReadCardSerialResult value) : base()
        {
            status = value.status;
            RawSerialNo = value.RawSerialNo;
            SerialNo = value.SerialNo;
        }

        #endregion

        #region Public Properties

        public int status { get; set; }
        public byte[] RawSerialNo { get; set; }
        public string SerialNo { get; set; }

        #endregion
    }

    public delegate void M1CardReadBlockEventHandler(object sender, M1CardReadBlockEventArgs e);

    public class M1CardReadBlockEventArgs
    {
        #region Constructor

        internal M1CardReadBlockEventArgs(Sl600SmartCardReader.ReadCardBlockResult value) : base()
        {
            status = value.status;
            RawBlock0 = value.RawBlock0;
            Block0 = value.Block0;
            RawBlock1 = value.RawBlock1;
            Block1 = value.Block1;
            RawBlock2 = value.RawBlock2;
            Block2 = value.Block2;
            RawBlock3 = value.RawBlock3;
            Block3 = value.Block3;
        }

        #endregion

        #region Public Properties

        public int status { get; set; }
        public byte[] RawSerialNo { get; set; }
        public string SerialNo { get; set; }
        public byte[] RawBlock0 { get; set; }
        public string Block0 { get; set; }
        public byte[] RawBlock1 { get; set; }
        public string Block1 { get; set; }
        public byte[] RawBlock2 { get; set; }
        public string Block2 { get; set; }
        public byte[] RawBlock3 { get; set; }
        public string Block3 { get; set; }

        #endregion
    }

    #endregion

    #region SmartcardService

    /// <summary>
    /// The Smartcard Service class.
    /// </summary>
    public class SmartcardService : IDisposable
    {
        #region Singelton

        private static SmartcardService _instance = null;
        /// <summary>
        /// Singelton Access.
        /// </summary>
        public static SmartcardService Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (typeof(SmartcardService))
                    {
                        _instance = new SmartcardService();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private bool onScanning = false;

        private SL600SDKFactory factory = null;
        private SL600SDK sdk = null;

        private Sl600SmartCardReader reader = null;
        private DispatcherTimer timer = null;

        public bool ReadSerialNoOnly = false;
        private Sl600SmartCardReader.ReadCardSerialResult _lastSN = null;
        private Sl600SmartCardReader.ReadCardBlockResult _lastData = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor (private)
        /// </summary>
        private SmartcardService()
        {
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            // Windows Forms
            System.Windows.Forms.Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            System.Windows.Forms.Application.ThreadExit += new EventHandler(Application_ThreadExit);
            // WPF
            if (null != System.Windows.Application.Current)
            {
                System.Windows.Application.Current.Exit += new System.Windows.ExitEventHandler(Current_Exit);
                System.Windows.Application.Current.SessionEnding += new System.Windows.SessionEndingCancelEventHandler(Current_SessionEnding);
            }
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~SmartcardService()
        {
            Dispose(true);
        }

        #endregion

        #region AppDomin/Application(Windows Forms/WPF) Event Handlers

        private void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.DomainUnload -= CurrentDomain_DomainUnload;
            Dispose(true);
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_ProcessExit;
            Dispose(true);
        }

        void Application_ThreadExit(object sender, EventArgs e)
        {
            // this event raise before application exit
            System.Windows.Forms.Application.ThreadExit -= new EventHandler(Application_ThreadExit);
            Dispose(true);
        }

        void Application_ApplicationExit(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.ApplicationExit -= new EventHandler(Application_ApplicationExit);
            Dispose(true);
        }

        private void Current_Exit(object sender, System.Windows.ExitEventArgs e)
        {
            // this event raise before application exit
            if (null != System.Windows.Application.Current)
            {
                System.Windows.Application.Current.Exit -= new System.Windows.ExitEventHandler(Current_Exit);
            }
            Dispose(true);
        }

        private void Current_SessionEnding(object sender, System.Windows.SessionEndingCancelEventArgs e)
        {
            // this event raise when window is logoff or shutdown
            if (null != System.Windows.Application.Current)
            {
                System.Windows.Application.Current.SessionEnding -= new System.Windows.SessionEndingCancelEventHandler(Current_SessionEnding);
            }
            Dispose(true);
        }

        #endregion

        #region Private Methods

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (onScanning) return;

            onScanning = true;

            if (null != reader)
            {
                if (reader.IsCardExist())
                {
                    //SL600SDK.DefaultKey
                    var snResult = reader.ReadCardSerial(/*true*/);
                    if (null != snResult && snResult.status == 0)
                    {
                        if (null == _lastSN ||
                            _lastSN.SerialNo != snResult.SerialNo)
                        {
                            if (null != OnCardReadSerial)
                            {
                                OnCardReadSerial.Invoke(null, new M1CardReadSerialEventArgs(snResult));
                            }
                            _lastSN = snResult;

                            if (!ReadSerialNoOnly)
                            {
                                var dataResult = reader.ReadCardBlock(SecureKey/*, true, true*/);
                                if (null == _lastData ||
                                    (null != _lastData &&
                                    _lastData.Block0 != dataResult.Block0 &&
                                    _lastData.Block1 != dataResult.Block1 &&
                                    _lastData.Block2 != dataResult.Block2 &&
                                    _lastData.Block3 != dataResult.Block3))
                                {
                                    // Raise event if last read value is not same card.
                                    if (null != OnCardReadBlock)
                                    {
                                        OnCardReadBlock.Invoke(null, new M1CardReadBlockEventArgs(dataResult));
                                    }
                                    _lastData = dataResult;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // reset last read card.
                    if (null != _lastSN || null != _lastData)
                    {
                        if (null != OnIdle)
                        {
                            OnIdle.Invoke(null, EventArgs.Empty);
                        }
                        _lastSN = null;
                        _lastData = null;
                    }
                }
            }

            onScanning = false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Dispose (Free all resources).
        /// </summary>
        /// <param name="disposed">True to free all resources.</param>
        public void Dispose(bool disposed)
        {
            Shutdown();
            if (disposed)
            {
                if (null != sdk)
                {
                    sdk.Dispose();
                }
                sdk = null;
                if (null != factory)
                {
                    // Release.
                    factory.Release();
                }
                factory = null;
            }
        }
        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(false);
        }
        /// <summary>
        /// Start listen USB port.
        /// </summary>
        public void Start()
        {
            //var resolver = CreateResolver();
            if (null == factory)
            {
                factory = SL600SDKFactory.CreateFactory(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MasterRD.dll"));
            }
            if (null == sdk)
            {
                sdk = factory.CreateInstance();
            }
            if (null == reader)
            {
                reader = new Sl600SmartCardReader(sdk, 0) { IsEmv = false };
            }

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(150);
            timer.Start();
        }
        /// <summary>
        /// Shutdown and free resources.
        /// </summary>
        /// <param name="disposed">True for force dispose.</param>
        public void Shutdown(bool disposed = false)
        {
            if (null != timer)
            {
                timer.Stop();
                timer.Tick -= Timer_Tick;
            }
            timer = null;
            if (null != reader)
            {
                reader.Dispose();
            }
            reader = null;
            if (disposed) Dispose(true);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Secure Key.
        /// </summary>
        public byte[] SecureKey { get; set; }
        /// <summary>
        /// Checks is port connected or not.
        /// </summary>
        public bool Connected
        {
            get 
            {
                return (null != reader) ? reader.Connected : false;
            }
        }
        /// <summary>
        /// Checks is port connected or not.
        /// </summary>
        public string ConnectStatus
        {
            get
            {
                return (null != reader) ? reader.ConnectStatus : "Reader is null.";
            }
        }

        #endregion

        #region Public events

        /// <summary>
        /// OnCardReadSerial Event Handler.
        /// </summary>
        public event M1CardReadSerialEventHandler OnCardReadSerial;
        /// <summary>
        /// OnCardReadBlock Event Handler.
        /// </summary>
        public event M1CardReadBlockEventHandler OnCardReadBlock;
        /// <summary>
        /// OnIdle Event Handler
        /// </summary>
        public event EventHandler OnIdle;

        #endregion

        #region Public Statis Methods

        /// <summary>
        /// Free all resources.
        /// </summary>
        public static void Release()
        {
            if (null != _instance)
            {
                _instance.Dispose(true);
            }
            _instance = null;
        }

        #endregion
    }

    #endregion
}
