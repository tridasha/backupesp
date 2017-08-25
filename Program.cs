using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backupesp
{
    /*
    Written in Visual Studio 2015 - Community edition
        
    Console application
        
    Used to copy out files from a directory structure that exist under specific subdirectories
    (filter) or levels of subdirectory, and keeps directory structure for found subitems intact.
    Used to backup files which are part of a Fallout 4 mod as well as the mode itself.
    Each time it is run it will create a new back up direction with a appended index number.

    It should be run from the Fallout 4 data directory (\steam\steamapps\common\fallout 4\data\)

    For example:

    backupesp.exe c:\mystuff\backupsFO4 Artstop

    Where artstop is the name of the ESP without the extension.
    The program will create a back up directory in c:\mystuff\backupsFO4\Aststop_1
    If it is run again, it will create the directory as c:\mystuff\backupsFO4\Aststop_2 and so on.

    In that directory will be a full structure of files where it finds a subdirectory called Artstop.

    E.g.c:\mystuff\backupsFO4\Aststop_1\meshes\Artstop\myartmesh1.BTR
        c:\mystuff\backupsFO4\Aststop_1\meshes\Artstop\myartmesh2.BTR
        c:\mystuff\backupsFO4\Aststop_1\materials\Artstop\myartmesh1.BGSM
        and so on
        c:\mystuff\backupsFO4\Aststop_1\Artstop.ESP

        It copies Artstop[.esp] (it adds on the extension) into c:\mystuff\backupsFO4\Aststop_1

    The idea is that if you need to restore your mod(ESP) you can copy(drag/drop) the files
    from your backup into the fallout 4 data directory and it will create the directory structure
    as well as copy over the files.It's a way to keep versions of your mod in case something goes wrong.

    In some cases, you might have other directories you need to back up, such as a world space, that
    are part of your mod. You can specify these after the ESP root name.
    E.g.backupesp.exe c:\mystuff\backupsFO4 adventuresthelost myworldspaceisland1 mywsp2


    It goes without saying, this comes without any warranty of any kind, and you use it at your own risk.

    Released under Creative Commons license CC BY-NC-SA 4.0 https://creativecommons.org/licenses/by-nc-sa/4.0/
    released under this only because I'm using the free version of Visual Studio 2015 and it requires non-commercial use. 
    Note, I don't need any attributions even though CC BY-NC-SA 4.0 requires it.

    To build this source, get the Visual Studio Community Edition (https://www.visualstudio.com/vs/community/)
    Use the Visual Studio wizard to create a new empty c# console application, giving it the name of what you want
    the console application to be called, e.g backupesp
    Cut and past this code in to replace the wizard generated console application and hit ctrl-shift-b to build it.

    Triash - I'm not really a c# coder.
    */

    public class Program
    {
        static string fixdir(string dir, bool bStart = false) 
        {
            char[] charsToTrim = { ' ', '\\' };
            // adds trailing seperater if it doesn't exist
            if (dir != "")
            {
                if (bStart)
                    return dir.TrimStart(charsToTrim);
                else
                    return dir.Trim(charsToTrim) + "\\";
            }
            return dir;
        }

        static string MakebackupDir(string dst, string filter)
        {
            // No dest we dont need to do anything, still a valid op as it's used for testing.
            if (dst == "")
                return dst;

            string path = fixdir(dst) + fixdir(filter, true);

             for (int i = 1; i < 255; i++)
            {
                string pbk = path + "_" + i;
                if (!System.IO.Directory.Exists(pbk))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(pbk);
                        Console.WriteLine("Backup directory created {0}", pbk);
                    }
                    catch (System.Exception excpt)
                    {
                        System.Console.WriteLine(excpt.Message);
                    }

                    return pbk;
                }
            }

            System.Console.WriteLine("Unable to create backup directory using base {0}", path);
            return null;
        }

        static void CopyFile(string root, string dst, string f)
        {
            try
            {

                string newname = f.Remove(0, root.Length);
                string dstname = fixdir(dst) + fixdir(newname, true);
                string path = System.IO.Path.GetDirectoryName(dstname);

                // Determine whether the directory exists. If it doesn't create it.
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                // Copy over the file
                System.IO.File.Copy(f, dstname, true);
                Console.WriteLine("created {0}", dstname);
            }
            catch (System.Exception excpt)
            {
                System.Console.WriteLine(excpt.Message);
            }
        }

        static void DirSearch(string dst, string root, string sDir, string filter, bool bBelowesp = false)
        {
            try
            {
                foreach (string f in System.IO.Directory.GetFiles(sDir, "*.*"))
                {
                    if (bBelowesp)
                    {
                        if (dst == "") // dont copy we dont have a destination, just report it, for testing.
                            System.Console.WriteLine(f);
                        else
                            CopyFile(root, dst, f);
                    }
                }

                // Recurse down the tree if we find another directory
                foreach (string d in System.IO.Directory.GetDirectories(sDir))
                {
                    var dirName = new System.IO.DirectoryInfo(d).Name;
                    if (dirName.ToLower() == filter.ToLower())
                        DirSearch(dst, root, d, filter, true);
                    else
                        DirSearch(dst, root, d, filter, bBelowesp);
                }
            }
            catch (System.Exception excpt)
            {
                System.Console.WriteLine(excpt.Message);
            }
        }

        static void Main(string[] args)
        {
            String dest, root, start;
            String [] filter = new string [10];
            int len = 0;

            if (args.Length < 2 || args.Length > 11)
            {
                System.Console.WriteLine("usage: dest subdir [subdir2] [subdir3] ... [subdir10]");
                System.Console.WriteLine("dest      - directory to back the files up in.");
                System.Console.WriteLine("subdir    - subdirectory where esp files are under (this is the esp file name without it's extension).");
                System.Console.WriteLine("subdir[n] - any other subdirectories which might need backing up.");
                return;
                            
                // for debugging under VS, fills in the parameters, need to comment out return above
                /*
                dest = ""; // blank for testing, debugging, doesn't perform the copy just finds the files
                root = "D:\\steam\\steamapps\\common\\Fallout 4\\Data";
                start = root;
                filter[0] = "esptest";
                len = 1;
                */
            }
            else
            {
                dest = args[0];
                root = Environment.CurrentDirectory;
                start = root;
                len = args.Length - 1;
                for (int j = 1; j<=len; j++)
                    filter[j-1] = args[j];
            }

            // Create the backup directory
            string bkupdir = MakebackupDir(dest, filter[0]);
            if (bkupdir != null)
            {
                for (int j = 0; j < len; j++)
                    if (filter[j].Length > 0)
                    {
                        DirSearch(bkupdir, root, start, filter[j]);
                        DirSearch(bkupdir, root, start, filter[j] + ".esp");  // pick up any files which might be in directories that include .esp extension
                    }

                // Save the esp itself
                try
                {
                    string srcname = fixdir(root) + fixdir(filter[0], true) + ".esp";
                    string dstname = fixdir(bkupdir) + fixdir(filter[0], true) + ".esp";
                    System.IO.File.Copy(srcname, dstname, true);
                    Console.WriteLine("created {0}", dstname);
                }
                catch (System.Exception excpt)
                {
                    System.Console.WriteLine(excpt.Message);
                }
            }
        }
    }
}
