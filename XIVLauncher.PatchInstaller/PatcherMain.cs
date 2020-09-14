﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Serilog;
using XIVLauncher.PatchInstaller.PatcherIpcMessages;
using XIVLauncher.PatchInstaller.ZiPatch;
using XIVLauncher.PatchInstaller.ZiPatch.Util;

namespace XIVLauncher.PatchInstaller
{
    public class PatcherMain
    {
        public const string BASE_GAME_VERSION = "2012.01.01.0000.0000";

        public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
            TypeNameHandling = TypeNameHandling.All
        };

        static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.Debug()
                    .MinimumLevel.Verbose()
                    .CreateLogger();

                if (args.Length > 1)
                {
                    try
                    {
                        if (InstallPatch(args[0], args[1]))
                        {
                            Log.Information("OK");
                        }
                        else
                        {
                            Environment.Exit(-1);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Patch installation failed.");
                        Environment.Exit(-1);
                    }

                    Environment.Exit(0);
                    return;
                }
                else
                {
                    Console.WriteLine("Missing Arguments.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Patcher init failed.\n\n" + ex);
            }

            return;

            if (args.Length == 3)
            {
                var patchlist = new[]
                {
                    "4e9a232b/H2017.06.06.0000.0001a.patch",
                    "4e9a232b/H2017.06.06.0000.0001b.patch",
                    "4e9a232b/H2017.06.06.0000.0001c.patch",
                    "4e9a232b/H2017.06.06.0000.0001d.patch",
                    "4e9a232b/H2017.06.06.0000.0001e.patch",
                    "4e9a232b/H2017.06.06.0000.0001f.patch",
                    "4e9a232b/H2017.06.06.0000.0001g.patch",
                    "4e9a232b/H2017.06.06.0000.0001h.patch",
                    "4e9a232b/H2017.06.06.0000.0001i.patch",
                    "4e9a232b/H2017.06.06.0000.0001j.patch",
                    "4e9a232b/H2017.06.06.0000.0001k.patch",
                    "4e9a232b/H2017.06.06.0000.0001l.patch",
                    "4e9a232b/H2017.06.06.0000.0001m.patch",
                    "4e9a232b/H2017.06.06.0000.0001n.patch",
                    "4e9a232b/D2017.07.11.0000.0001.patch",
                    "4e9a232b/D2017.09.24.0000.0001.patch",
                    "4e9a232b/D2017.10.11.0000.0001.patch",
                    "4e9a232b/D2017.10.31.0000.0001.patch",
                    "4e9a232b/D2017.11.24.0000.0001.patch",
                    "4e9a232b/D2018.01.12.0000.0001.patch",
                    "4e9a232b/D2018.02.09.0000.0001.patch",
                    "4e9a232b/D2018.04.27.0000.0001.patch",
                    "4e9a232b/D2018.05.26.0000.0001.patch",
                    "4e9a232b/D2018.06.19.0000.0001.patch",
                    "4e9a232b/D2018.07.18.0000.0001.patch",
                    "4e9a232b/D2018.09.05.0000.0001.patch",
                    "4e9a232b/D2018.10.19.0000.0001.patch",
                    "4e9a232b/D2018.12.15.0000.0001.patch",
                    "4e9a232b/D2019.01.26.0000.0001.patch",
                    "4e9a232b/D2019.03.12.0000.0001.patch",
                    "4e9a232b/D2019.03.15.0000.0001.patch",
                    "4e9a232b/D2019.04.12.0000.0001.patch",
                    "4e9a232b/D2019.04.16.0000.0000.patch",
                    "4e9a232b/D2019.05.08.0000.0001.patch",
                    "4e9a232b/D2019.05.09.0000.0000.patch",
                    "4e9a232b/D2019.05.29.0000.0000.patch",
                    "4e9a232b/D2019.05.29.0001.0000.patch",
                    "4e9a232b/D2019.05.31.0000.0001.patch",
                    "4e9a232b/D2019.06.07.0000.0001.patch",
                    "4e9a232b/D2019.06.18.0000.0001.patch",
                    "4e9a232b/D2019.06.22.0000.0000.patch",
                    "4e9a232b/D2019.07.02.0000.0000.patch",
                    "4e9a232b/D2019.07.09.0000.0000.patch",
                    "4e9a232b/D2019.07.10.0000.0001.patch",
                    "4e9a232b/D2019.07.10.0001.0000.patch",
                    "4e9a232b/D2019.07.24.0000.0001.patch",
                    "4e9a232b/D2019.07.24.0001.0000.patch",
                    "4e9a232b/D2019.08.20.0000.0000.patch",
                    "4e9a232b/D2019.08.21.0000.0000.patch",
                    "4e9a232b/D2019.10.11.0000.0000.patch",
                    "4e9a232b/D2019.10.16.0000.0001.patch",
                    "4e9a232b/D2019.10.19.0000.0001.patch",
                    "4e9a232b/D2019.10.24.0000.0000.patch",
                    "4e9a232b/D2019.11.02.0000.0000.patch",
                    "4e9a232b/D2019.11.05.0000.0001.patch",
                    "4e9a232b/D2019.11.06.0000.0000.patch",
                    "4e9a232b/D2019.11.19.0000.0000.patch",
                    "4e9a232b/D2019.12.04.0000.0001.patch",
                    "4e9a232b/D2019.12.05.0000.0000.patch",
                    "4e9a232b/D2019.12.17.0000.0000.patch",
                    "4e9a232b/D2019.12.19.0000.0000.patch",
                    "4e9a232b/D2020.01.31.0000.0000.patch",
                    "4e9a232b/D2020.01.31.0001.0000.patch",
                    "4e9a232b/D2020.02.10.0000.0001.patch",
                    "4e9a232b/D2020.02.11.0000.0000.patch",
                    "4e9a232b/D2020.02.27.0000.0000.patch",
                    "4e9a232b/D2020.02.29.0000.0001.patch",
                    "4e9a232b/D2020.03.04.0000.0000.patch",
                    "4e9a232b/D2020.03.12.0000.0000.patch",
                    "4e9a232b/D2020.03.24.0000.0000.patch",
                    "4e9a232b/D2020.03.25.0000.0000.patch",
                    "4e9a232b/D2020.03.26.0000.0000.patch",
                    "4e9a232b/D2020.03.27.0000.0000.patch",
                    /*"ex1/6b936f08/H2017.06.01.0000.0001a.patch",
                    "ex1/6b936f08/H2017.06.01.0000.0001b.patch",
                    "ex1/6b936f08/H2017.06.01.0000.0001c.patch",
                    "ex1/6b936f08/H2017.06.01.0000.0001d.patch",
                    "ex1/6b936f08/D2017.09.24.0000.0000.patch",
                    "ex1/6b936f08/D2018.09.05.0000.0001.patch",
                    "ex1/6b936f08/D2019.11.06.0000.0001.patch",
                    "ex1/6b936f08/D2020.03.04.0000.0001.patch",
                    "ex1/6b936f08/D2020.03.24.0000.0000.patch",
                    "ex1/6b936f08/D2020.03.26.0000.0000.patch",
                    "ex2/f29a3eb2/D2017.03.18.0000.0000.patch",
                    "ex2/f29a3eb2/D2017.05.26.0000.0000.patch",
                    "ex2/f29a3eb2/D2017.05.26.0001.0000.patch",
                    "ex2/f29a3eb2/D2017.05.26.0002.0000.patch",
                    "ex2/f29a3eb2/D2017.06.27.0000.0001.patch",
                    "ex2/f29a3eb2/D2017.09.24.0000.0001.patch",
                    "ex2/f29a3eb2/D2018.01.12.0000.0001.patch",
                    "ex2/f29a3eb2/D2018.02.23.0000.0001.patch",
                    "ex2/f29a3eb2/D2018.04.27.0000.0001.patch",
                    "ex2/f29a3eb2/D2018.07.18.0000.0001.patch",
                    "ex2/f29a3eb2/D2018.09.05.0000.0001.patch",
                    "ex2/f29a3eb2/D2018.12.14.0000.0001.patch",
                    "ex2/f29a3eb2/D2019.01.26.0000.0001.patch",
                    "ex2/f29a3eb2/D2019.03.12.0000.0001.patch",
                    "ex2/f29a3eb2/D2019.05.29.0000.0001.patch",
                    "ex2/f29a3eb2/D2019.12.04.0000.0001.patch",
                    "ex2/f29a3eb2/D2020.02.29.0000.0001.patch",
                    "ex2/f29a3eb2/D2020.03.24.0000.0000.patch",
                    "ex2/f29a3eb2/D2020.03.26.0000.0000.patch",
                    "ex3/859d0e24/D2019.04.02.0000.0000.patch",
                    "ex3/859d0e24/D2019.05.29.0000.0000.patch",
                    "ex3/859d0e24/D2019.05.29.0001.0000.patch",
                    "ex3/859d0e24/D2019.05.29.0002.0000.patch",
                    "ex3/859d0e24/D2019.07.09.0000.0001.patch",
                    "ex3/859d0e24/D2019.10.11.0000.0001.patch",
                    "ex3/859d0e24/D2020.01.31.0000.0001.patch",
                    "ex3/859d0e24/D2020.02.29.0000.0001.patch",
                    "ex3/859d0e24/D2020.03.24.0000.0000.patch",
                    "ex3/859d0e24/D2020.03.26.0000.0000.patch"*/
                };
                foreach (var s in patchlist)
                    InstallPatch(args[0] + s, args[1]);
                return;
            }

            if (args.Length == 1)
            {
                return;
            }

            Console.WriteLine("XIVLauncher.PatchInstaller\n\nUsage:\nXIVLauncher.PatchInstaller.exe <patch path> <game path> <repository>\nOR\nXIVLauncher.PatchInstaller.exe <pipe name>");
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private static bool InstallPatch(string patchPath, string gamePath)
        {
            try
            {
                Log.Debug("Installing {0} to {1}", patchPath, gamePath);

                using var patchFile = ZiPatchFile.FromFileName(patchPath);

                using (var store = new SqexFileStreamStore())
                {
                    var config = new ZiPatchConfig(gamePath) { Store = store };

                    foreach (var chunk in patchFile.GetChunks())
                        chunk.ApplyChunk(config);
                }

                Log.Debug("Patch {0} installed", patchPath);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Patch install failed.");
                return false;
            }
        }

        private static void VerToBck(DirectoryInfo gamePath)
        {
            Thread.Sleep(200);

            foreach (var repository in Enum.GetValues(typeof(Repository)).Cast<Repository>())
            {
                // Overwrite the old BCK with the new game version
                try
                {
                    repository.GetVerFile(gamePath).CopyTo(repository.GetVerFile(gamePath, true).FullName, true);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Could not copy to BCK");
                }
            }
        }
    }
}
