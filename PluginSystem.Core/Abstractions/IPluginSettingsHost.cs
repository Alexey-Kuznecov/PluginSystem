using PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core.Abstractions.Settings;

public interface IPluginSettingsHost
{
    IPluginSettingsService SettingsService { get; }
    void RegisterSetting(object setting);
}