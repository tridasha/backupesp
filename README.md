# backupesp
Used to backup an fallout 4 mod.

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
