using System;

namespace steamLauncher.DummyProcessLauncher
{
    using System.Diagnostics;

    class Program
    {
        static int Main(string[] args)
        {
            if (args == null)
            {
                return (int)ExitCodes.ArgsNull;
            }

            if (args.Length != 2)
            {
                return (int)ExitCodes.ArgsNumber;
            }

            try
            {
                Process.Start(args[0], args[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return (int)ExitCodes.GenericError;
            }

            return (int)ExitCodes.Ok;
        }

        [Flags]
        private enum ExitCodes
        {
            Ok = 0,
            GenericError,
            ArgsNull,
            ArgsNumber
        }
    }
}
