using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Com.Apdcomms.CdeApi;
using KOps.Application;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KOps.CdeApi
{
    public class CdeApi : ICdeApi
    {
        private readonly string localDirectory;
        private readonly ILogger logger;
        private readonly ICde cde;

        public CdeApi(ILogger<CdeApi> logger, IMediator mediator)
        {
            localDirectory = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);

            cde = new Cde(localDirectory);

            this.logger = logger;

            Calls = new CdeCallsApi(logger, cde, mediator);
        }

        public ICdeCallsApi Calls { get; }

        public async Task LoginAsync(string id)
        {
            var archieveLogs = Path.Combine(localDirectory, "archieve logs");

            if (!Directory.Exists(archieveLogs))
            {
                Directory.CreateDirectory(archieveLogs);
            }

            var logFiles = Directory.GetFiles(localDirectory, "*.log");

            foreach (var logFile in logFiles)
            {
                var filename = Path.GetFileName(logFile);
                var newLogFile = Path.Combine(archieveLogs, filename);

                if (File.Exists(newLogFile))
                {
                    File.Delete(newLogFile);
                }
                File.Move(logFile, newLogFile);
            }

            var dbFiles = Directory.GetFiles(localDirectory, "*.db");

            foreach (var dbFile in dbFiles)
            {
                File.Delete(dbFile);
            }

            logger.LogWarning("Starting..");

            try
            {
                await cde.Initialize();
            }
            catch (Exception ex)
            {
                const string message = "Failed to Initialize";
                logger.LogError(ex, message);
                throw new Exception(message, ex);
            }

            if (cde.State != CdeState.Initialized)
            {
                // initialization error
            }

            try
            {
                await cde.Activate(id);
            }
            catch (Exception ex)
            {
                const string message = "Failed to Activate";
                logger.LogError(ex, message);
                throw new Exception(message, ex);
            }

            if (cde.State != CdeState.Activated)
            {
                // activation error
            }

            try
            {
                await cde.LogIn();
            }
            catch (Exception ex)
            {
                const string message = "Failed to LogIn";
                logger.LogError(ex, message);
                throw new Exception(message, ex);
            }

            if (cde.State != CdeState.LoggedIn)
            {
                // login error
            }

            logger.LogWarning("Logged In!");
        }
    }
}
