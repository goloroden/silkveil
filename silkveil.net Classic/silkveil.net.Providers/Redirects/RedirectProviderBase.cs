using LightCore;

using silkveil.net.Contracts;
using silkveil.net.Contracts.Redirects;
using silkveil.net.Contracts.Users;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace silkveil.net.Providers.Redirects
{
    /// <summary>
    /// Represents the abstract base class for redirect providers.
    /// </summary>
    /// <typeparam name="TRedirectDataContext">The type of the redirect data context.</typeparam>
    public abstract class RedirectProviderBase<TRedirectDataContext> : ProviderBase, IRedirectProvider
    {
        /// <summary>
        /// Contains the reader writer lock.
        /// </summary>
        private readonly ReaderWriterLockSlim _readerWriterLockSlim =
            new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectProviderBase{TRedirectDataContext}" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        protected RedirectProviderBase(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Creates sample data.
        /// </summary>
        protected override void CreateSampleData()
        {
            var redirect = this.Container.Resolve<IRedirect>();
            redirect.Id = new Guid("{47978548-59f5-4f81-b57f-af99802a3199}");
            redirect.Name = "silkveil";
            redirect.Uri = new Uri("http://www.silkveil.net/");

            this.CreateRedirect(redirect);
        }

        /// <summary>
        /// Gets the redirect data context.
        /// </summary>
        /// <returns>The redirect data context.</returns>
        protected abstract TRedirectDataContext GetRedirectDataContext();

        /// <summary>
        /// Saves the redirect data context.
        /// </summary>
        /// <returns>The redirect data context.</returns>
        protected abstract void SaveRedirectDataContext(TRedirectDataContext redirectDataContext);

        /// <summary>
        /// Disposes the redirect data context.
        /// </summary>
        /// <returns>The redirect data context.</returns>
        protected abstract void DisposeRedirectDataContext(TRedirectDataContext redirectDataContext);

        /// <summary>
        /// Reads all redirects.
        /// </summary>
        /// <returns>The redirects.</returns>
        public IEnumerable<IRedirect> ReadRedirects()
        {
            this._readerWriterLockSlim.EnterReadLock();

            try
            {
                // Get the redirect data context.
                TRedirectDataContext redirectDataContext = this.GetRedirectDataContext();

                // Read all redirects.
                var redirects = ReadRedirects(redirectDataContext);

                // Dispose the redirect data context.
                this.DisposeRedirectDataContext(redirectDataContext);

                // Return the redirects to the caller.
                return redirects;
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Reads all redirects.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <returns>The redirects.</returns>
        protected abstract IEnumerable<IRedirect> ReadRedirects(TRedirectDataContext redirectDataContext);

        /// <summary>
        /// Reads all redirects for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The redirects.</returns>
        public IEnumerable<IRedirect> ReadRedirects(IUser user)
        {
            this._readerWriterLockSlim.EnterReadLock();

            try
            {
                // Get the redirect data context.
                TRedirectDataContext redirectDataContext = this.GetRedirectDataContext();

                // Read all redirects.
                var redirects = ReadRedirects(redirectDataContext, user);

                // Dispose the redirect data context.
                this.DisposeRedirectDataContext(redirectDataContext);

                // Return the redirects to the caller.
                return redirects;
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Reads all redirects for the specified user.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="user">The user.</param>
        /// <returns>The redirects.</returns>
        protected abstract IEnumerable<IRedirect> ReadRedirects(TRedirectDataContext redirectDataContext, IUser user);

        /// <summary>
        /// Reads the redirect for the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The redirect.</returns>
        public IRedirect ReadRedirect(Guid id)
        {
            this._readerWriterLockSlim.EnterReadLock();

            try
            {
                // Get the redirect data context.
                TRedirectDataContext redirectDataContext = this.GetRedirectDataContext();

                // Read the redirect.
                var redirect = ReadRedirect(redirectDataContext, id);

                // Dispose the redirect data context.
                this.DisposeRedirectDataContext(redirectDataContext);

                // Return the redirect to the caller.
                return redirect;
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Reads the redirect for the specified ID.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="id">The ID.</param>
        /// <returns>The redirect.</returns>
        protected abstract IRedirect ReadRedirect(TRedirectDataContext redirectDataContext, Guid id);

        /// <summary>
        /// Reads the redirect for the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The redirect.</returns>
        public IRedirect ReadRedirectByName(string name)
        {
            this._readerWriterLockSlim.EnterReadLock();

            try
            {
                // Get the redirect data context.
                TRedirectDataContext redirectDataContext = this.GetRedirectDataContext();

                // Read the redirect.
                var redirect = ReadRedirectByName(redirectDataContext, name);

                // Dispose the redirect data context.
                this.DisposeRedirectDataContext(redirectDataContext);

                // Return the redirect to the caller.
                return redirect;
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Reads the redirect for the specified name.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="name">The name.</param>
        /// <returns>The redirect.</returns>
        protected abstract IRedirect ReadRedirectByName(TRedirectDataContext redirectDataContext, string name);

        /// <summary>
        /// Creates the specified redirect.
        /// </summary>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        public IRedirect CreateRedirect(IRedirect redirect)
        {
            this._readerWriterLockSlim.EnterWriteLock();

            try
            {
                // Get the redirect data context.
                TRedirectDataContext redirectDataContext = this.GetRedirectDataContext();

                try
                {
                    // Try to load the redirect. If this fails, the redirect can be created.
                    this.ReadRedirect(redirectDataContext, redirect.Id);
                }
                catch (Exception)
                {
                    // Create the redirect.
                    var createdRedirect = this.CreateRedirect(redirectDataContext, redirect);

                    // Save the redirect data context.
                    this.SaveRedirectDataContext(redirectDataContext);

                    // Return the created redirect to the caller.
                    return createdRedirect;
                }
                finally
                {
                    // Dispose the redirect data context.
                    this.DisposeRedirectDataContext(redirectDataContext);
                }

                throw new UniqueConstraintViolationException(String.Format(CultureInfo.CurrentUICulture,
                    "The redirect '{0}' already exists.", redirect.Name));
            }
            finally
            {
                this._readerWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Creates the specified redirect.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        protected abstract IRedirect CreateRedirect(TRedirectDataContext redirectDataContext, IRedirect redirect);

        /// <summary>
        /// Deletes the specified redirect.
        /// </summary>
        /// <param name="redirect">The redirect.</param>
        public void DeleteRedirect(IRedirect redirect)
        {
            this._readerWriterLockSlim.EnterWriteLock();

            try
            {
                // Get the redirect data context.
                TRedirectDataContext redirectDataContext = this.GetRedirectDataContext();

                // Delete the redirect.
                this.DeleteRedirect(redirectDataContext, redirect);

                // Save the redirect data context.
                this.SaveRedirectDataContext(redirectDataContext);

                // Dispose the redirect data context.
                this.DisposeRedirectDataContext(redirectDataContext);
            }
            finally
            {
                this._readerWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Deletes the specified redirect.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="redirect">The redirect.</param>
        protected abstract void DeleteRedirect(TRedirectDataContext redirectDataContext, IRedirect redirect);

        /// <summary>
        /// Updates the specified redirect.
        /// </summary>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        public IRedirect UpdateRedirect(IRedirect redirect)
        {
            this._readerWriterLockSlim.EnterWriteLock();

            try
            {
                // Get the redirect data context.
                TRedirectDataContext redirectDataContext = this.GetRedirectDataContext();

                // Try to load the redirect to ensure that it exists.
                this.ReadRedirect(redirectDataContext, redirect.Id);

                // Update the redirect.
                var updatedRedirect = this.UpdateRedirect(redirectDataContext, redirect);

                // Save the redirect data context.
                this.SaveRedirectDataContext(redirectDataContext);

                // Dispose the redirect data context.
                this.DisposeRedirectDataContext(redirectDataContext);

                // Return the updated redirect to the caller.
                return updatedRedirect;
            }
            finally
            {
                this._readerWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Updates the specified redirect.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        protected abstract IRedirect UpdateRedirect(TRedirectDataContext redirectDataContext, IRedirect redirect);
    }
}