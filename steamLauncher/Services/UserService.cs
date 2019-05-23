namespace SteamLauncher.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SteamLauncher.AppConfiguration;
    using SteamLauncher.Helpers;

    /// <summary>
    /// The configuration service.
    /// </summary>
    public class UserService
    {
        private readonly ConfigurationService configService;

        private ConfigData data;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="configService">
        /// The config service.
        /// </param>
        public UserService(ConfigurationService configService)
        {
            this.configService = configService;
            this.data = configService.ReadConfiguration();
        }

        /// <summary>
        /// The add user.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="UserData"/>.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        public UserData AddUser(UserData user)
        {
            if (string.IsNullOrEmpty(user.User))
            {
                throw new ApplicationException("User name cannot be empty");
            }

            if (string.IsNullOrEmpty(user.Secret))
            {
                throw new ApplicationException("User's secret cannot be empty.");
            }

            user.Id = this.GetNextId();
            user.CreatedOn = DateTime.Now;
            user.Secret = EncryptionHelper.Encrypt(this.configService.Key, user.Secret);
            this.data.Accounts.Add(user);

            this.SaveChanges();

            return user;
        }

        /// <summary>
        /// The delete user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <exception cref="ApplicationException">
        /// </exception>
        public void DeleteUser(int id)
        {
            var current = this.GetUser(id);
            if (current == null)
            {
                throw new ApplicationException("The user with given id doesn't exists: " + id);
            }

            this.DeleteUser(current);
        }

        /// <summary>
        /// The delete user.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <exception cref="ApplicationException">
        /// </exception>
        public void DeleteUser(string name)
        {
            var current = this.GetUser(name);
            if (current == null)
            {
                throw new ApplicationException("The user with given name doesn't exists: " + name);
            }

            this.DeleteUser(current);
        }

        /// <summary>
        /// The delete user.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <exception cref="ApplicationException">
        /// </exception>
        public void DeleteUser(UserData user)
        {
            if (this.data.Accounts.Contains(user))
            {
                this.data.Accounts.Remove(user);
                this.SaveChanges();
            }
            else
            {
                throw new ApplicationException("Provided user object doesn't exist.");
            }

        }

        /// <summary>
        /// The get u ser.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="UserData"/>.
        /// </returns>
        public UserData GetUser(string name)
        {
            return this.data.Accounts.FirstOrDefault(item => item.User.Equals(name, StringComparison.InvariantCulture));
        }

        /// <summary>
        /// The get user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="UserData"/>.
        /// </returns>
        public UserData GetUser(int id)
        {
            return this.data.Accounts.FirstOrDefault(item => item.Id == id);
        }

        /// <summary>
        /// The get all users.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<UserData> GetAllUsers()
        {
            return this.data.Accounts.ToList();
        }

        /// <summary>
        /// The save changes.
        /// </summary>
        private void SaveChanges()
        {
            this.configService.SaveConfiguration(this.data);
        }

        /// <summary>
        /// The get next id.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int GetNextId()
        {
            var result = 1;
            while (this.data.Accounts.Any(item => item.Id == result))
            {
                result++;
            }

            return result;
        }
    }
}