using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZeroStack.ProjectRename
{
    class Program
    {
        static void Main(string[] args)
        {
            CwAbout();
            ProjectRename();
        }

        /// <summary>
        /// 项目欢迎
        /// </summary>
        static void CwAbout()
        {
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("  ============欢迎访问项目名称修改工具============");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("请输入您所需要修改的项目名称：");
            NewProjectName = Console.ReadLine();
        }
        /// <summary>
        /// 项目重命名
        /// </summary>
        static void ProjectRename()
        {
            //  1.获取方案路径
            var slnPath = GetSlnPath(Directory.GetCurrentDirectory());
            if (string.IsNullOrEmpty(slnPath))
                return;

            //  2.获取解决方案下所有的文件路径
            List<string> pathList = new List<string>();
            GetDirectories(slnPath, ref pathList);

            //  3.替换内容
            MoveDirectories(pathList);
        }

        #region 替换
        /// <summary>
        /// 替换资源
        /// </summary>
        /// <param name="list"></param>
        static void MoveDirectories(List<string> list)
        {
            foreach (var item in list)
            {
                FileInfo info = new FileInfo(item);
                var newdir = item.Replace(OldProjectName, NewProjectName);
                ReplaceInfo(item, newdir);
            }
            Console.WriteLine("替换完成，请重新打开解决方案！");
        }

        /// <summary>
        /// 替换内容
        /// </summary>
        /// <param name="olddir"></param>
        /// <param name="newdir"></param>
        static void ReplaceInfo(string olddir, string newdir)
        {
            FileInfo info = new FileInfo(newdir);
            DirectoryInfo d = info.Directory;
            if (!d.Exists)
            {
                d.Create();
            };
            File.Move(olddir, newdir);

            var textStreamReader = new StreamReader(newdir);
            string content = textStreamReader.ReadToEnd();
            textStreamReader.Close();
            var newContent = content.Replace(OldProjectName, NewProjectName);

            File.WriteAllText(newdir, newContent);
        }
        #endregion

        #region 获取
        readonly static List<string> IgnoreDirec = new List<string>() { ".git", ".vs", "bin", "obj" };

        /// <summary>
        /// 获取文件夹下的所有文件
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="list"></param>
        static void GetDirectories(string dirPath, ref List<string> list)
        {

            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
            foreach (FileInfo info in directoryInfo.GetFiles("*"))
            {
                Console.WriteLine(info.FullName);
                list.Add(info.FullName);
            }
            foreach (DirectoryInfo info in directoryInfo.GetDirectories())
            {
                if (IgnoreDirec.Contains(info.Name))
                {
                    continue;
                }
                GetDirectories(info.FullName, ref list);
            }

        }

        /// <summary>
        /// 获取解决方案的路径
        /// </summary>
        /// <param name="currentDirectory"></param>
        /// <returns></returns>
        private static string GetSlnPath(string currentDirectory)
        {
            var sln = Directory.GetFiles(currentDirectory, "*.sln");
            if (sln.Length == 0)
            {
                var parentDirectory = Directory.GetParent(currentDirectory);
                if (parentDirectory == null)
                {
                    Console.WriteLine("未找到解决项目的解决方案，请检查！");
                    return null;
                }
                return GetSlnPath(parentDirectory.FullName);
            }
            else
            {
                OldProjectName = Path.GetFileNameWithoutExtension(sln[0]);
                return currentDirectory;
            }
        }
        #endregion


        /// <summary>
        /// 旧项目名称
        /// </summary>
        private static string OldProjectName { get; set; }
        /// <summary>
        /// 新项目名称
        /// </summary>
        private static string NewProjectName { get; set; }

    }
}
