using LightCore;

using Microsoft.IdentityModel.Claims;

using silkveil.net.Contracts.Security;
using silkveil.net.Contracts.Users;

using System.Collections.Generic;
using System.Security;
using System.Security.Principal;
using System.Threading;

namespace silkveil.net.Providers.Users
{
    /// <summary>
    /// Represents the abstract base class for user providers.
    /// </summary>
    /// <typeparam name="TUserDataContext">The type of the user data context.</typeparam>
    public abstract class UserProviderBase<TUserDataContext> : ProviderBase, IUserProvider
    {
        /// <summary>
        /// Contains the reader writer lock.
        /// </summary>
        private readonly ReaderWriterLockSlim _readerWriterLockSlim =
            new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProviderBase{TUserDataContext}" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        protected UserProviderBase(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Creates sample data.
        /// </summary>
        protected override void CreateSampleData()
        {
        }

        /// <summary>
        /// Gets the user data context.
        /// </summary>
        /// <returns>The user data context.</returns>
        protected abstract TUserDataContext GetUserDataContext();

        /// <summary>
        /// Saves the user data context.
        /// </summary>
        /// <returns>The user data context.</returns>
        protected abstract void SaveUserDataContext(TUserDataContext userDataContext);

        /// <summary>
        /// Disposes the user data context.
        /// </summary>
        /// <returns>The user data context.</returns>
        protected abstract void DisposeUserDataContext(TUserDataContext dataContext);

        /// <summary>
        /// Logs on the user with the specified credentials.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// An instance of <see cref="IPrincipal" /> if the logon was successful. Otherwise, a
        /// <see cref="SecurityException" /> is thrown.
        /// </returns>
        public IPrincipal Logon(string login, string password)
        {
            this._readerWriterLockSlim.EnterReadLock();

            try
            {
                // Get the user data context.
                TUserDataContext userDataContext = this.GetUserDataContext();

                // Logon the user.
                var principal = Logon(userDataContext, login, password);

                // Dispose the user data context.
                this.DisposeUserDataContext(userDataContext);

                // Return the principal to the caller.
                return principal;
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Logs on the user with the specified credentials.
        /// </summary>
        /// <param name="userDataContext">The user data context.</param>
        /// <param name="login">The logon.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// An instance of <see cref="IPrincipal" /> if the login was successful. Otherwise, a
        /// <see cref="SecurityException" /> is thrown.
        /// </returns>
        /// <remarks>The returned principal only contains the name of the user.</remarks>
        protected abstract IPrincipal Logon(TUserDataContext userDataContext, string login, string password);

        /// <summary>
        /// Reads the user's security ID by the login.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>The security ID.</returns>
        public ISecurityId ReadSecurityIdByLogin(string login)
        {
            this._readerWriterLockSlim.EnterReadLock();

            try
            {
                // Get the user data context.
                TUserDataContext userDataContext = this.GetUserDataContext();

                // Read the security ID by the login.
                var securityId = ReadSecurityIdByLogin(userDataContext, login);

                // Dispose the user data context.
                this.DisposeUserDataContext(userDataContext);

                // Return the security ID to the caller.
                return securityId;
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Reads the user's security ID by the login.
        /// </summary>
        /// <param name="userDataContext">The user data context.</param>
        /// <param name="login">The login.</param>
        /// <returns>The security ID.</returns>
        protected abstract ISecurityId ReadSecurityIdByLogin(TUserDataContext userDataContext, string login);

        /// <summary>
        /// Reads the claims for the user with the specified security ID.
        /// </summary>
        /// <param name="securityId">The security ID.</param>
        /// <returns>A list of claims.</returns>
        public IEnumerable<Claim> ReadClaimsBySecurityId(ISecurityId securityId)
        {
            this._readerWriterLockSlim.EnterReadLock();

            try
            {
                // Get the user data context.
                TUserDataContext userDataContext = this.GetUserDataContext();

                // Read the claims by the security ID.
                var claims = ReadClaimsBySecurityId(userDataContext, securityId);

                // Dispose the user data context.
                this.DisposeUserDataContext(userDataContext);

                // Return the claims to the caller.
                return claims;
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Reads the claims for the user with the specified security ID.
        /// </summary>
        /// <param name="userDataContext">The user data context.</param>
        /// <param name="securityId">The security ID.</param>
        /// <returns>A list of claims.</returns>
        protected abstract IEnumerable<Claim> ReadClaimsBySecurityId(TUserDataContext userDataContext, ISecurityId securityId);
    }
}