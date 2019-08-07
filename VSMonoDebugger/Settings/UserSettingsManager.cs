﻿using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using NLog;
using System;

namespace VSMonoDebugger.Settings
{
    public class UserSettingsManager
    {
        public readonly static string SETTINGS_STORE_NAME = "VSMonoDebugger";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly UserSettingsManager manager = new UserSettingsManager();
        private WritableSettingsStore store;

        private UserSettingsManager()
        {
        }

        public static UserSettingsManager Instance
        {
            get { return manager; }
        }

        public UserSettingsContainer Load(string appDirectoryPath)
        {
            var result = new UserSettingsContainer();

            if (store.CollectionExists(SETTINGS_STORE_NAME))
            {
                try
                {
                    string content = store.GetString(SETTINGS_STORE_NAME, "Settings");
                    result = UserSettingsContainer.DeserializeFromJson(content, appDirectoryPath);
                    return result;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }

            return result;
        }

        public void Save(UserSettingsContainer settings)
        {
            string json = settings.SerializeToJson();
            if (!store.CollectionExists(SETTINGS_STORE_NAME))
            {
                store.CreateCollection(SETTINGS_STORE_NAME);
            }
            store.SetString(SETTINGS_STORE_NAME, "Settings", json);
        }

        public static void Initialize(Package package)
        {
            if (package != null)
            {
                var settingsManager = new ShellSettingsManager(package);
                var configurationSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
                Instance.store = configurationSettingsStore;
            }
            else
            {
                throw new ArgumentNullException("package argument was null");
            }
        }
    }
}