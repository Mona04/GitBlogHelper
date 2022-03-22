using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DirectoryHelper
{
    class DirHelper
    {
        static string Info =
@"=====================================================================
This program do 2 Works.

1. make Markdown documents for directory indices. 
2. append directory structures like ""- title: Categories"" in /_data/navigation.yml

As it reads setting.xml, navigation.yml and index.md, please set up that file. 
=====================================================================";

        static XmlNodeList xmlList;
        static string baseDir;
        static string postsDir;
        static string indexMD;
        static string[] navigation;
        static string[] SafeDirs(string InDir)
        {
            string[] dirs = { };
            try
            {
                dirs = Directory.GetDirectories(InDir);             
            }
            catch
            {
                Console.WriteLine("Invalid Directory");
            }
            return dirs;
        }

        public static void Do()
        {
            Console.WriteLine(Info);

            Console.WriteLine("If ready, press Enter.");
            Console.ReadLine();

            XmlDocument xml_setting = new XmlDocument();
            xml_setting.Load("setting.xml");

            xmlList    = xml_setting.SelectNodes("/setting");
            navigation = File.ReadAllLines("navigation.yml");
            indexMD    = File.ReadAllText("index.md");
            baseDir    = xmlList.Item(0)["GitBlogFolder"].InnerText;
            postsDir   = xmlList.Item(0)["PostFolder"].InnerText;

            foreach (XmlNode xnl in xmlList) {
                Console.WriteLine();
            }

   
            IndexMDCreate(postsDir);
            Navigation(baseDir, postsDir, navigation);
        }

        static void IndexMDCreate(string InDir)
        {
            string[] dirs = SafeDirs(InDir);

            foreach (string dir in dirs)
            {
                DirectoryInfo cur_info = new DirectoryInfo(dir);
                if(cur_info.Name.ToLower() == "_posts")
                    continue;
                
                string md_file_name = cur_info.Parent.FullName + "/" + cur_info.Name + ".md";
                string content = indexMD.Replace("CATEGORY", cur_info.Name);
                File.WriteAllText(md_file_name, content);
                IndexMDCreate(dir);
            }
        }

        static void Navigation(in string InBaseDir, in string InPostDir, in string[] InNaviYml)
        {
            string navigation_yml = InBaseDir + "/_data/navigation.yml";

            List<string> added = new List<string>();
            Navi_Recursive(1, InPostDir, InBaseDir, ref added);

            using (StreamWriter sw = new StreamWriter(navigation_yml))
            {
                foreach(var nav in InNaviYml)
                    sw.WriteLine(nav);
                sw.WriteLine();
                foreach (var nav in added)
                    sw.WriteLine(nav);
                sw.WriteLine();
            }
        }

        static void Navi_Recursive(int level, in string InDir, in string InBaseDir, ref List<string> Out)
        {
            string[] dirs = SafeDirs(InDir);

            if (dirs.Length == 0 || (dirs.Length == 1 && new DirectoryInfo(dirs[0]).Name.ToLower() == "_posts"))
                return;

            string offset = "";
            for (int i = 0; i < level; i++) offset += "    ";
            
            Out.Add(offset + "children:\n");

            foreach (string dir in dirs)
            {
                DirectoryInfo info = new DirectoryInfo(dir);
                if (info.Name.ToLower() == "_posts")
                    continue;

                Out.Add(offset + "  - title: \"" + info.Name + "\"");
                Out.Add(offset + "    url: /" + Path.GetRelativePath(InBaseDir, info.FullName).Replace("\\", "/") + "/");

                Navi_Recursive(level + 1, dir, InBaseDir, ref Out);
            }

            Out.Add("\n");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DirHelper.Do();
        }
    }
}
