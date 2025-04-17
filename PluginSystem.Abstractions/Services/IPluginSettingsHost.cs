
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Abstractions.Services;

public interface IPluginSettingsHost
{
    IPluginSettingsService SettingsService { get; }
    void RegisterSetting(object setting);
}