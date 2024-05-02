using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTP
{
    #region Delegates

    /// <summary>
    /// Delegate which runs before all route-methods and returns if the processing should finish (true) or continue (false).
    /// </summary>
    /// <param name="request">HTTP request.</param>
    /// <param name="response">HTTP response.</param>
    /// <returns>True if the request is handled, false otherwise.</returns>
    public delegate bool OnBefore(HttpListenerRequest request, HttpListenerResponse response);

    /// <summary>
    /// Delegate which runs before any route-action is invoked to determine which route should be executed.
    /// </summary>
    /// <param name="request">HTTP request.</param>
    /// <param name="arguments">
    /// Empty collection of key-value pairs populated by this function. 
    /// <para>If <see cref="OnBefore"/> is run it may contain some data.</para>
    /// </param>
    /// <returns>True if the route action should be executed, false otherwise.</returns>
    public delegate bool ShouldProcessFunc(HttpListenerRequest request, Dictionary<string, string> arguments);

    /// <summary>
    /// Delegate which runs when a route is matched.
    /// </summary>
    /// <param name="request">HTTP request.</param>
    /// <param name="response">HTTP response.</param>
    /// <param name="arguments">Collection of key-value pairs populated by the <see cref="ShouldProcessFunc"/>.</param>
    /// <returns>Action task.</returns>
    public delegate Task HttpActionAsync(HttpListenerRequest request, HttpListenerResponse response, Dictionary<string, string> arguments);
    /// <summary>
    /// Delegate which runs when a route is matched.
    /// </summary>
    /// <param name="request">HTTP request.</param>
    /// <param name="response">HTTP response.</param>
    /// <param name="arguments">Collection of key-value pairs populated by the <see cref="ShouldProcessFunc"/>.</param>
    public delegate void HttpAction(HttpListenerRequest request, HttpListenerResponse response, Dictionary<string, string> arguments);

    /// <summary>
    /// Delegate which runs if an error occurs.
    /// </summary>
    /// <param name="request">HTTP request.</param>
    /// <param name="response">HTTP response.</param>
    /// <param name="exception">Thrown exception.</param>
    public delegate void OnError(HttpListenerRequest request, HttpListenerResponse response, Exception exception);

    #endregion
    #region Exceptions

    /// <summary>
    /// Represents error that occur when a route is not found.
    /// </summary>
    public class RouteNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new instance of the route not found exception.
        /// </summary>
        /// <param name="route"></param>
        public RouteNotFoundException(string route)
            : base($"Route {route} not found.")
        { }
    }

    #endregion
    public class Routes
    {
        #region Const/Static Values
        #endregion
        #region Private Values
        private OnBefore _onBefore;
        private OnError _onError;
        private readonly List<(ShouldProcessFunc, HttpActionAsync)> _routes;
        #endregion
        #region New
        /// <summary>
        /// Creates a new instance of the route.
        /// </summary>
        public Routes()
        {
            _routes = new List<(ShouldProcessFunc, HttpActionAsync)>();
            _onError = (req, res, ex) =>
            {
                if (res.StatusCode >= 200 && res.StatusCode <= 299)
                    res.StatusCode = (int)HttpStatusCode.BadRequest;

                res.AsText(ex.Message, "text/plain");
            };
        }
        #endregion
        #region Method

        /// <summary>
        /// Handles the request.
        /// </summary>
        public async Task HandleRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            //run the 'before' method
            try
            {
                var isHandled = OnBefore?.Invoke(request, response);
                if (isHandled.HasValue && (bool)isHandled) return;
            }
            catch (Exception ex)
            {
                try { OnError?.Invoke(request, response, ex); }
                catch { }
            }

            //select and run an action
            var args = new Dictionary<string, string>();
            foreach (var (shouldProcessFunc, action) in _routes)
            {
                if (shouldProcessFunc(request, args) == false)
                {
                    args.Clear(); //if something was written
                    continue;
                }

                try
                {
                    await action(request, response, args);
                }
                catch (Exception ex)
                {
                    try { OnError?.Invoke(request, response, ex); }
                    catch { }
                }

                return;
            }

            //handle if no route is selected
            try
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                OnError?.Invoke(request, response, new RouteNotFoundException(request.Url.PathAndQuery));
            }
            catch { }
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
        /// <summary>
        /// Gets or sets OnBefore.
        /// </summary>
        /// <value> 
        /// The OnBefore.
        /// </value>
        public OnBefore OnBefore
        {
            get { return _onBefore; }
            set { _onBefore = value; }
        }
        /// <summary>
        /// Gets or sets OnError.
        /// </summary>
        /// <value>
        /// The OnError.
        public OnError OnError
        {
            get { return _onError; }
            set { _onError = value; }
        }
        #endregion
        #region Add (pattern)

        /// <summary>
        /// Adds the specified action to the route collection.
        /// <para>The order of actions defines the priority.</para>
        /// </summary>
        /// <param name="pattern">
        /// String pattern optionally containing named arguments in {}. 
        /// <para>
        /// Example: "/page-{pageNumber}/". 'pageNumber' will be parsed and added to 'arguments' key-value pair collection.
        /// The last argument is parsed as greedy one.
        /// </para>
        /// </param>
        /// <param name="action">Action executed if the specified pattern matches the URL path.</param>
        /// <param name="method">HTTP method (GET, POST, DELETE, HEAD).</param>
        public void Add(string pattern, HttpAction action, string method = "GET")
        {
            Add((rq, args) =>
                {
                    if (rq.HttpMethod != method)
                        return false;

                    return rq.Url.PathAndQuery.TryMatch(pattern, args);
                },
                action);
        }

        /// <summary>
        /// Adds the specified action to the route collection.
        /// <para>The order of actions defines the priority.</para>
        /// </summary>
        /// <param name="pattern">
        /// String pattern optionally containing named arguments in {}. 
        /// <para>
        /// Example: "/page-{pageNumber}/". 'pageNumber' will be parsed and added to 'arguments' key-value pair collection.
        /// The last argument is parsed as greedy one.
        /// </para>
        /// </param>
        /// <param name="action">Action executed if the specified pattern matches the URL path.</param>
        /// <param name="method">HTTP method (GET, POST, DELETE, HEAD).</param>
        public void Add(string pattern, HttpActionAsync action, string method = "GET")
        {
            Add((rq, args) =>
                {
                    if (rq.HttpMethod != method)
                        return false;

                    return rq.Url.PathAndQuery.TryMatch(pattern, args);
                },
                action);
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds the specified action to the route collection.
        /// <para>The order of actions defines the priority.</para>
        /// </summary>
        /// <param name="shouldProcess">Function defining whether the specified action should be executed or not.</param>
        /// <param name="action">Action executed if the specified pattern matches the URL path.</param>
        public void Add(ShouldProcessFunc shouldProcess, HttpActionAsync action)
        {
            _routes.Add((shouldProcess, action));
        }

        /// <summary>
        /// Adds the specified action to the route collection.
        /// <para>The order of actions defines the priority.</para>
        /// </summary>
        /// <param name="shouldProcess">Function defining whether the specified action should be executed or not.</param>
        /// <param name="action">Action executed if the specified pattern matches the URL path.</param>
        public void Add(ShouldProcessFunc shouldProcess, HttpAction action)
        {
            _routes.Add((shouldProcess, (rq, rp, args) =>
            {
                action(rq, rp, args);
                return Task.FromResult(true);
            }
            ));
        }

        #endregion

    }
}
