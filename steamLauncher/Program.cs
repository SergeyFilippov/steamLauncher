namespace SteamLauncher
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    using CommandLine;

    using SteamLauncher.AppConfiguration;
    using SteamLauncher.CommandConfiguration;
    using SteamLauncher.Services;

    /// <summary>
    /// The program.
    /// </summary>
    class Program
    {
        private static UserService userService;

        private static ConfigurationService configService;

        private static SteamService steamService;

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        static int Main(string[] args)
        {
            try
            {
                configService = new ConfigurationService();
                userService = new UserService(configService);
                steamService = new SteamService(configService, userService);

                return Parser.Default.ParseArguments<AddParams, RunParams, ListParams, DeleteParams, ConfigParams>(args).MapResult(
                    (AddParams opts) => ExecutAddParams(opts),
                    (RunParams opts) => ExecuteRunParams(opts),
                    (ListParams opts) => ExecuteListParams(opts),
                    (DeleteParams opts) => ExecuteDeleteParams(opts),
                    (ConfigParams opts) => ExecuteConfigParams(opts),
                    errs => 1);
            }
            catch (ApplicationException exception)
            {
                Console.WriteLine(exception.Message);
                ExitWithWait(2);
            }
            catch (FileNotFoundException exception)
            {
                Console.WriteLine(exception.Message);
                ExitWithWait(4);
            }

            return 0;
        }

        /// <summary>
        /// The execute config params.
        /// </summary>
        /// <param name="configParams">
        /// The config params.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        /// </exception>
        private static int ExecuteConfigParams(ConfigParams configParams)
        {
            if (!File.Exists(configParams.SteamPath))
            {
                throw new FileNotFoundException("Provided steam path is not valid. File doesn't exist.");
            }

            var config = configService.ReadConfiguration();
            config.SteamPath = configParams.SteamPath;
            configService.SaveConfiguration(config);
            return 0;
        }

        /// <summary>
        /// The execute delete params.
        /// </summary>
        /// <param name="deleteParams">
        /// The delete params.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        private static int ExecuteDeleteParams(DeleteParams deleteParams)
        {
            if (deleteParams.Number > 0 && !string.IsNullOrEmpty(deleteParams.User))
            {
                throw new ApplicationException("Both user number and name are provided. Only one of the following fields can be used.");
            }

            if (deleteParams.Number > 0)
            {
                userService.DeleteUser(deleteParams.Number);
                return 0;
            }

            if (!string.IsNullOrEmpty(deleteParams.User))
            {
                userService.DeleteUser(deleteParams.User);
                return 0;
            }

            throw new ApplicationException("For deleting the user entry, at least valid number or a name must be provided.");
        }

        /// <summary>
        /// The execute list params.
        /// </summary>
        /// <param name="listParams">
        /// The list params.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int ExecuteListParams(ListParams listParams)
        {
            var allUsers = userService.GetAllUsers();
            var builder = new StringBuilder();
            foreach (var userData in allUsers)
            {
                builder.AppendLine($"{userData.Id}. {userData.User} : {userData.CreatedOn}");
            }

            Console.WriteLine();
            Console.WriteLine(builder.ToString());
            return 0;
        }

        /// <summary>
        /// The execute run params.
        /// </summary>
        /// <param name="runParams">
        /// The run params.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        private static int ExecuteRunParams(RunParams runParams)
        {
            var config = configService.ReadConfiguration();
            if (string.IsNullOrEmpty(config.SteamPath))
            {
                throw new ApplicationException("The steam path must be configured before execution login. Please run '<app> config -s <full_path_to_steam>'");
            }

            if (!File.Exists(config.SteamPath))
            {
                throw new ApplicationException("Looks like steam path is not valid. Please run 'config' with correct steam path once more.");
            }

            if (runParams.Number > 0 && !string.IsNullOrEmpty(runParams.User))
            {
                throw new ApplicationException("Both user number and name are provided. Only one of the following fields can be used.");
            }

            if (runParams.Number > 0)
            {
                steamService.RunForAccount(runParams.Number);
                return 0;
            }

            if (!string.IsNullOrEmpty(runParams.User))
            {
                steamService.RunForAccount(runParams.User);
                return 0;
            }

            throw new ApplicationException("For deleting the user entry, at least valid number or a name must be provided.");
        }

        /// <summary>
        /// The execut add params.
        /// </summary>
        /// <param name="addParams">
        /// The add params.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int ExecutAddParams(AddParams addParams)
        {
            var user = new UserData { User = addParams.User, Secret = addParams.Secret };
            userService.AddUser(user);
            return 0;
        }

        private static void ExitWithWait(int exitCode)
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Environment.Exit(exitCode);
        }
    }
}
