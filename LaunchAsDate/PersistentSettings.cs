using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace FSTools {
    public class PersistentSettings : Component {
        public delegate void PersistentSettingsEventHandler(object sender, RegistryKey registryKey);
        public event PersistentSettingsEventHandler SettingsLoaded;
        public event PersistentSettingsEventHandler SettingsSaved;
        public event PersistentSettingsEventHandler RegistryAccessFailed;

        private string registryPath;
        private RegistryKey registryKeyReadOnly, registryKeyWritable;

        public PersistentSettings() {
            registryPath = Path.Combine("Software", Application.CompanyName, Application.ProductName);
            registryKeyReadOnly = null;
            registryKeyWritable = null;
        }

        public void Save(string valueName, object value) {
            try {
                if (registryKeyWritable == null) {
                    registryKeyWritable = Registry.CurrentUser.CreateSubKey(registryPath);
                }
                registryKeyWritable.SetValue(valueName, value == null ? string.Empty : value.GetType() == typeof(bool) ? (bool)value ? 1 : 0 : value);
                SettingsSaved?.Invoke(this, registryKeyWritable);
            } catch (SecurityException exception) {
                Debug.WriteLine(exception);
                RegistryAccessFailed?.Invoke(this, registryKeyWritable);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
            }
        }

        public T Load<T>(string valueName, T defaultValue) {
            try {
                if (registryKeyReadOnly == null) {
                    registryKeyReadOnly = Registry.CurrentUser.OpenSubKey(registryPath);
                }
                if (registryKeyReadOnly != null) {
                    object value = registryKeyReadOnly.GetValue(valueName, defaultValue);
                    SettingsLoaded?.Invoke(this, registryKeyReadOnly);
                    if (typeof(T) == typeof(bool)) {
                        return (T)Convert.ChangeType(Convert.ToInt32(value) > 0, typeof(T));
                    } else {
                        return (T)value;
                    }
                }
                return defaultValue;
            } catch (SecurityException exception) {
                Debug.WriteLine(exception);
                RegistryAccessFailed?.Invoke(this, registryKeyReadOnly);
                return defaultValue;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                return defaultValue;
            }
        }

        public T Load<T>(string valueName) {
            try {
                if (registryKeyReadOnly == null) {
                    registryKeyReadOnly = Registry.CurrentUser.OpenSubKey(registryPath);
                }
                if (registryKeyReadOnly != null) {
                    object value = registryKeyReadOnly.GetValue(valueName, null);
                    SettingsLoaded?.Invoke(this, registryKeyReadOnly);
                    if (typeof(T) == typeof(bool)) {
                        return (T)Convert.ChangeType((int)value > 0, typeof(T));
                    } else {
                        return (T)value;
                    }
                }
                return default(T);
            } catch (SecurityException exception) {
                Debug.WriteLine(exception);
                RegistryAccessFailed?.Invoke(this, registryKeyReadOnly);
                return default(T);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
                return default(T);
            }
        }

        public void Clear() {
            try {
                Registry.CurrentUser.DeleteSubKeyTree(registryPath);
            } catch (Exception exception) {
                Debug.WriteLine(exception);
            }
        }

        public string RegistryPath {
            get {
                return registryPath;
            }
        }
    }
}
