using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Com.Apdcomms.CdeApi;
using Com.Apdcomms.CdeApi.Affiliations;
using Com.Apdcomms.CdeApi.Subscription.Configuration;
using KOps.Application;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KOps.CdeApi
{
    public class CdeApi : ICdeApi
    {
        private readonly string localDirectory;
        private readonly ILogger logger;
        private readonly CdeCalls calls;
        private readonly CdeAudio audio;
        private readonly CdeGroups groups;
        private readonly ICde cde;

        public CdeApi(ILogger<CdeApi> logger, IMediator mediator)
        {
            this.logger = logger;
            
            localDirectory = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);

            cde = new Cde(localDirectory);

            calls = new CdeCalls(logger, cde, mediator);
            audio = new CdeAudio(logger, cde, mediator);
            groups = new CdeGroups(logger, cde, mediator);

            cde.Affiliations.AffiliationStateChanged += Affiliations_AffiliationStateChanged;
            cde.SubscriberConfigurationChanged += Cde_SubscriberConfigurationChanged;

            cde.LogEnabled = true;
            cde.LogFilterOptions.FatalErrorEnabled = true;
            cde.LogFilterOptions.ErrorEnabled = true;
            cde.LogFilterOptions.WarningEnabled= true;
            cde.LogFilterOptions.InformationEnabled = true;
            cde.LogFilterOptions.VerboseEnabled= true;
            cde.LogFilterOptions.DebugEnabled = true;

            cde.LogMessageAvailable += Cde_LogMessageAvailable;
        }

        private void Cde_LogMessageAvailable(object sender, string e)
        {
            //logger.LogDebug("[{EventName}] {@EventArgs}", "LogMessageAvailable", e);
        }

        private void Cde_SubscriberConfigurationChanged(object sender, IEnumerable<CdeSubscriberConfiguration> e)
        {
            foreach (var item in e)
            {
                logger.LogInformation("[{EventName}] {@EventArgs}", "SubscriberConfigurationChanged", item);
            }
        }

        private void Affiliations_AffiliationStateChanged(object sender, AffiliationInformationEventArgs e)
        {
            logger.LogInformation("[{EventName}] {@EventArgs}", "AffiliationStateChanged", e);
        }

        public async Task MakeGroupCallAsync(string groupId)
        {
            await calls.MakeGroupCallAsync(groupId);
        }

        public async Task AcquireFloor()
        {            
            await calls.AcquireFloor();
            audio.StartCapture();
        }

        public async Task ReleaseFloor()
        {
            await calls.ReleaseFloor();
            audio.StopCapture();
        }

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

            cde.SetAudioPlaybackInterval(10u);
            cde.SetAudioCaptureInterval(20u);

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
