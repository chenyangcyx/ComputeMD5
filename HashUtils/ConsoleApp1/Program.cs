﻿namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || !File.Exists(args[0]))
            {
                Console.WriteLine("没有输入配置文件！");
                Environment.Exit(1);
            }
            SettingStruct.SettingConfig setting = Utilities.getSetting(args[0]);

            // 检查设置选项
            // 检查check_folder_config_file是否存在
            if (!File.Exists(setting.check_folder_config_file))
            {
                Console.WriteLine("hash文件夹配置文件" + setting.check_folder_config_file + "不存在！");
                Environment.Exit(1);
            }

            string programFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            // 检查blake程序是否存在
            string blake2_exe_path1 = programFolderPath + setting.blake2_exe_path;
            string blake2_exe_path2 = setting.blake2_exe_path;
            if (File.Exists(blake2_exe_path1))
            {
                setting.blake2_exe_path = blake2_exe_path1;
            }
            else if (File.Exists(blake2_exe_path2))
            {
                setting.blake2_exe_path = blake2_exe_path2;
            }
            else
            {
                Console.WriteLine("BLAKE2程序" + setting.blake2_exe_path + "不存在！");
                Environment.Exit(1);
            }
            string blake3_exe_path1 = programFolderPath + setting.blake3_exe_path;
            string blake3_exe_path2 = setting.blake3_exe_path;
            if (File.Exists(blake3_exe_path1))
            {
                setting.blake3_exe_path = blake3_exe_path1;
            }
            else if (File.Exists(blake3_exe_path2))
            {
                setting.blake3_exe_path = blake3_exe_path2;
            }
            else
            {
                Console.WriteLine("BLAKE3程序" + setting.blake3_exe_path + "不存在！");
                Environment.Exit(1);
            }

            // 读入检查目录内容
            string check_folder_path1 = programFolderPath + setting.check_folder_config_file;
            string check_folder_path2 = setting.check_folder_config_file;
            if (File.Exists(check_folder_path1))
            {
                setting.check_folder_config_file = check_folder_path1;
            }
            else if (File.Exists(check_folder_path2))
            {
                setting.check_folder_config_file = check_folder_path2;
            }
            else
            {
                Console.WriteLine("检查目录" + setting.check_folder_config_file + "不存在！");
                Environment.Exit(1);
            }
            List<string> check_folder_list = new List<string>();
            using (StreamReader stream_reader = new StreamReader(setting.check_folder_config_file, Utilities.utf8_encoding))
            {
                while (!stream_reader.EndOfStream)
                {
                    string line = stream_reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    check_folder_list.Add(line);
                }
            }
            // 输出设置
            Console.WriteLine("所有设置：");
            Console.WriteLine("## check_folder");
            int no = 0;
            foreach (var path in check_folder_list)
                Console.WriteLine((++no) + ". " + path);
            Console.WriteLine("## blake2_exe_path: " + setting.blake2_exe_path);
            Console.WriteLine("## blake3_exe_path: " + setting.blake3_exe_path);
            // 设置hash_method_name, generate_method_use
            List<string> hash_method_name = new List<string>();
            List<bool> generate_method_use = new List<bool>();
            setHashMethod(setting, hash_method_name, generate_method_use);
            // 设置verify_method_use
            List<bool> verify_method_use = new List<bool>();
            setCheckMethod(setting, verify_method_use);
            // 输出generate_method
            Console.Write("## generate_method: ");
            List<string> generateMethodList = new List<string>();
            for (int i = 0; i < hash_method_name.Count; i++)
            {
                if (generate_method_use[i])
                {
                    generateMethodList.Add(hash_method_name[i]);
                }
            }
            Console.WriteLine(string.Join(", ", generateMethodList));
            // 输出verify_method
            Console.Write("## verify_method: ");
            List<string> verifyMethodList = new List<string>();
            for (int i = 0; i < hash_method_name.Count; i++)
            {
                if (verify_method_use[i])
                    verifyMethodList.Add((hash_method_name[i]));
            }
            Console.WriteLine(string.Join(", ", verifyMethodList));

            Console.WriteLine("## forecast_remain_time: " + (setting.forecast_remain_time == 1));

            // 选择运行模式
            Console.WriteLine("请选择程序的运行模式：");
            Console.WriteLine("[1] 生成hash文件");
            Console.WriteLine("[2] 校验文件hash");
            Console.WriteLine("[3] 查看并输出setting文件demo");
            Console.WriteLine("[4] 查看NET8功能支持情况");
            string chooseNo = Console.ReadLine();
            switch (chooseNo)
            {
                case "1":
                    Console.WriteLine("选择了：[1] 生成hash文件\n");
                    Controller.generateHash(setting, check_folder_list, hash_method_name, generate_method_use);
                    break;
                case "2":
                    Console.WriteLine("选择了：[2] 校验文件hash\n");
                    Controller.checkHash(setting, check_folder_list, hash_method_name, verify_method_use);
                    break;
                    case "3":
                    Console.WriteLine("选择了：[3] 查看并输出setting文件demo\n");

                    break;
                    case "4":
                    Console.WriteLine("选择了：[4] 查看当前系统信息\n");
                    Controller.checkSystemInfo();
                    break;
                default:
                    break;
            }
        }

        static void setHashMethod(SettingStruct.SettingConfig setting, List<string> hash_method_name, List<bool> generate_method_use)
        {
            // 设置hash_method_name
            hash_method_name.Add(Utilities.MD5_NAME);
            hash_method_name.Add(Utilities.SHA1_NAME);
            hash_method_name.Add(Utilities.SHA2_256_NAME);
            hash_method_name.Add(Utilities.SHA2_384_NAME);
            hash_method_name.Add(Utilities.SHA2_512_NAME);
            hash_method_name.Add(Utilities.SHA3_224_NAME);
            hash_method_name.Add(Utilities.SHA3_256_NAME);
            hash_method_name.Add(Utilities.SHA3_384_NAME);
            hash_method_name.Add(Utilities.SHA3_512_NAME);
            hash_method_name.Add(Utilities.SHAKE128_NAME);
            hash_method_name.Add(Utilities.SHAKE256_NAME);
            hash_method_name.Add(Utilities.BLAKE2b_NAME);
            hash_method_name.Add(Utilities.BLAKE2s_NAME);
            hash_method_name.Add(Utilities.BLAKE2bp_NAME);
            hash_method_name.Add(Utilities.BLAKE2sp_NAME);
            hash_method_name.Add(Utilities.BLAKE3_NAME);
            /** MD5 **/
            if (setting.generate_method.md5 != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            /** MD5 **/

            /** SHA1 **/
            if (setting.generate_method.sha1 != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            /** SHA1 **/

            /** SHA2 **/
            if (setting.generate_method.sha2_256 != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            if (setting.generate_method.sha2_384 != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            if (setting.generate_method.sha2_512 != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            /** SHA2 **/

            /** SHA3 **/
            if (setting.generate_method.sha3_224 != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            if (setting.generate_method.sha3_256 != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            if (setting.generate_method.sha3_384 != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            if (setting.generate_method.sha3_512 != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            if (setting.generate_method.shake128 != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            if (setting.generate_method.shake256 != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            /** SHA3 **/

            /** BLAKE2 **/
            if (setting.generate_method.blake2b != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            if (setting.generate_method.blake2s != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            if (setting.generate_method.blake2bp != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            if (setting.generate_method.blake2sp != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            /** BLAKE2 **/

            /** BLAKE3 **/
            if (setting.generate_method.blake3 != 0)
                generate_method_use.Add(true);
            else
                generate_method_use.Add(false);
            /** BLAKE3 **/
        }

        static void setCheckMethod(SettingStruct.SettingConfig setting, List<bool> verify_method_use)
        {
            if (setting.verify_method.md5 != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);

            if (setting.verify_method.sha1 != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);

            if (setting.verify_method.sha2_256 != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);
            if (setting.verify_method.sha2_384 != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);
            if (setting.verify_method.sha2_512 != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);

            if (setting.verify_method.sha3_224 != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);
            if (setting.verify_method.sha3_256 != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);
            if (setting.verify_method.sha3_384 != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);
            if (setting.verify_method.sha3_512 != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);
            if (setting.verify_method.shake128 != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);
            if (setting.verify_method.shake256 != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);

            if (setting.verify_method.blake2b != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);
            if (setting.verify_method.blake2s != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);
            if (setting.verify_method.blake2bp != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);
            if (setting.verify_method.blake2sp != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);

            if (setting.verify_method.blake3 != 0)
                verify_method_use.Add(true);
            else
                verify_method_use.Add(false);
        }
    }
}