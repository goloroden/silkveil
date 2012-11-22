using silkveil.net.Contracts.Redirects;
using silkveil.net.Contracts.Services;
using silkveil.net.Contracts.Users;

using System;
using System.Collections.Generic;
using System.Transactions;

namespace silkveil.net.Services
{
    /// <summary>
    /// Represents the redirect service.
    /// </summary>
    public class RedirectService : IRedirectService
    {
        /// <summary>
        /// Contains the redirect provider.
        /// </summary>
        private IRedirectProvider _redirectProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectService"/> type.
        /// </summary>
        /// <param name="redirectProvider">The redirect provider.</param>
        public RedirectService(IRedirectProvider redirectProvider)
        {
            this._redirectProvider = redirectProvider;
        }

        /// <summary>
        /// Creates the specified redirect.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        public IRedirect CreateRedirect(IUser user, IRedirect redirect)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                // Create the redirect.
                IRedirect returnRedirect = this._redirectProvider.CreateRedirect(redirect);

                // Commit the transaction.
                transactionScope.Complete();

                // Return the redirect to the caller.
                return returnRedirect;
            }
        }

        /// <summary>
        /// Reads the redirect with the specified id.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="id">The id of the redirect</param>
        /// <returns>The redirect.</returns>
        public IRedirect ReadRedirect(IUser user, Guid id)
        {
            // Read the redirect.
            IRedirect redirect = this._redirectProvider.ReadRedirect(id);

            // Return the redirect to the caller.
            return redirect;
        }

        /// <summary>
        /// Reads the redirect with the specified name.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="name">The name of the redirect</param>
        /// <returns>The redirect.</returns>
        public IRedirect ReadRedirectByName(IUser user, string name)
        {
            // Read the redirect.
            IRedirect redirect = this._redirectProvider.ReadRedirectByName(name);

            // Return the redirect to the caller.
            return redirect;
        }

        /// <summary>
        /// Reads all redirects.
        /// </summary>
        /// <param name="user">The user.</param>
        public IEnumerable<IRedirect> ReadRedirects(IUser user)
        {
            // Read all redirects.
            foreach (IRedirect redirect in this._redirectProvider.ReadRedirects())
            {
                // Yield return the redirect.
                yield return redirect;
            }
        }

        /// <summary>
        /// Updates the specified redirect.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        public IRedirect UpdateRedirect(IUser user, IRedirect redirect)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                // Update the redirect.
                IRedirect returnRedirect = this._redirectProvider.UpdateRedirect(redirect);

                // Commit the transaction scope.
                transactionScope.Complete();

                // Return the redirect to the caller.
                return returnRedirect;
            }
        }

        /// <summary>
        /// Deletes the specified redirect.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="redirect">The redirect.</param>
        public void DeleteRedirect(IUser user, IRedirect redirect)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                // Delete the redirect.
                this._redirectProvider.DeleteRedirect(redirect);

                // Commit the transaction.
                transactionScope.Complete();
            }
        }
    }
}