using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace GrovePiSigfox
{
    internal class RPISigfox// : IDisposable
    {
        private SerialDevice serialPort = null;
        ~RPISigfox()
        {
            Dispose(false);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
                Close();
        }
        public void Dispose()
        {
            Dispose(true);
        }
        public async Task<bool> Open()
        {
            var devices = await DeviceInformation.FindAllAsync(SerialDevice.GetDeviceSelector());
            if (devices.Count > 0)
            {
                serialPort = await SerialDevice.FromIdAsync(devices[0].Id);
                if (serialPort != null)
                {
                    serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                    serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                    serialPort.BaudRate = 9600;
                    serialPort.Parity = SerialParity.None;
                    serialPort.StopBits = SerialStopBitCount.One;
                    serialPort.DataBits = 8;
                    return await SendAsync("AT", "AT");
                }
            }
            return false;
        }
        public void Close()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
                serialPort = null;
            }
        }
        public async Task<bool> SendAsync(byte[] data)
        {
            if (data.Length > 12)
                return false;
            var sb = new StringBuilder();
            foreach (var b in data)
                sb.AppendFormat("{0:X2}", b);
            return await SendAsync(sb.ToString());
        }
        private async Task<bool> SendAsync(string data)
        {
            return await SendAsync(string.Format("AT$SS={0}\n", data), "OK\r\n", "\r\nE");
        }
        private async Task<bool> SendAsync(string data, string ok = "", string err = "")
        {
            if (serialPort == null)
            {
                if (!await Open())
                    return false;
            }
            try
            {
                using (var writer = new DataWriter(serialPort.OutputStream))
                {
                    writer.WriteString(data);
                    await writer.StoreAsync();
                    writer.DetachStream();
                }
            }
            catch (OperationCanceledException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            try
            {
                var buffer = string.Empty;
                using (var reader = new DataReader(serialPort.InputStream))
                {
                    reader.InputStreamOptions = InputStreamOptions.ReadAhead;
                    if (await reader.LoadAsync(1024) > 0)
                        buffer = reader.ReadString(reader.UnconsumedBufferLength);
                    reader.DetachStream();
                }
                if (!string.IsNullOrEmpty(ok) && buffer.Contains(ok))
                {
                    return true;
                }
                else if (!string.IsNullOrEmpty(err) && buffer.Contains(err))
                {
                    return false;
                }
            }
            catch (OperationCanceledException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
    }
}
