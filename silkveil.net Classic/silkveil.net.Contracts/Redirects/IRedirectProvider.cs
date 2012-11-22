using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Users;

using System;
using System.Collections.Generic;

namespace silkveil.net.Contracts.Redirects
{
    /// <summary>
    /// Contains the method for redirect providers.
    /// </summary>
    public interface IRedirectProvider : IProvider
    {
        /// <summary>
        /// Creates the specified redirect.
        /// </summary>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        IRedirect CreateRedirect(IRedirect redirect);

        /// <summary>
        /// Deletes the specified redirect.
        /// </summary>
        /// <param name="redirect">The redirect.</param>
        void DeleteRedirect(IRedirect redirect);

        /// <summary>
        /// Updates the specified redirect.
        /// </summary>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        IRedirect UpdateRedirect(IRedirect redirect);

        /// <summary>
        /// Reads all redirects.
        /// </summary>
        /// <returns>The redirects.</returns>
        IEnumerable<IRedirect> ReadRedirects();

        /// <summary>
        /// Reads all redirects for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The redirects.</returns>
        IEnumerable<IRedirect> ReadRedirects(IUser user);

        /// <summary>
        /// Reads the redirect for the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The redirect.</returns>
        IRedirect ReadRedirect(Guid id);

        /// <summary>
        /// Reads the redirect for the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The redirect.</returns>
        IRedirect ReadRedirectByName(string name);
    }
}