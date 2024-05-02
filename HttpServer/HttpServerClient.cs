using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantframeLib.HttpServer
{
    public class HttpServerClient
    {
        #region Const/Static Values
        #endregion
        #region Private Values

        #endregion
        #region New
        public static void Initializer()
        {
            StartServer();
        }
        #endregion
        #region Method

        #endregion
        #region Loops
        private static void StartServer()
        {
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        HttpListener listener = new HttpListener();
                        listener.Prefixes.Add("http://localhost:8080/");
                        listener.Start();
                        while (true)
                        {


                            //HttpListenerContext context = listener.GetContext();
                            //HttpListenerRequest request = context.Request;
                            //HttpListenerResponse response = context.Response;
                            //string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                            //byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                            //response.ContentLength64 = buffer.Length;
                            //System.IO.Stream output = response.OutputStream;
                            //output.Write(buffer, 0, buffer.Length);
                            //output.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        Thread.Sleep(10000);
                    }
                }

            })
            {
                IsBackground = true
            }.Start();

        }
        #endregion
        #region Override Method
        /// <summary>
        /// Override ToString for <see cref="HttpServerClient"/>
        /// </summary>
        /// <returns>
        /// Returns store data in string.
        /// </returns>
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
        #region Method Get Set

        #endregion
        #region Events

        #endregion
    }
}
