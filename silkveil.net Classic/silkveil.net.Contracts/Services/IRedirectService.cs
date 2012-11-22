using silkveil.net.Contracts.Redirects;
using silkveil.net.Contracts.Users;

using System;
using System.Collections.Generic;

namespace silkveil.net.Contracts.Services
{
    /// <summary>
    /// Contains the methods for a redirect service.
    /// </summary>
    public interface IRedirectService : IService
    {
        /// <summary>
        /// Creates the specified redirect.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        IRedirect CreateRedirect(IUser user, IRedirect redirect);

        /// <summary>
        /// Reads the redirect with the specified id.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="id">The id of the redirect</param>
        /// <returns>The redirect.</returns>
        IRedirect ReadRedirect(IUser user, Guid id);

        /// <summary>
        /// Reads the redirect with the specified name.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="name">The name of the redirect</param>
        /// <returns>The redirect.</returns>
        IRedirect ReadRedirectByName(IUser user, string name);

        /// <summary>
        /// Reads all redirects.
        /// </summary>
        /// <param name="user">The user.</param>
        IEnumerable<IRedirect> ReadRedirects(IUser user);

        /// <summary>
        /// Updates the specified redirect.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        IRedirect UpdateRedirect(IUser user, IRedirect redirect);

        /// <summary>
        /// Deletes the specified redirect.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="redirect">The redirect.</param>
        void DeleteRedirect(IUser user, IRedirect redirect);
    }
}