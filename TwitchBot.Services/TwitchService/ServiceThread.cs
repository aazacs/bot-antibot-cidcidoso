using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TwitchBot.Services.TwitchService
{
    /// <summary>
    /// Class responsible for handling channel threads.
    /// </summary>
    public class ServiceThread : IDisposable
    {
        public Thread Thread { get; set; }
        public string Channel { get; private set; }
        internal TcpClient TCPClient { get; private set; }
        internal StreamReader Reader { get; private set; }
        internal StreamWriter Writer { get; private set; }

        /// <summary>
        /// Initializes a new instance connecting to Twitch IRC.
        /// </summary>
        /// <param name="host">Twitch host.</param>
        /// <param name="port">Twitch port.</param>
        /// <param name="channel">Channel name to connect.</param>
        public ServiceThread(string host, int port, string channel)
        {
            this.TCPClient = new TcpClient(host, port);
            this.Reader = new StreamReader(TCPClient.GetStream());
            this.Writer = new StreamWriter(TCPClient.GetStream())
            {
                AutoFlush = true
            };
            this.Channel = channel;
        }

        /// <summary>
        /// Causes the operating system to change the state of the current instance to <see cref="ThreadState.Running"/>.
        /// </summary>
        /// <exception cref="ThreadStateException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        public void Start() => this.Thread.Start();
        /// <summary>
        /// Raises a <see cref="ThreadAbortException"/> in the thread on which it is invoked, to begin the process of terminating the thread. Calling this method usually terminates the thread.
        /// </summary>
        /// <exception cref="ThreadStateException"></exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        public void Abort()
        {
            this.Thread.Abort();
            this.TCPClient.Close();
            this.Dispose();
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: descartar estado gerenciado (objetos gerenciados).
                }

                // TODO: liberar recursos não gerenciados (objetos não gerenciados) e substituir um finalizador abaixo.
                // TODO: definir campos grandes como nulos.

                disposedValue = true;
            }
        }

        // TODO: substituir um finalizador somente se Dispose(bool disposing) acima tiver o código para liberar recursos não gerenciados.
        // ~ServiceThread()
        // {
        //   // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
        //   Dispose(false);
        // }

        // Código adicionado para implementar corretamente o padrão descartável.
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
            Dispose(true);
            // TODO: remover marca de comentário da linha a seguir se o finalizador for substituído acima.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
